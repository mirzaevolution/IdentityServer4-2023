using IdentityModel;
using IdentityServer4.Test;
using System.Collections.Generic;
using System.Security.Claims;

namespace IdentityServerHost.Quickstart.UI
{
    public class TestUsers
    {
        public static List<TestUser> Users
        {
            get
            {

                return new List<TestUser>
                {
                    new TestUser
                    {
                        SubjectId = "0000000101",
                        Username = "mirzaghulamrasyid",
                        Password = "@Future30",
                        Claims =
                        {
                            new Claim(JwtClaimTypes.Name, "Mirza Ghulam Rasyid"),
                            new Claim(JwtClaimTypes.Email, "ghulamcyber@hotmail.com"),
                            new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean)
                        }
                    }
                };
            }
        }
    }
}