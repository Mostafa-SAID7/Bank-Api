using Bank.Domain.Common;
using Bank.Domain.Enums;

namespace Bank.Domain.Entities;

/// <summary>
/// Represents a bill presented by a participating biller
/// </summary>
public class BillPresentment : BaseEntity
{
    public Guid CustomerId { get; set; }
    public Guid BillerId { get; set; }
    public string AccountNumber { get; set; } = string.Empty;
    public decimal AmountDue { get; set; }
    public decimal MinimumPayment { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime StatementDate { get; set; }
    public string Currency { get; set; } = "USD";
    public BillPresentmentStatus Status { get; set; } = BillPresentmentStatus.Pending;
    public string BillNumber { get; set; } = string.Empty;
    public string ExternalBillId { get; set; } = string.Empty;
    public DateTime? PaidDate { get; set; }
    public Guid? PaymentId { get; set; }
    public string LineItemsJson { get; set; } = string.Empty; // Serialized JSON of line items

    // Navigation properties
    public virtual User Customer { get; set; } = null!;
    public virtual Biller Biller { get; set; } = null!;
    public virtual BillPayment? Payment { get; set; }

    /// <summary>
    /// Mark the bill as paid
    /// </summary>
    public void MarkAsPaid(Guid paymentId)
    {
        Status = BillPresentmentStatus.Paid;
        PaymentId = paymentId;
        PaidDate = DateTime.UtcNow;
    }

    /// <summary>
    /// Mark the bill as overdue
    /// </summary>
    public void MarkAsOverdue()
    {
        if (Status == BillPresentmentStatus.Presented && DueDate < DateTime.UtcNow.Date)
        {
            Status = BillPresentmentStatus.Overdue;
        }
    }

    /// <summary>
    /// Cancel the bill presentment
    /// </summary>
    public void Cancel()
    {
        if (Status != BillPresentmentStatus.Paid)
        {
            Status = BillPresentmentStatus.Cancelled;
        }
        else
        {
            throw new InvalidOperationException("Cannot cancel a paid bill");
        }
    }

    /// <summary>
    /// Check if the bill is overdue
    /// </summary>
    public bool IsOverdue()
    {
        return Status == BillPresentmentStatus.Presented && DueDate < DateTime.UtcNow.Date;
    }

    /// <summary>
    /// Check if the bill can be paid
    /// </summary>
    public bool CanBePaid()
    {
        return Status is BillPresentmentStatus.Presented or BillPresentmentStatus.Overdue;
    }

    /// <summary>
    /// Get days until due date
    /// </summary>
    public int DaysUntilDue()
    {
        return (DueDate.Date - DateTime.UtcNow.Date).Days;
    }
}