using Bank.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Bank.Application.DTOs.Card.Transactions;

/// <summary>
/// Request for card statement generation
/// </summary>
public class CardStatementRequest
{
    [Required]
    public Guid CardId { get; set; }
    
    [Required]
    public DateTime FromDate { get; set; }
    
    [Required]
    public DateTime ToDate { get; set; }
    
    public StatementFormat Format { get; set; } = StatementFormat.PDF;
    
    public bool IncludeTransactionDetails { get; set; } = true;
    
    public bool IncludeFeeBreakdown { get; set; } = true;
    
    public string? DeliveryEmail { get; set; }
}

/// <summary>
/// Result of card statement generation
/// </summary>
public class CardStatementResult
{
    public bool Success { get; set; }
    public Guid StatementId { get; set; }
    public string FileName { get; set; } = string.Empty;
    public byte[]? Content { get; set; }
    public StatementFormat Format { get; set; }
    public DateTime GeneratedDate { get; set; }
    public string? ErrorMessage { get; set; }
}

/// <summary>
/// Card statement DTO
/// </summary>
public class CardStatementData
{
    public Guid Id { get; set; }
    public Guid CardId { get; set; }
    public string CardNumber { get; set; } = string.Empty;
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    public DateTime GeneratedDate { get; set; }
    public StatementFormat Format { get; set; }
    public string FileName { get; set; } = string.Empty;
    public int TransactionCount { get; set; }
    public decimal TotalSpent { get; set; }
    public decimal TotalFees { get; set; }
    public decimal PreviousBalance { get; set; }
    public decimal CurrentBalance { get; set; }
    public decimal MinimumPayment { get; set; }
    public DateTime? PaymentDueDate { get; set; }
}


