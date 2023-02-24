using Basics.IntrApi.Client.AutoRefresh.Services;
using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace Basics.IntrApi.Client.AutoRefresh
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var services = builder.Services;
            var config = builder.Configuration;
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            services.AddControllersWithViews();
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
            {
                options.LoginPath = new PathString("/account/login");
                options.LogoutPath = new PathString("/account/logout");
            })
            .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
            {
                options.Authority = config["IDP:Authority"];
                options.ClientId = config["IDP:ClientId"];
                options.ClientSecret = config["IDP:ClientSecret"];
                options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.SaveTokens = true;
                options.GetClaimsFromUserInfoEndpoint = true;
                options.ClaimActions.MapAll();
                options.ResponseType = OidcConstants.ResponseTypes.Code;
                var scopes = (config["IDP:Scope"] ?? "").Split(" ", StringSplitOptions.RemoveEmptyEntries);
                foreach (var scope in scopes)
                {
                    options.Scope.Add(scope);
                }
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = "name",
                    RoleClaimType = "role"
                };
            });
            services.AddAccessTokenManagement();
            services.AddHttpClient<ApiInvokerService>(options =>
            {
                options.BaseAddress = new Uri(config["API:BaseAddress"]);
            }).AddUserAccessTokenHandler();

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