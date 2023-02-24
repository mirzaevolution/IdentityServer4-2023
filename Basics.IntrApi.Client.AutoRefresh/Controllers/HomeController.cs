using Basics.IntrApi.Client.AutoRefresh.Models;
using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Basics.IntrApi.Client.AutoRefresh.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        [Authorize]
        public async Task<IActionResult> Protected()
        {
            var authenticateResult = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            string[] tokenResponses =
            {
                OidcConstants.TokenResponse.IdentityToken,
                OidcConstants.TokenResponse.AccessToken,
                OidcConstants.TokenResponse.RefreshToken,
                OidcConstants.TokenResponse.Scope,
                OidcConstants.TokenResponse.IssuedTokenType,
                OidcConstants.TokenResponse.TokenType,
                OidcConstants.TokenResponse.ExpiresIn,
                "expires_at"
            };
            List<ProtectedItemModel> protectedItemModels = new List<ProtectedItemModel>();
            foreach (string tokenResponse in tokenResponses)
            {
                protectedItemModels.Add(new ProtectedItemModel
                {
                    Key = tokenResponse,
                    Value = await HttpContext.GetTokenAsync(tokenResponse) ?? "-"
                });
            }
            foreach (var claim in User.Claims)
            {
                protectedItemModels.Add(new ProtectedItemModel
                {
                    Key = claim.Type,
                    Value = claim.Value
                });
            }
            return View(protectedItemModels);

        }



        public IActionResult Index()
        {
            return View();
        }


    }
}