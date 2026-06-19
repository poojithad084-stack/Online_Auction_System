using System.Data;
using System.Security.Claims;
using System.Text;
using Auction.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Auction
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddHostedService<AuctionStatusUpdater>();

            builder.Services.AddControllers();
            builder.Services.AddDbContext<Models.AuctionContext>();
          
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });

            var secretKey = builder.Configuration["JwtSettings:SecretKey"]
               ?? throw new InvalidOperationException("JWT SecretKey is not configured in appsettings.json");
            var issuer = builder.Configuration["JwtSettings:Issuer"]
                ?? throw new InvalidOperationException("JWT Issuer is not configured in appsettings.json");
            var audience = builder.Configuration["JwtSettings:Audience"]
                ?? throw new InvalidOperationException("JWT Audience is not configured in appsettings.json");
            var key = Encoding.UTF8.GetBytes(secretKey);

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = issuer,
                    ValidAudience = audience,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ClockSkew = TimeSpan.Zero
                };
            });

            var app = builder.Build();
            app.UseCors("AllowAll");
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
