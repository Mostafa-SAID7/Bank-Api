using Bank.Domain.Entities;
using Bank.Domain.Enums;
using Bank.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Bank.Infrastructure.Repositories;

/// <summary>
/// Repository for audit logs with immutable storage guarantees.
/// Provides read-only access to audit logs for compliance and security analysis.
/// </summary>
public class AuditLogRepository : IAuditLogRepository
{
    private readonly BankDbContext _context;

    public AuditLogRepository(BankDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    /// <summary>
    /// Adds a new audit log entry. Once added, audit logs cannot be modified.
    /// </summary>
    public async Task<AuditLog> AddAsync(AuditLog auditLog, CancellationToken cancellationToken = default)
    {
        if (auditLog == null)
            throw new ArgumentNullException(nameof(auditLog));

        _context.AuditLogs.Add(auditLog);
        await _context.SaveChangesAsync(cancellationToken);
        return auditLog;
    }

    /// <summary>
    /// Gets audit logs by user ID with pagination and date filtering
    /// </summary>
    public async Task<IEnumerable<AuditLog>> GetByUserIdAsync(
        Guid userId,
        DateTime? fromDate = null,
        DateTime? toDate = null,
        int pageNumber = 1,
        int pageSize = 50,
        CancellationToken cancellationToken = default)
    {
        var query = _context.AuditLogs
            .Where(al => al.UserId == userId);

        if (fromDate.HasValue)
            query = query.Where(al => al.CreatedAt >= fromDate.Value);

        if (toDate.HasValue)
            query = query.Where(al => al.CreatedAt <= toDate.Value);

        return await query
            .OrderByDescending(al => al.CreatedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Gets audit logs by entity type and ID with pagination and date filtering
    /// </summary>
    public async Task<IEnumerable<AuditLog>> GetByEntityAsync(
        string entityType,
        string entityId,
        DateTime? fromDate = null,
        DateTime? toDate = null,
        int pageNumber = 1,
        int pageSize = 50,
        CancellationToken cancellationToken = default)
    {
        var query = _context.AuditLogs
            .Where(al => al.EntityType == entityType && al.EntityId == entityId);

        if (fromDate.HasValue)
            query = query.Where(al => al.CreatedAt >= fromDate.Value);

        if (toDate.HasValue)
            query = query.Where(al => al.CreatedAt <= toDate.Value);

        return await query
            .OrderByDescending(al => al.CreatedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Gets audit logs by event type with pagination and date filtering
    /// </summary>
    public async Task<IEnumerable<AuditLog>> GetByEventTypeAsync(
        AuditEventType eventType,
        DateTime? fromDate = null,
        DateTime? toDate = null,
        int pageNumber = 1,
        int pageSize = 50,
        CancellationToken cancellationToken = default)
    {
        var query = _context.AuditLogs
            .Where(al => al.EventType == eventType);

        if (fromDate.HasValue)
            query = query.Where(al => al.CreatedAt >= fromDate.Value);

        if (toDate.HasValue)
            query = query.Where(al => al.CreatedAt <= toDate.Value);

        return await query
            .OrderByDescending(al => al.CreatedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Gets audit logs by action with pagination and date filtering
    /// </summary>
    public async Task<IEnumerable<AuditLog>> GetByActionAsync(
        string action,
        DateTime? fromDate = null,
        DateTime? toDate = null,
        int pageNumber = 1,
        int pageSize = 50,
        CancellationToken cancellationToken = default)
    {
        var query = _context.AuditLogs
            .Where(al => al.Action == action);

        if (fromDate.HasValue)
            query = query.Where(al => al.CreatedAt >= fromDate.Value);

        if (toDate.HasValue)
            query = query.Where(al => al.CreatedAt <= toDate.Value);

        return await query
            .OrderByDescending(al => al.CreatedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Gets audit logs by IP address for security analysis
    /// </summary>
    public async Task<IEnumerable<AuditLog>> GetByIpAddressAsync(
        string ipAddress,
        DateTime? fromDate = null,
        DateTime? toDate = null,
        int pageNumber = 1,
        int pageSize = 50,
        CancellationToken cancellationToken = default)
    {
        var query = _context.AuditLogs
            .Where(al => al.IpAddress == ipAddress);

        if (fromDate.HasValue)
            query = query.Where(al => al.CreatedAt >= fromDate.Value);

        if (toDate.HasValue)
            query = query.Where(al => al.CreatedAt <= toDate.Value);

        return await query
            .OrderByDescending(al => al.CreatedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Gets audit log statistics for reporting and analytics
    /// </summary>
    public async Task<Dictionary<string, int>> GetStatisticsAsync(
        DateTime fromDate,
        DateTime toDate,
        CancellationToken cancellationToken = default)
    {
        var statistics = new Dictionary<string, int>();

        var query = _context.AuditLogs
            .Where(al => al.CreatedAt >= fromDate && al.CreatedAt <= toDate);

        // Total count
        statistics["Total"] = await query.CountAsync(cancellationToken);

        // By event type
        var eventTypeStats = await query
            .GroupBy(al => al.EventType)
            .Select(g => new { EventType = g.Key.ToString(), Count = g.Count() })
            .ToListAsync(cancellationToken);

        foreach (var stat in eventTypeStats)
        {
            statistics[stat.EventType] = stat.Count;
        }

        // By action (top 10)
        var actionStats = await query
            .GroupBy(al => al.Action)
            .Select(g => new { Action = g.Key, Count = g.Count() })
            .OrderByDescending(g => g.Count)
            .Take(10)
            .ToListAsync(cancellationToken);

        foreach (var stat in actionStats)
        {
            statistics[$"Action_{stat.Action}"] = stat.Count;
        }

        return statistics;
    }

    /// <summary>
    /// Gets a specific audit log by ID (read-only)
    /// </summary>
    public async Task<AuditLog?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.AuditLogs
            .AsNoTracking()
            .FirstOrDefaultAsync(al => al.Id == id, cancellationToken);
    }

    /// <summary>
    /// Counts total audit logs matching the criteria
    /// </summary>
    public async Task<int> CountAsync(
        Guid? userId = null,
        string? entityType = null,
        string? entityId = null,
        AuditEventType? eventType = null,
        string? action = null,
        string? ipAddress = null,
        DateTime? fromDate = null,
        DateTime? toDate = null,
        CancellationToken cancellationToken = default)
    {
        var query = _context.AuditLogs.AsQueryable();

        if (userId.HasValue)
            query = query.Where(al => al.UserId == userId.Value);

        if (!string.IsNullOrEmpty(entityType))
            query = query.Where(al => al.EntityType == entityType);

        if (!string.IsNullOrEmpty(entityId))
            query = query.Where(al => al.EntityId == entityId);

        if (eventType.HasValue)
            query = query.Where(al => al.EventType == eventType.Value);

        if (!string.IsNullOrEmpty(action))
            query = query.Where(al => al.Action == action);

        if (!string.IsNullOrEmpty(ipAddress))
            query = query.Where(al => al.IpAddress == ipAddress);

        if (fromDate.HasValue)
            query = query.Where(al => al.CreatedAt >= fromDate.Value);

        if (toDate.HasValue)
            query = query.Where(al => al.CreatedAt <= toDate.Value);

        return await query.CountAsync(cancellationToken);
    }
}