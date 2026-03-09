using Bank.Domain.Common;
using Bank.Domain.Enums;

namespace Bank.Domain.Entities;

/// <summary>
/// Represents a card authorization
/// </summary>
public class CardAuthorization : BaseEntity
{
    public Guid CardId { get; set; }
    public Card? Card { get; set; }
    
    public string AuthorizationCode { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "USD";
    
    public string MerchantId { get; set; } = string.Empty;
    public string MerchantName { get; set; } = string.Empty;
    public MerchantCategory MerchantCategory { get; set; }
    public string? MerchantCountry { get; set; }
    
    public DateTime TransactionDate { get; set; }
    public DateTime ExpiresAt { get; set; }
    
    public CardTransactionStatus Status { get; set; }
    
    public bool IsInternational { get; set; }
    public bool IsOnline { get; set; }
    public bool IsContactless { get; set; }
    
    public decimal? CapturedAmount { get; set; }
    public DateTime? CaptureDate { get; set; }
    
    public DateTime? VoidDate { get; set; }
    public string? VoidReason { get; set; }
    
    public string? ProcessorResponse { get; set; }
    public string? NetworkTransactionId { get; set; }
}