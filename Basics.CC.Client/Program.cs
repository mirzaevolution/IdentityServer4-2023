using IdentityModel.Client;

namespace Basics.CC.Client
{
    internal class Program
    {
        static async Task GetIdentity()
        {
            string apiAddress = "https://localhost:7032";
            string idpAddress = "https://localhost:7031";
            string idpScope = "scope:full_access";
            string idpClientId = "12345";
            string idpClientSecret = "P@$sw0Rd";
            var client = new HttpClient();
            var discoveryEndpoint = await client.GetDiscoveryDocumentAsync(idpAddress);
            if (!discoveryEndpoint.IsError)
            {
                var tokenResult = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
                {
                    Address = discoveryEndpoint.TokenEndpoint,
                    Scope = idpScope,
                    ClientId = idpClientId,
                    ClientSecret = idpClientSecret
                });
                if (!tokenResult.IsError)
                {
                    var apiClient = new HttpClient();
                    apiClient.SetBearerToken(tokenResult.AccessToken);
                    var apiResult = await apiClient.GetStringAsync($"{apiAddress}/api/identity");
                    Console.WriteLine(apiResult);
                }
            }
        }
        static void Main(string[] args)
        {
            GetIdentity().Wait();
            Console.ReadLine();
        }
    }
}