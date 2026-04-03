using Microsoft.EntityFrameworkCore;
using Bank.Infrastructure.Data;

namespace Bank.Api.Extensions.Infrastructure;

/// <summary>
/// Extension methods for database service registration
/// </summary>
public static class DatabaseServiceExtensions
{
    /// <summary>
    /// Register database and Entity Framework services
    /// </summary>
    public static IServiceCollection AddDatabaseServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection") 
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        var allowOfflineMode = configuration.GetValue<bool>("DatabaseSettings:AllowOfflineMode", false);

        services.AddDbContext<BankDbContext>(options =>
        {
            options.UseSqlServer(connectionString, sqlOptions =>
            {
                // Enable retry on failure for transient network issues
                sqlOptions.EnableRetryOnFailure(
                    maxRetryCount: allowOfflineMode ? 2 : 5,
                    maxRetryDelay: TimeSpan.FromSeconds(allowOfflineMode ? 10 : 30),
                    errorNumbersToAdd: null);
                
                // Set command timeout for long-running operations
                sqlOptions.CommandTimeout(allowOfflineMode ? 30 : 60);
                
                // Enable connection resiliency
                sqlOptions.MigrationsAssembly("Bank.Infrastructure");
            });

            // In development, enable sensitive data logging
            if (allowOfflineMode)
            {
                options.EnableSensitiveDataLogging();
                options.EnableDetailedErrors();
            }
        });

        return services;
    }
}
