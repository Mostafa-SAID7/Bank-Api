using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Bank.Api;
using Bank.Application.Interfaces;
using Bank.Infrastructure.Data;
using Xunit;

namespace Bank.Tests;

/// <summary>
/// Integration tests for security foundation components using in-memory database
/// </summary>
public class SecurityFoundationIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly IServiceScope _scope;

    public SecurityFoundationIntegrationTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _scope = _factory.Services.CreateScope();
    }

    [Fact]
    public void SecurityServices_ShouldBeRegisteredCorrectly()
    {
        // Arrange & Act
        var twoFactorService = _scope.ServiceProvider.GetService<ITwoFactorAuthService>();
        var auditService = _scope.ServiceProvider.GetService<IAuditLogService>();
        var rateLimitService = _scope.ServiceProvider.GetService<IRateLimitingService>();
        var fraudService = _scope.ServiceProvider.GetService<IFraudDetectionService>();

        // Assert
        Assert.NotNull(twoFactorService);
        Assert.NotNull(auditService);
        Assert.NotNull(rateLimitService);
        Assert.NotNull(fraudService);
    }

    [Fact]
    public async Task Application_ShouldStartWithInMemoryDatabase()
    {
        // Arrange & Act
        var client = _factory.CreateClient();
        
        // Try to access a simple endpoint that doesn't require authentication
        var response = await client.GetAsync("/");

        // Assert
        // We expect either success or a redirect/not found, but not a server error
        Assert.True(response.IsSuccessStatusCode || 
                   response.StatusCode == System.Net.HttpStatusCode.NotFound ||
                   response.StatusCode == System.Net.HttpStatusCode.Redirect);
    }

    [Fact]
    public void DatabaseContext_ShouldBeConfiguredCorrectly()
    {
        // Arrange & Act
        var dbContext = _scope.ServiceProvider.GetService<BankDbContext>();

        // Assert
        Assert.NotNull(dbContext);
        Assert.True(dbContext.Database.IsInMemory());
    }

    public void Dispose()
    {
        _scope?.Dispose();
    }
}

/// <summary>
/// Custom WebApplicationFactory that uses in-memory database to avoid seeding issues
/// </summary>
public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Remove the existing DbContext registration
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<BankDbContext>));
            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            // Add in-memory database
            services.AddDbContext<BankDbContext>(options =>
            {
                options.UseInMemoryDatabase("TestDb");
            });

            // Build the service provider
            var serviceProvider = services.BuildServiceProvider();

            // Create a scope to obtain a reference to the database context
            using var scope = serviceProvider.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<BankDbContext>();

            // Ensure the database is created
            db.Database.EnsureCreated();
        });

        builder.UseEnvironment("Testing");
    }
}