using Bank.Domain.Common;
using Bank.Domain.Entities;

namespace Bank.Domain.Interfaces;

/// <summary>
/// Repository interface for PaymentRetry entity operations
/// </summary>
public interface IPaymentRetryRepository : IRepository<PaymentRetry>
{
    /// <summary>
    /// Get retry attempts for a payment
    /// </summary>
    Task<List<PaymentRetry>> GetPaymentRetriesAsync(Guid paymentId);

    /// <summary>
    /// Get payments due for retry
    /// </summary>
    Task<List<PaymentRetry>> GetPaymentsDueForRetryAsync();

    /// <summary>
    /// Get latest retry attempt for a payment
    /// </summary>
    Task<PaymentRetry?> GetLatestRetryAttemptAsync(Guid paymentId);

    /// <summary>
    /// Get retry statistics
    /// </summary>
    Task<Dictionary<string, int>> GetRetryStatisticsAsync(DateTime fromDate, DateTime toDate);

    /// <summary>
    /// Get payments that have reached max retries
    /// </summary>
    Task<List<PaymentRetry>> GetMaxRetriesReachedAsync();
}