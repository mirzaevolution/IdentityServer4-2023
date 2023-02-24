// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Models;
using System.Collections.Generic;

namespace Basics.CC.Idp
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("scope:full_access")
            };

        public static IEnumerable<ApiResource> ApiResources => new ApiResource[]
        {
            new ApiResource("Basics.CC.DemoApp")
            {
                Scopes =
                {
                    "scope:full_access"
                }
            }
        };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                new Client
                {
                    ClientId = "12345",
                    ClientSecrets =
                    {
                        new Secret("P@$sw0Rd".Sha256())
                    },
                    AllowedScopes =
                    {
                        "scope:full_access"
                    },
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientName = "Basics.CC.DemoApp"
                }
            };
    }
}