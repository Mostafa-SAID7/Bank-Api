using Bank.Application.DTOs;

namespace Bank.Application.Interfaces;

/// <summary>
/// Service interface for batch payment processing
/// </summary>
public interface IBatchPaymentService
{
    /// <summary>
    /// Process a batch of payments for efficiency
    /// </summary>
    Task<BatchPaymentResponse> ProcessPaymentBatchAsync(List<Guid> paymentIds);

    /// <summary>
    /// Create and process batch from scheduled payments
    /// </summary>
    Task<BatchPaymentResponse> ProcessScheduledPaymentBatchAsync(DateTime processingDate, int batchSize = 100);

    /// <summary>
    /// Get batch processing status
    /// </summary>
    Task<BatchPaymentResponse?> GetBatchStatusAsync(string batchId);

    /// <summary>
    /// Get batch processing statistics
    /// </summary>
    Task<Dictionary<string, object>> GetBatchStatisticsAsync(DateTime fromDate, DateTime toDate);

    /// <summary>
    /// Process high-priority payments first
    /// </summary>
    Task<BatchPaymentResponse> ProcessPriorityPaymentBatchAsync(List<Guid> paymentIds);

    /// <summary>
    /// Validate batch before processing
    /// </summary>
    Task<(bool IsValid, List<string> ValidationErrors)> ValidatePaymentBatchAsync(List<Guid> paymentIds);
}