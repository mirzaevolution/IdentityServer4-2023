using Basics.IntrApi.Client.Models;
using IdentityModel;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Basics.IntrApi.Client.Services
{
    public class ApiInvokerService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        public ApiInvokerService(
                HttpClient httpClient,
                IHttpContextAccessor httpContextAccessor,
                IConfiguration configuration
            )
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _httpClient.BaseAddress = new Uri(_configuration["API:BaseAddress"]);
            _httpContextAccessor = httpContextAccessor;
        }

        private async Task<string> GetToken()
        {
            DateTimeOffset now = DateTimeOffset.UtcNow.AddMinutes(1);
            DateTimeOffset? expiresAt =
                DateTimeOffset.Parse(await _httpContextAccessor.HttpContext.GetTokenAsync("expires_at"));
            string? accessToken = await _httpContextAccessor.HttpContext.GetTokenAsync(OidcConstants.TokenTypes.AccessToken);
            if (now > expiresAt)
            {
                HttpClient httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(_configuration["IDP:Authority"]);
                DiscoveryDocumentResponse documentResponse = await httpClient.GetDiscoveryDocumentAsync();
                if (documentResponse != null && !documentResponse.IsError)
                {
                    string? currentRefreshToken = await
                        _httpContextAccessor.HttpContext.GetTokenAsync(OidcConstants.TokenTypes.RefreshToken);
                    TokenResponse refreshResponse = await httpClient.RequestRefreshTokenAsync(new RefreshTokenRequest
                    {
                        Address = documentResponse.TokenEndpoint,
                        RefreshToken = currentRefreshToken,
                        ClientId = _configuration["IDP:ClientId"],
                        ClientSecret = _configuration["IDP:ClientSecret"]
                    });
                    if (refreshResponse != null && !refreshResponse.IsError)
                    {
                        accessToken = refreshResponse.AccessToken;
                        List<AuthenticationToken> authenticationTokens = new List<AuthenticationToken>
                        {
                            new AuthenticationToken { Name = OidcConstants.TokenTypes.IdentityToken, Value = refreshResponse.IdentityToken },
                            new AuthenticationToken { Name = OidcConstants.TokenTypes.AccessToken, Value = refreshResponse.AccessToken },
                            new AuthenticationToken { Name = OidcConstants.TokenTypes.RefreshToken, Value = refreshResponse.RefreshToken  },
                            new AuthenticationToken { Name = "expires_at", Value = DateTimeOffset.UtcNow.Add(TimeSpan.FromSeconds(refreshResponse.ExpiresIn)).ToString("O") }
                        };
                        AuthenticateResult authenticationResult =
                            await _httpContextAccessor.HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                        if (authenticationResult != null && authenticationResult.Succeeded)
                        {
                            authenticationResult.Properties.StoreTokens(authenticationTokens);
                            await _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                                authenticationResult.Principal, authenticationResult.Properties);
                            return accessToken;
                        }
                        throw new Exception(authenticationResult.Failure?.Message ?? "An error occured while authenticating new tokens");

                    }
                    throw new Exception(refreshResponse.Error ?? "An error occured while retrieving refreshing token");
                }
                throw new Exception(documentResponse.Error ?? "An error occured while retrieving discovery document");

            }
            return accessToken;
        }

        public async Task<DataResponse?> Get(string path)
        {
            string token = await GetToken();
            _httpClient.SetBearerToken(token);
            DataResponse? response = await _httpClient.GetFromJsonAsync<DataResponse>(path);
            return response;
        }

    }
}
