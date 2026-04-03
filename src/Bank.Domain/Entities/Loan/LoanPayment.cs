using Bank.Domain.Common;
using Bank.Domain.Enums;

namespace Bank.Domain.Entities;

/// <summary>
/// Represents a loan payment transaction
/// </summary>
public class LoanPayment : BaseEntity
{
    public decimal Amount { get; set; }
    public Guid LoanId { get; set; }
    public Loan Loan { get; set; } = null!;
    
    public decimal PaymentAmount { get; set; }
    public decimal PrincipalAmount { get; set; }
    public decimal InterestAmount { get; set; }
    public decimal LateFeeAmount { get; set; }
    public decimal OutstandingBalanceAfterPayment { get; set; }
    
    public DateTime PaymentDate { get; set; } = DateTime.UtcNow;
    public DateTime DueDate { get; set; }
    public LoanPaymentStatus Status { get; set; } = LoanPaymentStatus.Scheduled;
    
    public string? TransactionReference { get; set; }
    public string? PaymentMethod { get; set; }
    public string? Notes { get; set; }
    
    // Payment processing details
    public DateTime? ProcessedDate { get; set; }
    public Guid? ProcessedBy { get; set; }
    public User? ProcessedByUser { get; set; }
    
    // Domain methods
    public bool IsOverdue()
    {
        return Status != LoanPaymentStatus.Paid && DateTime.UtcNow > DueDate;
    }
    
    public int GetDaysOverdue()
    {
        if (!IsOverdue()) return 0;
        return (DateTime.UtcNow - DueDate).Days;
    }
    
    public void MarkAsPaid(string? transactionReference = null)
    {
        Status = LoanPaymentStatus.Paid;
        ProcessedDate = DateTime.UtcNow;
        TransactionReference = transactionReference;
    }
    
    public void MarkAsPartial(decimal paidAmount)
    {
        Status = LoanPaymentStatus.Partial;
        PaymentAmount = paidAmount;
        ProcessedDate = DateTime.UtcNow;
    }
    
    public void MarkAsFailed(string? reason = null)
    {
        Status = LoanPaymentStatus.Failed;
        Notes = reason;
    }
}
