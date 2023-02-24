// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Models;
using System.Collections.Generic;
namespace Basics.Intr.Idp
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
        public static IEnumerable<Client> Clients =>
            new Client[]
            {

                new Client
                {
                    ClientId = "interactive",
                    ClientSecrets = { new Secret("49C1A7E1-0C79-4A89-A3D6-A37998FB86B0".Sha256()) },

                    AllowedGrantTypes = GrantTypes.Code,

                    RedirectUris =
                    {
                        "https://localhost:7271/signin-oidc",
                    },
                    PostLogoutRedirectUris =
                    {
                        "https://localhost:7271/signout-callback-oidc"
                    },

                    AllowedScopes =
                    {
                        IdentityModel.OidcConstants.StandardScopes.OpenId,
                        IdentityModel.OidcConstants.StandardScopes.Profile,
                        IdentityModel.OidcConstants.StandardScopes.Email
                    }
                },
            };
    }
}