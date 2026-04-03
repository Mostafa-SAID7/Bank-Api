namespace Bank.Api.Extensions.Configuration;

/// <summary>
/// Extension methods for CORS policy registration
/// </summary>
public static class CorsServiceExtensions
{
    /// <summary>
    /// Register CORS policies
    /// </summary>
    public static IServiceCollection AddCorsServices(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAngular", policy =>
            {
                policy.WithOrigins("http://localhost:4200")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
            });
        });

        return services;
    }
}
