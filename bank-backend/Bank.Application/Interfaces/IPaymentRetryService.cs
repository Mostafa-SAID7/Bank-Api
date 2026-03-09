using Bank.Application.DTOs;

namespace Bank.Application.Interfaces;

/// <summary>
/// Service interface for payment retry logic with exponential backoff
/// </summary>
public interface IPaymentRetryService
{
    /// <summary>
    /// Schedule a payment for retry with exponential backoff
    /// </summary>
    Task<PaymentRetryResult> SchedulePaymentRetryAsync(PaymentRetryRequest request);

    /// <summary>
    /// Process payments that are due for retry
    /// </summary>
    Task<List<PaymentRetryResult>> ProcessRetryPaymentsAsync();

    /// <summary>
    /// Get retry attempts for a payment
    /// </summary>
    Task<List<PaymentRetryResult>> GetPaymentRetryHistoryAsync(Guid paymentId);

    /// <summary>
    /// Cancel retry attempts for a payment
    /// </summary>
    Task<bool> CancelPaymentRetriesAsync(Guid paymentId);

    /// <summary>
    /// Get retry statistics for reporting
    /// </summary>
    Task<Dictionary<string, int>> GetRetryStatisticsAsync(DateTime fromDate, DateTime toDate);

    /// <summary>
    /// Process payments that have reached maximum retry attempts
    /// </summary>
    Task<List<Guid>> ProcessMaxRetriesReachedAsync();

    /// <summary>
    /// Check if payment has reached maximum retry attempts
    /// </summary>
    Task<bool> HasReachedMaxRetriesAsync(Guid paymentId);
}