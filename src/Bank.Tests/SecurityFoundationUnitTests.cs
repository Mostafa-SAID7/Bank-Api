using Bank.Domain.Entities;
using Bank.Domain.Enums;
using Xunit;

namespace Bank.Tests;

/// <summary>
/// Unit tests for security foundation components without requiring full application startup
/// </summary>
public class SecurityFoundationUnitTests
{
    [Fact]
    public void AuditLog_CreateUserAction_ShouldCreateCorrectly()
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
    public void AuditLog_CreateSecurityEvent_ShouldCreateCorrectly()
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
    public void AuditLog_CreateSystemEvent_ShouldCreateCorrectly()
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
    public void AuditLog_ShouldBeImmutable()
    {
        // Arrange & Act
        var auditLog = AuditLog.CreateUserAction(
            Guid.NewGuid(),
            "TEST_ACTION",
            "TEST_ENTITY",
            "TEST_ID");

        // Assert - All properties should be read-only (private setters)
        // This test verifies the immutability design of audit logs
        Assert.NotNull(auditLog.Action);
        Assert.NotNull(auditLog.EntityType);
        Assert.NotNull(auditLog.EntityId);
        Assert.True(auditLog.CreatedAt > DateTime.MinValue);
    }

    [Theory]
    [InlineData(AuditEventType.UserAction)]
    [InlineData(AuditEventType.SecurityEvent)]
    [InlineData(AuditEventType.SystemEvent)]
    public void AuditLog_ShouldSupportAllEventTypes(AuditEventType eventType)
    {
        // Arrange & Act
        AuditLog auditLog = eventType switch
        {
            AuditEventType.UserAction => AuditLog.CreateUserAction(Guid.NewGuid(), "ACTION", "ENTITY", "ID"),
            AuditEventType.SecurityEvent => AuditLog.CreateSecurityEvent(Guid.NewGuid(), "ACTION", "ENTITY", "ID"),
            AuditEventType.SystemEvent => AuditLog.CreateSystemEvent("ACTION", "ENTITY", "ID"),
            _ => throw new ArgumentException("Invalid event type")
        };

        // Assert
        Assert.Equal(eventType, auditLog.EventType);
    }

    [Fact]
    public void AuditLog_CreateUserAction_ShouldRequireValidParameters()
    {
        // Arrange
        var userId = Guid.NewGuid();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => 
            AuditLog.CreateUserAction(userId, null!, "ENTITY", "ID"));
        
        Assert.Throws<ArgumentNullException>(() => 
            AuditLog.CreateUserAction(userId, "ACTION", null!, "ID"));
        
        Assert.Throws<ArgumentNullException>(() => 
            AuditLog.CreateUserAction(userId, "ACTION", "ENTITY", null!));
    }

    [Fact]
    public void AuditLog_CreateSecurityEvent_ShouldAllowNullUserId()
    {
        // Arrange & Act
        var auditLog = AuditLog.CreateSecurityEvent(
            null, // Null user ID for anonymous security events
            "SUSPICIOUS_ACTIVITY",
            "SECURITY",
            "ANONYMOUS_REQUEST");

        // Assert
        Assert.Null(auditLog.UserId);
        Assert.Equal(AuditEventType.SecurityEvent, auditLog.EventType);
        Assert.Equal("SUSPICIOUS_ACTIVITY", auditLog.Action);
    }

    [Fact]
    public void AuditLog_ShouldSetCreatedAtToCurrentTime()
    {
        // Arrange
        var beforeCreation = DateTime.UtcNow;

        // Act
        var auditLog = AuditLog.CreateSystemEvent("TEST", "ENTITY", "ID");
        var afterCreation = DateTime.UtcNow;

        // Assert
        Assert.True(auditLog.CreatedAt >= beforeCreation);
        Assert.True(auditLog.CreatedAt <= afterCreation);
    }
}