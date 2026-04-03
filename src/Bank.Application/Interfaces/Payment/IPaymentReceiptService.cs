using Bank.Application.DTOs;
using Bank.Application.DTOs.Payment.Receipt;
using Bank.Domain.Common;

namespace Bank.Application.Interfaces;

/// <summary>
/// Service interface for payment receipt generation and management
/// </summary>
public interface IPaymentReceiptService
{
    /// <summary>
    /// Generate a payment receipt for a bill payment
    /// </summary>
    Task<PaymentReceiptDto> GeneratePaymentReceiptAsync(Guid paymentId);

    /// <summary>
    /// Get payment receipt by receipt number
    /// </summary>
    Task<PaymentReceiptDto?> GetReceiptByNumberAsync(string receiptNumber);

    /// <summary>
    /// Get payment receipt by payment ID
    /// </summary>
    Task<PaymentReceiptDto?> GetReceiptByPaymentIdAsync(Guid paymentId);

    /// <summary>
    /// Get customer payment receipts with pagination
    /// </summary>
    Task<Domain.Common.PagedResult<PaymentReceiptDto>> GetCustomerReceiptsAsync(
        Guid customerId, 
        int pageNumber = 1, 
        int pageSize = 20,
        DateTime? fromDate = null,
        DateTime? toDate = null);

    /// <summary>
    /// Update receipt status when payment status changes
    /// </summary>
    Task<bool> UpdateReceiptStatusAsync(Guid paymentId, Domain.Enums.BillPaymentStatus status);

    /// <summary>
    /// Generate receipt PDF document
    /// </summary>
    Task<byte[]> GenerateReceiptPdfAsync(string receiptNumber);

    /// <summary>
    /// Resend receipt to customer
    /// </summary>
    Task<bool> ResendReceiptAsync(string receiptNumber, string deliveryMethod = "email");

    /// <summary>
    /// Validate receipt authenticity
    /// </summary>
    Task<bool> ValidateReceiptAsync(string receiptNumber, string confirmationNumber);
}