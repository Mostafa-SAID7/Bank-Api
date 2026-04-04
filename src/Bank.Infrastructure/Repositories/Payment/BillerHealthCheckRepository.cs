using Bank.Domain.Common;
using Bank.Domain.Entities;
using Bank.Domain.Interfaces;
using Bank.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Bank.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for BillerHealthCheck entity
/// </summary>
public class BillerHealthCheckRepository : Repository<BillerHealthCheck>, IBillerHealthCheckRepository
{
    public BillerHealthCheckRepository(BankDbContext context) : base(context)
    {
    }

    public async Task<BillerHealthCheck?> GetLatestHealthCheckAsync(Guid billerId)
    {
        return await _dbSet
            .Include(bhc => bhc.Biller)
            .Where(bhc => bhc.BillerId == billerId && !bhc.IsDeleted)
            .OrderByDescending(bhc => bhc.CheckDate)
            .FirstOrDefaultAsync();
    }

    public async Task<List<BillerHealthCheck>> GetHealthCheckHistoryAsync(Guid billerId, DateTime? fromDate = null, DateTime? toDate = null)
    {
        var query = _dbSet
            .Include(bhc => bhc.Biller)
            .Where(bhc => bhc.BillerId == billerId && !bhc.IsDeleted);

        if (fromDate.HasValue)
        {
            query = query.Where(bhc => bhc.CheckDate >= fromDate.Value);
        }

        if (toDate.HasValue)
        {
            query = query.Where(bhc => bhc.CheckDate <= toDate.Value);
        }

        return await query
            .OrderByDescending(bhc => bhc.CheckDate)
            .ToListAsync();
    }

    public async Task<List<BillerHealthCheck>> GetUnhealthyBillersAsync()
    {
        // Get the latest health check for each biller
        var latestChecks = await _dbSet
            .Include(bhc => bhc.Biller)
            .Where(bhc => !bhc.IsDeleted)
            .GroupBy(bhc => bhc.BillerId)
            .Select(g => g.OrderByDescending(bhc => bhc.CheckDate).First())
            .Where(bhc => !bhc.IsHealthy)
            .ToListAsync();

        return latestChecks;
    }

    public async Task<List<Guid>> GetBillersDueForHealthCheckAsync(TimeSpan checkInterval)
    {
        var cutoffTime = DateTime.UtcNow.Subtract(checkInterval);

        // Get all billers that either have no health checks or their latest check is older than the interval
        var billersWithRecentChecks = await _dbSet
            .Where(bhc => bhc.CheckDate >= cutoffTime && !bhc.IsDeleted)
            .Select(bhc => bhc.BillerId)
            .Distinct()
            .ToListAsync();

        // Get all active billers
        var allActiveBillers = await _context.Set<Biller>()
            .Where(b => b.IsActive && !b.IsDeleted)
            .Select(b => b.Id)
            .ToListAsync();

        // Return billers that don't have recent health checks
        return allActiveBillers.Except(billersWithRecentChecks).ToList();
    }

    public async Task<Dictionary<string, object>> GetHealthCheckStatisticsAsync(DateTime fromDate, DateTime toDate)
    {
        var checks = await _dbSet
            .Where(bhc => bhc.CheckDate >= fromDate && 
                         bhc.CheckDate <= toDate &&
                         !bhc.IsDeleted)
            .ToListAsync();

        var totalChecks = checks.Count;
        var healthyChecks = checks.Count(c => c.IsHealthy);
        var unhealthyChecks = totalChecks - healthyChecks;
        var averageResponseTime = checks.Any() ? checks.Average(c => c.ResponseTime.TotalMilliseconds) : 0;

        var billerStats = checks
            .GroupBy(c => c.BillerId)
            .ToDictionary(
                g => g.Key.ToString(),
                g => new
                {
                    TotalChecks = g.Count(),
                    HealthyChecks = g.Count(c => c.IsHealthy),
                    UptimePercentage = BillerHealthCheck.CalculateUptimePercentage(g.ToList()),
                    AverageResponseTime = g.Average(c => c.ResponseTime.TotalMilliseconds)
                }
            );

        return new Dictionary<string, object>
        {
            ["TotalChecks"] = totalChecks,
            ["HealthyChecks"] = healthyChecks,
            ["UnhealthyChecks"] = unhealthyChecks,
            ["OverallUptimePercentage"] = totalChecks > 0 ? (double)healthyChecks / totalChecks * 100 : 0,
            ["AverageResponseTimeMs"] = averageResponseTime,
            ["BillerStatistics"] = billerStats
        };
    }

    public async Task<List<BillerHealthCheck>> GetBillersWithConsecutiveFailuresAsync(int minFailures)
    {
        // Get the latest health check for each biller with consecutive failures >= minFailures
        var latestChecks = await _dbSet
            .Include(bhc => bhc.Biller)
            .Where(bhc => !bhc.IsDeleted)
            .GroupBy(bhc => bhc.BillerId)
            .Select(g => g.OrderByDescending(bhc => bhc.CheckDate).First())
            .Where(bhc => bhc.ConsecutiveFailures >= minFailures)
            .ToListAsync();

        return latestChecks;
    }
}
