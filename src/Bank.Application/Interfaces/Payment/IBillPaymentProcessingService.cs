using Bank.Application.DTOs.Payment.Biller;

namespace Bank.Application.Interfaces;

/// <summary>
/// Service interface for bill payment processing operations
/// Handles payment scheduling, processing, cancellation, and updates
/// </summary>
public interface IBillPaymentProcessingService
{
    /// <summary>
    /// Schedule a one-time bill payment
    /// </summary>
    Task<ScheduleBillPaymentResponse> ScheduleBillPaymentAsync(Guid customerId, ScheduleBillPaymentRequest request);

    /// <summary>
    /// Process scheduled bill payments that are due
    /// </summary>
    Task<List<ProcessBillPaymentResponse>> ProcessBillPaymentAsync(DateTime? processingDate = null);

    /// <summary>
    /// Cancel a scheduled bill payment
    /// </summary>
    Task<bool> CancelScheduledPaymentAsync(Guid customerId, Guid paymentId);

    /// <summary>
    /// Update a scheduled bill payment (only if pending)
    /// </summary>
    Task<bool> UpdateScheduledPaymentAsync(Guid customerId, Guid paymentId, UpdateBillPaymentRequest request);

    /// <summary>
    /// Validate a bill payment request
    /// </summary>
    Task<(bool IsValid, string ErrorMessage)> ValidateBillPaymentAsync(Guid customerId, ScheduleBillPaymentRequest request);
}
