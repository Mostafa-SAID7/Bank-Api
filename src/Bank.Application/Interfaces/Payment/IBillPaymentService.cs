using Bank.Application.DTOs;
using Bank.Application.DTOs.Payment.Biller;
using Bank.Domain.Common;
using Bank.Domain.Enums;

namespace Bank.Application.Interfaces;

/// <summary>
/// Service interface for bill payment operations
/// </summary>
public interface IBillPaymentService
{
    /// <summary>
    /// Get all available active billers
    /// </summary>
    Task<List<BillerDto>> GetAvailableBillersAsync();

    /// <summary>
    /// Get billers by category
    /// </summary>
    Task<List<BillerDto>> GetBillersByCategoryAsync(BillerCategory category);

    /// <summary>
    /// Search billers by name or other criteria
    /// </summary>
    Task<List<BillerDto>> SearchBillersAsync(BillerSearchRequest request);

    /// <summary>
    /// Get biller details by ID
    /// </summary>
    Task<BillerDto?> GetBillerByIdAsync(Guid billerId);

    /// <summary>
    /// Schedule a one-time bill payment
    /// </summary>
    Task<ScheduleBillPaymentResponse> ScheduleBillPaymentAsync(Guid customerId, ScheduleBillPaymentRequest request);

    /// <summary>
    /// Process scheduled bill payments that are due
    /// </summary>
    Task<List<ProcessBillPaymentResponse>> ProcessBillPaymentAsync(DateTime? processingDate = null);

    /// <summary>
    /// Get bill payment history for a customer
    /// </summary>
    Task<Bank.Domain.Common.PagedResult<BillPaymentHistoryDto>> GetBillPaymentHistoryAsync(Guid customerId, BillPaymentHistoryRequest request);

    /// <summary>
    /// Get pending bill payments for a customer
    /// </summary>
    Task<List<BillPaymentDto>> GetPendingBillPaymentsAsync(Guid customerId);

    /// <summary>
    /// Cancel a scheduled bill payment
    /// </summary>
    Task<bool> CancelScheduledPaymentAsync(Guid customerId, Guid paymentId);

    /// <summary>
    /// Get bill payment details by ID
    /// </summary>
    Task<BillPaymentDto?> GetBillPaymentByIdAsync(Guid customerId, Guid paymentId);

    /// <summary>
    /// Update a scheduled bill payment (only if pending)
    /// </summary>
    Task<bool> UpdateScheduledPaymentAsync(Guid customerId, Guid paymentId, UpdateBillPaymentRequest request);

    /// <summary>
    /// Validate bill payment request
    /// </summary>
    Task<(bool IsValid, string ErrorMessage)> ValidateBillPaymentAsync(Guid customerId, ScheduleBillPaymentRequest request);
}