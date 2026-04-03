using Bank.Domain.Common;
using Bank.Domain.Enums;

namespace Bank.Domain.Entities;

/// <summary>
/// Represents a card transaction
/// </summary>
public class CardTransaction : BaseEntity
{
    public Guid CardId { get; set; }
    public Card? Card { get; set; }
    
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "USD";
    
    public CardTransactionType TransactionType { get; set; }
    public CardTransactionStatus Status { get; set; }
    
    public string MerchantId { get; set; } = string.Empty;
    public string MerchantName { get; set; } = string.Empty;
    public MerchantCategory MerchantCategory { get; set; }
    public string? MerchantCountry { get; set; }
    
    public string? Description { get; set; }
    public string? Reference { get; set; }
    public string? AuthorizationCode { get; set; }
    
    public DateTime TransactionDate { get; set; }
    public DateTime? SettlementDate { get; set; }
    
    public bool IsInternational { get; set; }
    public bool IsOnline { get; set; }
    public bool IsContactless { get; set; }
    
    public decimal? Fees { get; set; }
    public string? FeeBreakdown { get; set; }
    
    public Guid? OriginalTransactionId { get; set; }
    public CardTransaction? OriginalTransaction { get; set; }
    
    public string? DeclineReason { get; set; }
    public string? ProcessorResponse { get; set; }
    
    // Navigation properties
    public List<CardTransaction> RelatedTransactions { get; set; } = new();
    
    /// <summary>
    /// Check if the transaction was successful
    /// </summary>
    public bool IsSuccessful()
    {
        return Status == CardTransactionStatus.Settled || Status == CardTransactionStatus.Authorized;
    }
}