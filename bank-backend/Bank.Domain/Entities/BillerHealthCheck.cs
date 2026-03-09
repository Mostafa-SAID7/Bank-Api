using Bank.Domain.Common;

namespace Bank.Domain.Entities;

/// <summary>
/// Represents a biller connectivity health check result
/// </summary>
public class BillerHealthCheck : BaseEntity
{
    public Guid BillerId { get; set; }
    public bool IsHealthy { get; set; }
    public DateTime CheckDate { get; set; }
    public TimeSpan ResponseTime { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? ErrorMessage { get; set; }
    public string HealthMetricsJson { get; set; } = string.Empty; // Serialized health metrics
    public int ConsecutiveFailures { get; set; }
    public DateTime? LastSuccessfulCheck { get; set; }

    // Navigation properties
    public virtual Biller Biller { get; set; } = null!;

    /// <summary>
    /// Mark health check as successful
    /// </summary>
    public void MarkAsHealthy(TimeSpan responseTime)
    {
        IsHealthy = true;
        ResponseTime = responseTime;
        Status = "Healthy";
        ErrorMessage = null;
        ConsecutiveFailures = 0;
        LastSuccessfulCheck = DateTime.UtcNow;
        CheckDate = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Mark health check as failed
    /// </summary>
    public void MarkAsUnhealthy(string errorMessage, TimeSpan responseTime = default)
    {
        IsHealthy = false;
        ResponseTime = responseTime;
        Status = "Unhealthy";
        ErrorMessage = errorMessage;
        ConsecutiveFailures++;
        CheckDate = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Check if biller should be marked as down
    /// </summary>
    public bool ShouldMarkAsDown(int maxConsecutiveFailures = 3)
    {
        return ConsecutiveFailures >= maxConsecutiveFailures;
    }

    /// <summary>
    /// Get uptime percentage over a period
    /// </summary>
    public static double CalculateUptimePercentage(List<BillerHealthCheck> checks)
    {
        if (!checks.Any()) return 0.0;

        var healthyChecks = checks.Count(c => c.IsHealthy);
        return (double)healthyChecks / checks.Count * 100.0;
    }
}