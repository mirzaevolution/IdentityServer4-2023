
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Basics.CC.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.Authority = builder.Configuration["IDP:Authority"];
                    options.Audience = builder.Configuration["IDP:Audience"];
                });

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("ApiScope", config =>
                {
                    config.RequireAuthenticatedUser();
                    config.RequireClaim("scope", builder.Configuration["IDP:Scope"]);
                });
            });

            var app = builder.Build();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers().RequireAuthorization("ApiScope");

            app.Run();
        }
    }
}