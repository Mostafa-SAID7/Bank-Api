using Bank.Domain.Common;
using Bank.Domain.Enums;

namespace Bank.Domain.Entities;

/// <summary>
/// Represents a bill payment transaction
/// </summary>
public class BillPayment : BaseEntity
{
    public Guid CustomerId { get; set; }
    public Guid BillerId { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "USD";
    public DateTime ScheduledDate { get; set; }
    public DateTime? ProcessedDate { get; set; }
    public BillPaymentStatus Status { get; set; } = BillPaymentStatus.Pending;
    public string Reference { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Guid? RecurringPaymentId { get; set; }

    // Navigation properties
    public virtual User Customer { get; set; } = null!;
    public virtual Biller Biller { get; set; } = null!;
    public virtual RecurringPayment? RecurringPayment { get; set; }

    /// <summary>
    /// Mark the payment as processed
    /// </summary>
    public void MarkAsProcessed()
    {
        Status = BillPaymentStatus.Processed;
        ProcessedDate = DateTime.UtcNow;
    }

    /// <summary>
    /// Mark the payment as delivered
    /// </summary>
    public void MarkAsDelivered()
    {
        Status = BillPaymentStatus.Delivered;
    }

    /// <summary>
    /// Mark the payment as failed with reason
    /// </summary>
    public void MarkAsFailed()
    {
        Status = BillPaymentStatus.Failed;
        ProcessedDate = DateTime.UtcNow;
    }

    /// <summary>
    /// Cancel the payment (only if pending)
    /// </summary>
    public void Cancel()
    {
        if (Status == BillPaymentStatus.Pending)
        {
            Status = BillPaymentStatus.Cancelled;
        }
        else
        {
            throw new InvalidOperationException($"Cannot cancel payment with status {Status}");
        }
    }

    /// <summary>
    /// Check if the payment can be cancelled
    /// </summary>
    public bool CanBeCancelled()
    {
        return Status == BillPaymentStatus.Pending;
    }

    /// <summary>
    /// Check if the payment is in a final state
    /// </summary>
    public bool IsInFinalState()
    {
        return Status is BillPaymentStatus.Delivered 
                      or BillPaymentStatus.Failed 
                      or BillPaymentStatus.Cancelled 
                      or BillPaymentStatus.Returned;
    }
}