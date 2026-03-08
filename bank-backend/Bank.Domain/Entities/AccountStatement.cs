using Bank.Domain.Common;
using Bank.Domain.Enums;

namespace Bank.Domain.Entities;

/// <summary>
/// Represents an account statement with transaction details and summaries
/// </summary>
public class AccountStatement : BaseEntity
{
    public Guid AccountId { get; set; }
    public Account Account { get; set; } = null!;
    
    public DateTime StatementDate { get; set; }
    public DateTime PeriodStartDate { get; set; }
    public DateTime PeriodEndDate { get; set; }
    
    // Statement identification
    public string StatementNumber { get; set; } = string.Empty;
    public int StatementSequence { get; set; }
    
    // Balance information
    public decimal OpeningBalance { get; set; }
    public decimal ClosingBalance { get; set; }
    public decimal AverageBalance { get; set; }
    public decimal MinimumBalance { get; set; }
    public decimal MaximumBalance { get; set; }
    
    // Transaction summaries
    public int TotalTransactions { get; set; }
    public int DebitTransactions { get; set; }
    public int CreditTransactions { get; set; }
    public decimal TotalDebits { get; set; }
    public decimal TotalCredits { get; set; }
    
    // Fees and charges
    public decimal TotalFees { get; set; }
    public decimal InterestEarned { get; set; }
    public decimal InterestCharged { get; set; }
    
    // Statement status and delivery
    public StatementStatus Status { get; set; } = StatementStatus.Generated;
    public StatementFormat Format { get; set; } = StatementFormat.PDF;
    public StatementDeliveryMethod DeliveryMethod { get; set; } = StatementDeliveryMethod.Email;
    
    // File information
    public string? FilePath { get; set; }
    public string? FileName { get; set; }
    public long? FileSizeBytes { get; set; }
    public string? FileHash { get; set; }
    
    // Delivery tracking
    public DateTime? DeliveredDate { get; set; }
    public string? DeliveryReference { get; set; }
    public bool IsDelivered { get; set; } = false;
    
    // Request information
    public Guid? RequestedByUserId { get; set; }
    public User? RequestedByUser { get; set; }
    public DateTime? RequestedDate { get; set; }
    public string? RequestReason { get; set; }
    
    // Navigation properties
    public ICollection<StatementTransaction> Transactions { get; set; } = new List<StatementTransaction>();
    
    /// <summary>
    /// Generate statement number based on account and date
    /// </summary>
    public void GenerateStatementNumber()
    {
        var yearMonth = StatementDate.ToString("yyyyMM");
        var accountSuffix = Account?.AccountNumber?.Substring(Math.Max(0, Account.AccountNumber.Length - 4)) ?? "0000";
        StatementNumber = $"STMT-{yearMonth}-{accountSuffix}-{StatementSequence:D3}";
    }
    
    /// <summary>
    /// Mark statement as delivered
    /// </summary>
    public void MarkAsDelivered(string deliveryReference)
    {
        IsDelivered = true;
        DeliveredDate = DateTime.UtcNow;
        DeliveryReference = deliveryReference;
        Status = StatementStatus.Delivered;
    }
    
    /// <summary>
    /// Calculate statement statistics
    /// </summary>
    public void CalculateStatistics()
    {
        if (Transactions?.Any() == true)
        {
            TotalTransactions = Transactions.Count;
            DebitTransactions = Transactions.Count(t => t.Amount < 0);
            CreditTransactions = Transactions.Count(t => t.Amount > 0);
            TotalDebits = Math.Abs(Transactions.Where(t => t.Amount < 0).Sum(t => t.Amount));
            TotalCredits = Transactions.Where(t => t.Amount > 0).Sum(t => t.Amount);
        }
    }
}

/// <summary>
/// Represents a transaction included in a statement
/// </summary>
public class StatementTransaction : BaseEntity
{
    public Guid StatementId { get; set; }
    public AccountStatement Statement { get; set; } = null!;
    
    public Guid TransactionId { get; set; }
    public Transaction Transaction { get; set; } = null!;
    
    // Transaction details for statement
    public DateTime TransactionDate { get; set; }
    public string Description { get; set; } = string.Empty;
    public string? Reference { get; set; }
    public decimal Amount { get; set; }
    public decimal RunningBalance { get; set; }
    public TransactionType Type { get; set; }
    public TransactionStatus Status { get; set; }
    
    // Additional statement-specific information
    public string? Category { get; set; }
    public string? Memo { get; set; }
    public bool IsReconciled { get; set; } = false;
}