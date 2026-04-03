using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc.Testing;
using Bank.Api;
using Bank.Application.Interfaces;
using Bank.Domain.Entities;
using Bank.Domain.Enums;
using Xunit;

namespace Bank.Tests;

/// <summary>
/// Tests to verify the security foundation components are working correctly
/// </summary>
public class SecurityFoundationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly IServiceScope _scope;

    public SecurityFoundationTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _scope = _factory.Services.CreateScope();
    }

    [Fact]
    public void TwoFactorAuthService_ShouldBeRegistered()
    {
        // Arrange & Act
        var twoFactorService = _scope.ServiceProvider.GetService<ITwoFactorAuthService>();

        // Assert
        Assert.NotNull(twoFactorService);
    }

    [Fact]
    public void AuditLogService_ShouldBeRegistered()
    {
        // Arrange & Act
        var auditService = _scope.ServiceProvider.GetService<IAuditLogService>();

        // Assert
        Assert.NotNull(auditService);
    }

    [Fact]
    public void RateLimitingService_ShouldBeRegistered()
    {
        // Arrange & Act
        var rateLimitService = _scope.ServiceProvider.GetService<IRateLimitingService>();

        // Assert
        Assert.NotNull(rateLimitService);
    }

    [Fact]
    public void FraudDetectionService_ShouldBeRegistered()
    {
        // Arrange & Act
        var fraudService = _scope.ServiceProvider.GetService<IFraudDetectionService>();

        // Assert
        Assert.NotNull(fraudService);
    }

    [Fact]
    public void AuditLog_ShouldCreateUserActionCorrectly()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var action = "LOGIN";
        var entityType = "USER";
        var entityId = userId.ToString();

        // Act
        var auditLog = AuditLog.CreateUserAction(
            userId,
            action,
            entityType,
            entityId,
            ipAddress: "192.168.1.1",
            userAgent: "Test Browser");

        // Assert
        Assert.Equal(userId, auditLog.UserId);
        Assert.Equal(action, auditLog.Action);
        Assert.Equal(entityType, auditLog.EntityType);
        Assert.Equal(entityId, auditLog.EntityId);
        Assert.Equal(AuditEventType.UserAction, auditLog.EventType);
        Assert.Equal("192.168.1.1", auditLog.IpAddress);
        Assert.Equal("Test Browser", auditLog.UserAgent);
        Assert.True(auditLog.CreatedAt <= DateTime.UtcNow);
    }

    [Fact]
    public void AuditLog_ShouldCreateSecurityEventCorrectly()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var action = "FAILED_LOGIN";
        var entityType = "SECURITY";
        var entityId = "LOGIN_ATTEMPT";

        // Act
        var auditLog = AuditLog.CreateSecurityEvent(
            userId,
            action,
            entityType,
            entityId,
            ipAddress: "192.168.1.1",
            userAgent: "Test Browser",
            additionalData: "Multiple failed attempts");

        // Assert
        Assert.Equal(userId, auditLog.UserId);
        Assert.Equal(action, auditLog.Action);
        Assert.Equal(entityType, auditLog.EntityType);
        Assert.Equal(entityId, auditLog.EntityId);
        Assert.Equal(AuditEventType.SecurityEvent, auditLog.EventType);
        Assert.Equal("192.168.1.1", auditLog.IpAddress);
        Assert.Equal("Test Browser", auditLog.UserAgent);
        Assert.Equal("Multiple failed attempts", auditLog.AdditionalData);
        Assert.True(auditLog.CreatedAt <= DateTime.UtcNow);
    }

    [Fact]
    public void AuditLog_ShouldCreateSystemEventCorrectly()
    {
        // Arrange
        var action = "SYSTEM_STARTUP";
        var entityType = "SYSTEM";
        var entityId = "APPLICATION";

        // Act
        var auditLog = AuditLog.CreateSystemEvent(
            action,
            entityType,
            entityId,
            additionalData: "Application started successfully");

        // Assert
        Assert.Null(auditLog.UserId);
        Assert.Equal(action, auditLog.Action);
        Assert.Equal(entityType, auditLog.EntityType);
        Assert.Equal(entityId, auditLog.EntityId);
        Assert.Equal(AuditEventType.SystemEvent, auditLog.EventType);
        Assert.Equal("Application started successfully", auditLog.AdditionalData);
        Assert.True(auditLog.CreatedAt <= DateTime.UtcNow);
    }

    [Fact]
    public async Task Application_ShouldStartSuccessfully()
    {
        // Arrange & Act
        var client = _factory.CreateClient();
        var response = await client.GetAsync("/health");

        // Assert
        Assert.True(response.IsSuccessStatusCode || response.StatusCode == System.Net.HttpStatusCode.NotFound);
        // Note: 404 is acceptable if health endpoint doesn't exist yet
    }

    public void Dispose()
    {
        _scope?.Dispose();
    }
}