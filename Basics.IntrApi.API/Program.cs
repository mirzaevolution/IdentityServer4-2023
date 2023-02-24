
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.IdentityModel.Tokens.Jwt;

namespace Basics.IntrApi.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var config = builder.Configuration;
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = config["IDP:Authority"];
                    options.TokenValidationParameters.NameClaimType = "name";
                    options.TokenValidationParameters.ValidateAudience = false;
                });
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("Default", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim("scope", config["IDP:Scope"]);
                });
            });
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers().RequireAuthorization("Default");

            app.Run();
        }
    }
}