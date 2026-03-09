using Bank.Domain.Common;
using Bank.Domain.Enums;

namespace Bank.Domain.Entities;

/// <summary>
/// Represents a card statement
/// </summary>
public class CardStatement : BaseEntity
{
    public Guid CardId { get; set; }
    public Card? Card { get; set; }
    
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    public DateTime GeneratedDate { get; set; }
    
    public StatementFormat Format { get; set; }
    public string? FileName { get; set; }
    public string? FilePath { get; set; }
    
    public int TransactionCount { get; set; }
    public decimal TotalSpent { get; set; }
    public decimal TotalFees { get; set; }
    public decimal PreviousBalance { get; set; }
    public decimal CurrentBalance { get; set; }
    public decimal MinimumPayment { get; set; }
    public DateTime? PaymentDueDate { get; set; }
    
    public StatementStatus Status { get; set; }
    public StatementDeliveryMethod? DeliveryMethod { get; set; }
    public string? DeliveryAddress { get; set; }
    public DateTime? DeliveredAt { get; set; }
    
    public string? GeneratedBy { get; set; }
    public string? Notes { get; set; }
}