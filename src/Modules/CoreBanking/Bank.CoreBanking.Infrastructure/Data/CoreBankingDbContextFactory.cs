using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Bank.CoreBanking.Infrastructure.Data;

/// <summary>
/// Factory for creating CoreBankingDbContext instances during migrations
/// Required by EF Core tools to generate migrations without DI container
/// </summary>
public sealed class CoreBankingDbContextFactory : IDesignTimeDbContextFactory<CoreBankingDbContext>
{
    public CoreBankingDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<CoreBankingDbContext>();
        
        // Use a default connection string for migrations
        // In production, this will be overridden by the host application
        var connectionString = "Server=localhost;Port=5432;Database=bank_core_banking;User Id=postgres;Password=postgres;";
        
        optionsBuilder.UseNpgsql(connectionString);

        return new CoreBankingDbContext(optionsBuilder.Options);
    }
}
