using Bank.Domain.Entities;
using Bank.Domain.Enums;

namespace Bank.Domain.Interfaces;

/// <summary>
/// Repository interface for audit logs with immutable storage guarantees.
/// Provides read-only access to audit logs for compliance and security analysis.
/// </summary>
public interface IAuditLogRepository
{
    /// <summary>
    /// Adds a new audit log entry. Once added, audit logs cannot be modified.
    /// </summary>
    Task<AuditLog> AddAsync(AuditLog auditLog, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets audit logs by user ID with pagination and date filtering
    /// </summary>
    Task<IEnumerable<AuditLog>> GetByUserIdAsync(
        Guid userId,
        DateTime? fromDate = null,
        DateTime? toDate = null,
        int pageNumber = 1,
        int pageSize = 50,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets audit logs by entity type and ID with pagination and date filtering
    /// </summary>
    Task<IEnumerable<AuditLog>> GetByEntityAsync(
        string entityType,
        string entityId,
        DateTime? fromDate = null,
        DateTime? toDate = null,
        int pageNumber = 1,
        int pageSize = 50,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets audit logs by event type with pagination and date filtering
    /// </summary>
    Task<IEnumerable<AuditLog>> GetByEventTypeAsync(
        AuditEventType eventType,
        DateTime? fromDate = null,
        DateTime? toDate = null,
        int pageNumber = 1,
        int pageSize = 50,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets audit logs by action with pagination and date filtering
    /// </summary>
    Task<IEnumerable<AuditLog>> GetByActionAsync(
        string action,
        DateTime? fromDate = null,
        DateTime? toDate = null,
        int pageNumber = 1,
        int pageSize = 50,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets audit logs by IP address for security analysis
    /// </summary>
    Task<IEnumerable<AuditLog>> GetByIpAddressAsync(
        string ipAddress,
        DateTime? fromDate = null,
        DateTime? toDate = null,
        int pageNumber = 1,
        int pageSize = 50,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets audit log statistics for reporting and analytics
    /// </summary>
    Task<Dictionary<string, int>> GetStatisticsAsync(
        DateTime fromDate,
        DateTime toDate,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a specific audit log by ID (read-only)
    /// </summary>
    Task<AuditLog?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Counts total audit logs matching the criteria
    /// </summary>
    Task<int> CountAsync(
        Guid? userId = null,
        string? entityType = null,
        string? entityId = null,
        AuditEventType? eventType = null,
        string? action = null,
        string? ipAddress = null,
        DateTime? fromDate = null,
        DateTime? toDate = null,
        CancellationToken cancellationToken = default);
}