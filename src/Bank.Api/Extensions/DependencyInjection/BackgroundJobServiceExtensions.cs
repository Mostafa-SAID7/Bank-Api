using Hangfire;

namespace Bank.Api.Extensions.DependencyInjection;

/// <summary>
/// Extension methods for background job service registration
/// </summary>
public static class BackgroundJobServiceExtensions
{
    /// <summary>
    /// Register background job services
    /// </summary>
    public static IServiceCollection AddBackgroundJobServices(this IServiceCollection services, IConfiguration configuration)
    {
        var allowOfflineMode = configuration.GetValue<bool>("DatabaseSettings:AllowOfflineMode", false);
        
        if (!allowOfflineMode)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection") 
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            // Hangfire for background jobs
            services.AddHangfire(config => config
                .UseSqlServerStorage(connectionString));
            services.AddHangfireServer();
        }
        else
        {
            // In offline mode, skip Hangfire and background services
            var logger = services.BuildServiceProvider().GetService<ILogger<Program>>();
            logger?.LogWarning("⚠️ Background job services disabled (offline mode)");
        }

        return services;
    }
}
