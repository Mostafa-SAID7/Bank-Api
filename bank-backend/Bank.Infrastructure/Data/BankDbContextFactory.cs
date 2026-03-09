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
        
        // Use the connection string for design-time operations
        var connectionString = "Server=db43977.public.databaseasp.net;Database=db43977;User Id=db43977;Password=3Sp-m9?A7+Kt;Encrypt=True;TrustServerCertificate=True;MultipleActiveResultSets=True";
        
        optionsBuilder.UseSqlServer(connectionString, sqlOptions => 
        {
            sqlOptions.EnableRetryOnFailure(
                maxRetryCount: 5,
                maxRetryDelay: TimeSpan.FromSeconds(30),
                errorNumbersToAdd: null);
        });

        return new BankDbContext(optionsBuilder.Options);
    }
}