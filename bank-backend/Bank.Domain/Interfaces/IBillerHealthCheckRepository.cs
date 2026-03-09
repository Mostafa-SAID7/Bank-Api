using Bank.Domain.Common;
using Bank.Domain.Entities;

namespace Bank.Domain.Interfaces;

/// <summary>
/// Repository interface for BillerHealthCheck entity operations
/// </summary>
public interface IBillerHealthCheckRepository : IRepository<BillerHealthCheck>
{
    /// <summary>
    /// Get latest health check for a biller
    /// </summary>
    Task<BillerHealthCheck?> GetLatestHealthCheckAsync(Guid billerId);

    /// <summary>
    /// Get health check history for a biller
    /// </summary>
    Task<List<BillerHealthCheck>> GetHealthCheckHistoryAsync(Guid billerId, DateTime? fromDate = null, DateTime? toDate = null);

    /// <summary>
    /// Get unhealthy billers
    /// </summary>
    Task<List<BillerHealthCheck>> GetUnhealthyBillersAsync();

    /// <summary>
    /// Get billers due for health check
    /// </summary>
    Task<List<Guid>> GetBillersDueForHealthCheckAsync(TimeSpan checkInterval);

    /// <summary>
    /// Get health check statistics
    /// </summary>
    Task<Dictionary<string, object>> GetHealthCheckStatisticsAsync(DateTime fromDate, DateTime toDate);

    /// <summary>
    /// Get billers with consecutive failures
    /// </summary>
    Task<List<BillerHealthCheck>> GetBillersWithConsecutiveFailuresAsync(int minFailures);
}