using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Bank.Domain.Entities;
using Bank.Infrastructure.Data;

namespace Bank.Api.Extensions.Data;

/// <summary>
/// Extension methods for data seeding and database management
/// </summary>
public static class DataSeedingExtensions
{
    /// <summary>
    /// Apply pending database migrations with enhanced error handling and retry logic
    /// </summary>
    public static async Task ApplyDatabaseMigrationsAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        
        try
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<BankDbContext>();
            
            logger.LogInformation("🔍 Checking database connectivity...");
            
            // Test database connectivity first with retry logic
            var maxRetries = 3;
            var retryDelay = TimeSpan.FromSeconds(5);
            
            for (int attempt = 1; attempt <= maxRetries; attempt++)
            {
                try
                {
                    logger.LogInformation("📡 Testing database connection (attempt {Attempt}/{MaxRetries})...", attempt, maxRetries);
                    
                    // Simple connectivity test
                    await dbContext.Database.CanConnectAsync();
                    logger.LogInformation("✅ Database connection successful!");
                    break;
                }
                catch (Exception connEx) when (attempt < maxRetries)
                {
                    logger.LogWarning("⚠️ Database connection attempt {Attempt} failed: {Message}. Retrying in {Delay} seconds...", 
                        attempt, connEx.Message, retryDelay.TotalSeconds);
                    await Task.Delay(retryDelay);
                    retryDelay = TimeSpan.FromSeconds(retryDelay.TotalSeconds * 2); // Exponential backoff
                }
                catch (Exception connEx) when (attempt == maxRetries)
                {
                    logger.LogError(connEx, "❌ Failed to connect to database after {MaxRetries} attempts", maxRetries);
                    logger.LogWarning("⚠️ Application will start without database connectivity. Please check:");
                    logger.LogWarning("   • Network connectivity to: db43977.public.databaseasp.net");
                    logger.LogWarning("   • Database server availability");
                    logger.LogWarning("   • Connection string credentials");
                    logger.LogWarning("   • Firewall settings");
                    return;
                }
            }
            
            logger.LogInformation("🔍 Checking for pending database migrations...");
            
            var pendingMigrations = await dbContext.Database.GetPendingMigrationsAsync();
            
            if (pendingMigrations.Any())
            {
                logger.LogInformation("📋 Found {Count} pending migrations:", pendingMigrations.Count());
                
                foreach (var migration in pendingMigrations)
                {
                    logger.LogInformation("   • {Migration}", migration);
                }
                
                logger.LogInformation("🚀 Applying database migrations...");
                await dbContext.Database.MigrateAsync();
                logger.LogInformation("✅ Database migrations applied successfully!");
            }
            else
            {
                logger.LogInformation("✅ No pending migrations - database is up to date!");
            }
        }
        catch (Microsoft.Data.SqlClient.SqlException sqlEx) when (sqlEx.Number == 64)
        {
            logger.LogError("❌ Network connectivity issue: {Message}", sqlEx.Message);
            logger.LogWarning("💡 Troubleshooting suggestions:");
            logger.LogWarning("   • Check if the database server 'db43977.public.databaseasp.net' is accessible");
            logger.LogWarning("   • Verify your internet connection");
            logger.LogWarning("   • Check if the database service is running");
            logger.LogWarning("   • Try running the improved-migration.sql script manually");
            logger.LogWarning("⚠️ Application will continue without applying migrations.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "❌ Unexpected error applying database migrations: {Message}", ex.Message);
            logger.LogWarning("💡 Consider running the migration script manually:");
            logger.LogWarning("   • Use bank-backend/Bank.Infrastructure/improved-migration.sql");
            logger.LogWarning("   • Connect to your database management tool");
            logger.LogWarning("   • Execute the script to create missing tables");
            logger.LogWarning("⚠️ Application will continue without applying migrations.");
        }
    }

    /// <summary>
    /// Seed initial data (roles, admin user, policies) with error handling
    /// </summary>
    public static async Task SeedInitialDataAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        
        try
        {
            logger.LogInformation("🌱 Starting data seeding...");
            
            await SeedRolesAsync(scope.ServiceProvider);
            await SeedAdminUserAsync(scope.ServiceProvider);
            await SeedPasswordPoliciesAsync(scope.ServiceProvider);
            
            logger.LogInformation("✅ Data seeding completed successfully!");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "❌ Error during data seeding: {Message}", ex.Message);
            logger.LogWarning("⚠️ Application will continue without seeding data. Database may not be accessible.");
        }
    }

    /// <summary>
    /// Seed system roles
    /// </summary>
    private static async Task SeedRolesAsync(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<Role>>();
        
        string[] roles = { "Admin", "User", "Manager", "Auditor" };
        
        foreach (var roleName in roles)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new Role 
                { 
                    Name = roleName, 
                    Description = $"{roleName} role with appropriate permissions" 
                });
            }
        }
    }

    /// <summary>
    /// Seed default admin user
    /// </summary>
    private static async Task SeedAdminUserAsync(IServiceProvider serviceProvider)
    {
        var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
        var dbContext = serviceProvider.GetRequiredService<BankDbContext>();

        var adminEmail = "admin@finbank.com";
        
        if (await userManager.FindByEmailAsync(adminEmail) == null)
        {
            var adminUser = new User
            {
                UserName = "admin",
                Email = adminEmail,
                EmailConfirmed = true,
                FirstName = "System",
                LastName = "Admin",
                PhoneNumber = "+1234567890",
                PhoneNumberConfirmed = true
            };

            var result = await userManager.CreateAsync(adminUser, "Admin123!");
            
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
                
                // Seed a default account for the admin
                if (!dbContext.Accounts.Any(a => a.UserId == adminUser.Id))
                {
                    dbContext.Accounts.Add(new Account
                    {
                        UserId = adminUser.Id,
                        AccountNumber = "DE-1000-AD",
                        AccountHolderName = "System Admin",
                        Balance = 50000.00m,
                        Type = Bank.Domain.Enums.AccountType.Checking,
                        Status = Bank.Domain.Enums.AccountStatus.Active
                    });
                    
                    await dbContext.SaveChangesAsync();
                }
            }
        }
    }

    /// <summary>
    /// Seed default password policies
    /// </summary>
    private static async Task SeedPasswordPoliciesAsync(IServiceProvider serviceProvider)
    {
        var dbContext = serviceProvider.GetRequiredService<BankDbContext>();
        
        if (!dbContext.PasswordPolicies.Any())
        {
            var policies = new[]
            {
                PasswordPolicy.CreateBasicPolicy(),
                PasswordPolicy.CreateStandardPolicy(),
                PasswordPolicy.CreateStrongPolicy(),
                PasswordPolicy.CreateEnterprisePolicy()
            };

            dbContext.PasswordPolicies.AddRange(policies);
            await dbContext.SaveChangesAsync();
        }
    }
}