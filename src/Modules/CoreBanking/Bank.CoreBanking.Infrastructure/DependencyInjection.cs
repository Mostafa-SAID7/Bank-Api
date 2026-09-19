using Bank.CoreBanking.Application;
using Bank.CoreBanking.Infrastructure.Data;
using Bank.CoreBanking.Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Bank.CoreBanking.Infrastructure;

/// <summary>
/// Registers Core Banking infrastructure services (DbContext, repositories, publishers)
/// into the dependency injection container
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddCoreBankingInfrastructure(
        this IServiceCollection services,
        string? connectionString = null)
    {
        // Register DbContext
        services.AddDbContext<CoreBankingDbContext>((serviceProvider, options) =>
        {
            // Use the provided connection string or fall back to reading from configuration
            if (!string.IsNullOrEmpty(connectionString))
            {
                options.UseNpgsql(connectionString);
            }
            else
            {
                // Connection string will be provided at application startup
                options.UseNpgsql();
            }
        });

        // Register repositories
        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddScoped<ITransactionRepository, TransactionRepository>();
        services.AddScoped<ILedgerRepository, LedgerRepository>();

        // Register event publisher
        services.AddScoped<ICorePostingEventPublisher, CorePostingEventPublisher>();

        return services;
    }
}
