using Bank.Domain.Entities;
using Bank.Domain.Enums;

namespace Bank.Application.Interfaces;

/// <summary>
/// Service interface for audit logging operations.
/// Provides methods for creating and querying audit logs with immutable storage.
/// </summary>
public interface IAuditLogService
{
    /// <summary>
    /// Creates a user action audit log entry
    /// </summary>
    Task<AuditLog> LogUserActionAsync(
        Guid userId,
        string action,
        string entityType,
        string entityId,
        string? oldValues = null,
        string? newValues = null,
        string? ipAddress = null,
        string? userAgent = null,
        string? sessionId = null,
        string? requestId = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a system event audit log entry
    /// </summary>
    Task<AuditLog> LogSystemEventAsync(
        string action,
        string entityType,
        string entityId,
        string? additionalData = null,
        string? requestId = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a security event audit log entry
    /// </summary>
    Task<AuditLog> LogSecurityEventAsync(
        Guid? userId,
        string action,
        string entityType,
        string entityId,
        string? ipAddress = null,
        string? userAgent = null,
        string? additionalData = null,
        string? sessionId = null,
        string? requestId = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets audit logs for a specific user
    /// </summary>
    Task<IEnumerable<AuditLog>> GetUserAuditLogsAsync(
        Guid userId,
        DateTime? fromDate = null,
        DateTime? toDate = null,
        int pageNumber = 1,
        int pageSize = 50,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets audit logs for a specific entity
    /// </summary>
    Task<IEnumerable<AuditLog>> GetEntityAuditLogsAsync(
        string entityType,
        string entityId,
        DateTime? fromDate = null,
        DateTime? toDate = null,
        int pageNumber = 1,
        int pageSize = 50,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets audit logs by event type
    /// </summary>
    Task<IEnumerable<AuditLog>> GetAuditLogsByEventTypeAsync(
        AuditEventType eventType,
        DateTime? fromDate = null,
        DateTime? toDate = null,
        int pageNumber = 1,
        int pageSize = 50,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets audit logs by action
    /// </summary>
    Task<IEnumerable<AuditLog>> GetAuditLogsByActionAsync(
        string action,
        DateTime? fromDate = null,
        DateTime? toDate = null,
        int pageNumber = 1,
        int pageSize = 50,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets audit logs by IP address for security analysis
    /// </summary>
    Task<IEnumerable<AuditLog>> GetAuditLogsByIpAddressAsync(
        string ipAddress,
        DateTime? fromDate = null,
        DateTime? toDate = null,
        int pageNumber = 1,
        int pageSize = 50,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets audit log statistics for reporting
    /// </summary>
    Task<Dictionary<string, int>> GetAuditLogStatisticsAsync(
        DateTime fromDate,
        DateTime toDate,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Simple audit log method for backward compatibility
    /// </summary>
    Task LogAsync(string action, string description, Guid? userId = null);
}