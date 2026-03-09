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
        
        // Use the connection string for design-time operations with enhanced resilience
        var connectionString = "Server=db43977.public.databaseasp.net;Database=db43977;User Id=db43977;Password=3Sp-m9?A7+Kt;Encrypt=True;TrustServerCertificate=True;MultipleActiveResultSets=True;Connection Timeout=60;Command Timeout=300;Pooling=true;Min Pool Size=5;Max Pool Size=100;ConnectRetryCount=3;ConnectRetryInterval=10";
        
        optionsBuilder.UseSqlServer(connectionString, sqlOptions => 
        {
            // Enable retry on failure for transient network issues
            sqlOptions.EnableRetryOnFailure(
                maxRetryCount: 5,
                maxRetryDelay: TimeSpan.FromSeconds(30),
                errorNumbersToAdd: null);
                
            // Set command timeout for long-running operations
            sqlOptions.CommandTimeout(60);
            
            // Enable connection resiliency
            sqlOptions.MigrationsAssembly("Bank.Infrastructure");
        });

        return new BankDbContext(optionsBuilder.Options);
    }
}