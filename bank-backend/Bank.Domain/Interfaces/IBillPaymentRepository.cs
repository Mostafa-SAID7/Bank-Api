using Bank.Domain.Common;
using Bank.Domain.Entities;
using Bank.Domain.Enums;

namespace Bank.Domain.Interfaces;

/// <summary>
/// Repository interface for BillPayment entity operations
/// </summary>
public interface IBillPaymentRepository : IRepository<BillPayment>
{
    /// <summary>
    /// Get bill payment history for a customer
    /// </summary>
    Task<PagedResult<BillPayment>> GetCustomerPaymentHistoryAsync(
        Guid customerId, 
        int pageNumber = 1, 
        int pageSize = 20,
        DateTime? fromDate = null,
        DateTime? toDate = null);

    /// <summary>
    /// Get scheduled payments that are due for processing
    /// </summary>
    Task<List<BillPayment>> GetScheduledPaymentsDueAsync(DateTime processingDate);

    /// <summary>
    /// Get pending payments for a customer
    /// </summary>
    Task<List<BillPayment>> GetCustomerPendingPaymentsAsync(Guid customerId);

    /// <summary>
    /// Get payments by status
    /// </summary>
    Task<List<BillPayment>> GetPaymentsByStatusAsync(BillPaymentStatus status);

    /// <summary>
    /// Get payment with related entities (customer, biller)
    /// </summary>
    Task<BillPayment?> GetPaymentWithDetailsAsync(Guid paymentId);

    /// <summary>
    /// Get payments for a specific biller
    /// </summary>
    Task<List<BillPayment>> GetPaymentsByBillerAsync(Guid billerId, DateTime? fromDate = null, DateTime? toDate = null);

    /// <summary>
    /// Get recurring payment executions
    /// </summary>
    Task<List<BillPayment>> GetRecurringPaymentExecutionsAsync(Guid recurringPaymentId);

    /// <summary>
    /// Check if customer has any pending payments to a specific biller
    /// </summary>
    Task<bool> HasPendingPaymentToBillerAsync(Guid customerId, Guid billerId);
}