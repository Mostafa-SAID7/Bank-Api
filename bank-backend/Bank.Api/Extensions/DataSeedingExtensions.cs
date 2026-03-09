using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Bank.Domain.Entities;
using Bank.Infrastructure.Data;

namespace Bank.Api.Extensions;

/// <summary>
/// Extension methods for data seeding and database management
/// </summary>
public static class DataSeedingExtensions
{
    /// <summary>
    /// Apply pending database migrations
    /// </summary>
    public static async Task ApplyDatabaseMigrationsAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        
        try
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<BankDbContext>();
            
            logger.LogInformation("Checking for pending database migrations...");
            
            var pendingMigrations = await dbContext.Database.GetPendingMigrationsAsync();
            
            if (pendingMigrations.Any())
            {
                logger.LogInformation("Found {Count} pending migrations. Applying...", pendingMigrations.Count());
                
                foreach (var migration in pendingMigrations)
                {
                    logger.LogInformation("Pending migration: {Migration}", migration);
                }
                
                await dbContext.Database.MigrateAsync();
                logger.LogInformation("✅ Database migrations applied successfully!");
            }
            else
            {
                logger.LogInformation("✅ No pending migrations - database is up to date!");
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "❌ Error applying database migrations: {Message}", ex.Message);
            
            // Don't throw - let the application start even if migrations fail
            // This allows for manual migration troubleshooting
            logger.LogWarning("⚠️ Application will continue without applying migrations. Please check database connectivity and apply migrations manually if needed.");
        }
    }

    /// <summary>
    /// Seed initial data (roles, admin user, policies)
    /// </summary>
    public static async Task SeedInitialDataAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        
        await SeedRolesAsync(scope.ServiceProvider);
        await SeedAdminUserAsync(scope.ServiceProvider);
        await SeedPasswordPoliciesAsync(scope.ServiceProvider);
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