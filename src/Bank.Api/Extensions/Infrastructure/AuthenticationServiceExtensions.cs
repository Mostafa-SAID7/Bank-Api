using Microsoft.AspNetCore.Identity;
using Bank.Domain.Entities;
using Bank.Infrastructure.Data;

namespace Bank.Api.Extensions.Infrastructure;

/// <summary>
/// Extension methods for authentication and authorization service registration
/// </summary>
public static class AuthenticationServiceExtensions
{
    /// <summary>
    /// Register authentication and authorization services
    /// </summary>
    public static IServiceCollection AddAuthenticationServices(this IServiceCollection services, IConfiguration configuration)
    {
        // ASP.NET Core Identity
        services.AddIdentity<User, Role>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequiredLength = 6;
            options.User.RequireUniqueEmail = true;
        })
        .AddEntityFrameworkStores<BankDbContext>()
        .AddDefaultTokenProviders();

        // JWT Authentication
        services.AddAuthentication(opts => {
            opts.DefaultAuthenticateScheme = "Bearer";
            opts.DefaultChallengeScheme = "Bearer";
        }).AddJwtBearer(options => {
            var jwtSettings = configuration.GetSection("Jwt");
            var key = System.Text.Encoding.ASCII.GetBytes(jwtSettings["Key"] ?? "SUPER_SECRET_KEY");
            options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = jwtSettings["Issuer"],
                ValidateAudience = true,
                ValidAudience = jwtSettings["Audience"]
            };
        });

        return services;
    }
}
