using Bank.Domain.Common;
using Bank.Domain.Entities;

namespace Bank.Domain.Interfaces;

/// <summary>
/// Repository interface for PaymentReceipt entity operations
/// </summary>
public interface IPaymentReceiptRepository : IRepository<PaymentReceipt>
{
    /// <summary>
    /// Get receipt by payment ID
    /// </summary>
    Task<PaymentReceipt?> GetByPaymentIdAsync(Guid paymentId);

    /// <summary>
    /// Get receipts for a customer
    /// </summary>
    Task<PagedResult<PaymentReceipt>> GetCustomerReceiptsAsync(
        Guid customerId, 
        int pageNumber = 1, 
        int pageSize = 20,
        DateTime? fromDate = null,
        DateTime? toDate = null);

    /// <summary>
    /// Get receipt by receipt number
    /// </summary>
    Task<PaymentReceipt?> GetByReceiptNumberAsync(string receiptNumber);

    /// <summary>
    /// Get receipts by confirmation number
    /// </summary>
    Task<List<PaymentReceipt>> GetByConfirmationNumberAsync(string confirmationNumber);

    /// <summary>
    /// Check if receipt number exists
    /// </summary>
    Task<bool> ReceiptNumberExistsAsync(string receiptNumber);
}