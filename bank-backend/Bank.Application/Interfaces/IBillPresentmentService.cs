using Bank.Application.DTOs;
using Bank.Domain.Common;

namespace Bank.Application.Interfaces;

/// <summary>
/// Service interface for bill presentment operations
/// </summary>
public interface IBillPresentmentService
{
    /// <summary>
    /// Get bill presentments for a customer
    /// </summary>
    Task<List<BillPresentmentDto>> GetCustomerBillPresentmentsAsync(Guid customerId, BillPresentmentStatus? status = null);

    /// <summary>
    /// Get bill presentments by biller
    /// </summary>
    Task<List<BillPresentmentDto>> GetBillPresentmentsByBillerAsync(Guid billerId, DateTime? fromDate = null, DateTime? toDate = null);

    /// <summary>
    /// Get overdue bill presentments
    /// </summary>
    Task<List<BillPresentmentDto>> GetOverdueBillPresentmentsAsync();

    /// <summary>
    /// Get bill presentments due within specified days
    /// </summary>
    Task<List<BillPresentmentDto>> GetBillPresentmentsDueWithinDaysAsync(int days);

    /// <summary>
    /// Create a new bill presentment
    /// </summary>
    Task<BillPresentmentDto> CreateBillPresentmentAsync(CreateBillPresentmentRequest request);

    /// <summary>
    /// Update bill presentment status
    /// </summary>
    Task<bool> UpdateBillPresentmentStatusAsync(Guid presentmentId, BillPresentmentStatus status);

    /// <summary>
    /// Mark bill presentment as paid
    /// </summary>
    Task<bool> MarkBillPresentmentAsPaidAsync(Guid presentmentId, Guid paymentId);

    /// <summary>
    /// Cancel bill presentment
    /// </summary>
    Task<bool> CancelBillPresentmentAsync(Guid presentmentId);

    /// <summary>
    /// Get bill presentment details
    /// </summary>
    Task<BillPresentmentDto?> GetBillPresentmentByIdAsync(Guid presentmentId);

    /// <summary>
    /// Process overdue bill presentments
    /// </summary>
    Task<int> ProcessOverdueBillPresentmentsAsync();

    /// <summary>
    /// Synchronize bill presentments with external biller systems
    /// </summary>
    Task<List<BillPresentmentSyncResult>> SynchronizeBillPresentmentsAsync(Guid customerId, Guid billerId);
}

// DTOs for bill presentment service
public record CreateBillPresentmentRequest(
    Guid CustomerId,
    Guid BillerId,
    string AccountNumber,
    decimal AmountDue,
    decimal MinimumPayment,
    DateTime DueDate,
    DateTime StatementDate,
    string BillNumber,
    string ExternalBillId,
    List<BillLineItemDto> LineItems);

public class BillPresentmentSyncResult
{
    public string ExternalBillId { get; set; } = string.Empty;
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public BillPresentmentStatus Status { get; set; }
    public DateTime SyncDate { get; set; }
}