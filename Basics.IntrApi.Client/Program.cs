using Basics.IntrApi.Client.Services;
using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using System.IdentityModel.Tokens.Jwt;

namespace Basics.IntrApi.Client
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var config = builder.Configuration;
            // Add services to the container.

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            builder.Services.AddControllersWithViews();
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
                {
                    options.LoginPath = "/Account/Login";
                    options.LogoutPath = "/Account/Logout";
                    options.ReturnUrlParameter = "redirectUrl";
                })
                .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
                {
                    options.Authority = config["IDP:Authority"];
                    options.ClientId = config["IDP:ClientId"];
                    options.ClientSecret = config["IDP:ClientSecret"];
                    options.ResponseType = OidcConstants.ResponseTypes.Code;
                    options.SaveTokens = true;
                    options.GetClaimsFromUserInfoEndpoint = true;
                    options.TokenValidationParameters.NameClaimType = "name";
                    options.ClaimActions.MapAll();
                    var scopes = config["IDP:Scope"].Split(" ", StringSplitOptions.RemoveEmptyEntries);
                    foreach (var scope in scopes)
                    {
                        options.Scope.Add(scope.Trim());
                    }
                });
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddHttpClient<ApiInvokerService>();
            var app = builder.Build();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}