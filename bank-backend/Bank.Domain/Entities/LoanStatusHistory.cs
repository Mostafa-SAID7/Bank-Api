using Bank.Domain.Common;
using Bank.Domain.Enums;

namespace Bank.Domain.Entities;

/// <summary>
/// Tracks the status history of a loan
/// </summary>
public class LoanStatusHistory : BaseEntity
{
    public Guid LoanId { get; set; }
    public Loan Loan { get; set; } = null!;
    
    public LoanStatus FromStatus { get; set; }
    public LoanStatus ToStatus { get; set; }
    public DateTime StatusChangeDate { get; set; } = DateTime.UtcNow;
    
    public Guid? ChangedBy { get; set; }
    public User? ChangedByUser { get; set; }
    
    public string? Reason { get; set; }
    public string? Notes { get; set; }
    
    // Additional context
    public string? SystemReference { get; set; }
    public bool IsSystemGenerated { get; set; }
}