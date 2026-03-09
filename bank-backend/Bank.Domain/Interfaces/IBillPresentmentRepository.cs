using Bank.Domain.Common;
using Bank.Domain.Entities;
using Bank.Domain.Enums;

namespace Bank.Domain.Interfaces;

/// <summary>
/// Repository interface for BillPresentment entity operations
/// </summary>
public interface IBillPresentmentRepository : IRepository<BillPresentment>
{
    /// <summary>
    /// Get bill presentments for a customer
    /// </summary>
    Task<List<BillPresentment>> GetCustomerBillPresentmentsAsync(Guid customerId, BillPresentmentStatus? status = null);

    /// <summary>
    /// Get bill presentments by biller
    /// </summary>
    Task<List<BillPresentment>> GetBillPresentmentsByBillerAsync(Guid billerId, DateTime? fromDate = null, DateTime? toDate = null);

    /// <summary>
    /// Get overdue bill presentments
    /// </summary>
    Task<List<BillPresentment>> GetOverdueBillPresentmentsAsync();

    /// <summary>
    /// Get bill presentment with details
    /// </summary>
    Task<BillPresentment?> GetBillPresentmentWithDetailsAsync(Guid presentmentId);

    /// <summary>
    /// Get unpaid bill presentments for a customer and biller
    /// </summary>
    Task<List<BillPresentment>> GetUnpaidBillPresentmentsAsync(Guid customerId, Guid billerId);

    /// <summary>
    /// Get bill presentments due within specified days
    /// </summary>
    Task<List<BillPresentment>> GetBillPresentmentsDueWithinDaysAsync(int days);

    /// <summary>
    /// Check if bill presentment exists for external bill ID
    /// </summary>
    Task<bool> ExistsByExternalBillIdAsync(string externalBillId);

    /// <summary>
    /// Get bill presentment by external bill ID
    /// </summary>
    Task<BillPresentment?> GetByExternalBillIdAsync(string externalBillId);
}