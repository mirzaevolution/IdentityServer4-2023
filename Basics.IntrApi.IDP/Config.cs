using IdentityServer4.Models;
using System;
using System.Collections.Generic;

namespace Basics.IntrApi.IDP
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email()
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("identity.read")
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {

                // interactive client using code flow + pkce
                new Client
                {
                    ClientId = "basics.intrapi",
                    ClientSecrets = { new Secret("49C1A7E1-0C79-4A89-A3D6-A37998FB86B0".Sha256()) },

                    AllowedGrantTypes = GrantTypes.Code,

                    RedirectUris =
                    {
                        "https://localhost:7166/signin-oidc",
                        "https://localhost:7017/signin-oidc"
                    },
                    PostLogoutRedirectUris =
                    {
                        "https://localhost:7166/signout-callback-oidc",
                        "https://localhost:7017/signout-callback-oidc"

                    },
                    AccessTokenLifetime = Convert.ToInt32(TimeSpan.FromMinutes(4).TotalSeconds),
                    AllowOfflineAccess = true,
                    AllowedScopes = { "openid", "profile", "email","offline_access","identity.read" }
                },
            };
    }
}