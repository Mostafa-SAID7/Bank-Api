using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Bank.Infrastructure.Data;

/// <summary>
/// Design-time factory for BankDbContext to support EF Core tools
/// </summary>
public class BankDbContextFactory : IDesignTimeDbContextFactory<BankDbContext>
{
    public BankDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<BankDbContext>();
        
        // Use local database connection string for design-time operations
        var connectionString = "Server=(localdb)\\mssqllocaldb;Database=BankingSystemLocalDb;Trusted_Connection=true;MultipleActiveResultSets=true;TrustServerCertificate=true";
        
        optionsBuilder.UseSqlServer(connectionString, sqlOptions => 
        {
            // Enable retry on failure for transient network issues
            sqlOptions.EnableRetryOnFailure(
                maxRetryCount: 3,
                maxRetryDelay: TimeSpan.FromSeconds(10),
                errorNumbersToAdd: null);
                
            // Set command timeout for long-running operations
            sqlOptions.CommandTimeout(60);
            
            // Enable connection resiliency
            sqlOptions.MigrationsAssembly("Bank.Infrastructure");
        });

        return new BankDbContext(optionsBuilder.Options);
    }
}