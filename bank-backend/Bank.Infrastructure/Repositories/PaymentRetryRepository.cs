using Bank.Domain.Common;
using Bank.Domain.Entities;
using Bank.Domain.Interfaces;
using Bank.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Bank.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for PaymentRetry entity
/// </summary>
public class PaymentRetryRepository : Repository<PaymentRetry>, IPaymentRetryRepository
{
    public PaymentRetryRepository(BankDbContext context) : base(context)
    {
    }

    public async Task<List<PaymentRetry>> GetPaymentRetriesAsync(Guid paymentId)
    {
        return await _dbSet
            .Include(pr => pr.Payment)
            .Where(pr => pr.PaymentId == paymentId && !pr.IsDeleted)
            .OrderBy(pr => pr.AttemptNumber)
            .ToListAsync();
    }

    public async Task<List<PaymentRetry>> GetPaymentsDueForRetryAsync()
    {
        var now = DateTime.UtcNow;
        
        return await _dbSet
            .Include(pr => pr.Payment)
            .Where(pr => pr.NextRetryDate <= now && 
                        !pr.IsMaxRetriesReached &&
                        !pr.IsDeleted)
            .OrderBy(pr => pr.NextRetryDate)
            .ToListAsync();
    }

    public async Task<PaymentRetry?> GetLatestRetryAttemptAsync(Guid paymentId)
    {
        return await _dbSet
            .Include(pr => pr.Payment)
            .Where(pr => pr.PaymentId == paymentId && !pr.IsDeleted)
            .OrderByDescending(pr => pr.AttemptNumber)
            .FirstOrDefaultAsync();
    }

    public async Task<Dictionary<string, int>> GetRetryStatisticsAsync(DateTime fromDate, DateTime toDate)
    {
        var retries = await _dbSet
            .Where(pr => pr.CreatedAt >= fromDate && 
                        pr.CreatedAt <= toDate &&
                        !pr.IsDeleted)
            .ToListAsync();

        return new Dictionary<string, int>
        {
            ["TotalRetries"] = retries.Count,
            ["SuccessfulRetries"] = retries.Count(r => r.Status == Domain.Enums.BillPaymentStatus.Processed),
            ["FailedRetries"] = retries.Count(r => r.Status == Domain.Enums.BillPaymentStatus.Failed),
            ["MaxRetriesReached"] = retries.Count(r => r.IsMaxRetriesReached),
            ["PendingRetries"] = retries.Count(r => r.Status == Domain.Enums.BillPaymentStatus.Pending)
        };
    }

    public async Task<List<PaymentRetry>> GetMaxRetriesReachedAsync()
    {
        return await _dbSet
            .Include(pr => pr.Payment)
            .Where(pr => pr.IsMaxRetriesReached && !pr.IsDeleted)
            .OrderByDescending(pr => pr.UpdatedAt)
            .ToListAsync();
    }
}