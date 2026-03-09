using Bank.Domain.Common;
using Bank.Domain.Enums;

namespace Bank.Domain.Entities;

/// <summary>
/// Represents a bank card (debit/credit) issued to a customer
/// </summary>
public class Card : BaseEntity
{
    /// <summary>
    /// Unique card number (encrypted in database)
    /// </summary>
    public string CardNumber { get; set; } = string.Empty;
    
    /// <summary>
    /// Masked card number for display (e.g., ****-****-****-1234)
    /// </summary>
    public string MaskedCardNumber { get; set; } = string.Empty;
    
    /// <summary>
    /// Customer who owns this card
    /// </summary>
    public Guid CustomerId { get; set; }
    public User Customer { get; set; } = null!;
    
    /// <summary>
    /// Account linked to this card
    /// </summary>
    public Guid AccountId { get; set; }
    public Account Account { get; set; } = null!;
    
    /// <summary>
    /// Type of card (debit, credit, etc.)
    /// </summary>
    public CardType Type { get; set; }
    
    /// <summary>
    /// Current status of the card
    /// </summary>
    public CardStatus Status { get; set; } = CardStatus.Inactive;
    
    /// <summary>
    /// Card expiry date
    /// </summary>
    public DateTime ExpiryDate { get; set; }
    
    /// <summary>
    /// Card issue date
    /// </summary>
    public DateTime IssueDate { get; set; } = DateTime.UtcNow;
    
    /// <summary>
    /// Card activation date
    /// </summary>
    public DateTime? ActivationDate { get; set; }
    
    /// <summary>
    /// Channel used for card activation
    /// </summary>
    public CardActivationChannel? ActivationChannel { get; set; }
    
    /// <summary>
    /// CVV/Security code (encrypted in database)
    /// </summary>
    public string SecurityCode { get; set; } = string.Empty;
    
    /// <summary>
    /// Daily spending limit
    /// </summary>
    public decimal DailyLimit { get; set; } = 5000m;
    
    /// <summary>
    /// Monthly spending limit
    /// </summary>
    public decimal MonthlyLimit { get; set; } = 50000m;
    
    /// <summary>
    /// ATM withdrawal daily limit
    /// </summary>
    public decimal AtmDailyLimit { get; set; } = 2000m;
    
    /// <summary>
    /// Whether contactless payments are enabled
    /// </summary>
    public bool ContactlessEnabled { get; set; } = true;
    
    /// <summary>
    /// Whether online transactions are enabled
    /// </summary>
    public bool OnlineTransactionsEnabled { get; set; } = true;
    
    /// <summary>
    /// Whether international transactions are enabled
    /// </summary>
    public bool InternationalTransactionsEnabled { get; set; } = false;
    
    /// <summary>
    /// PIN hash (encrypted)
    /// </summary>
    public string? PinHash { get; set; }
    
    /// <summary>
    /// PIN set date
    /// </summary>
    public DateTime? PinSetDate { get; set; }
    
    /// <summary>
    /// Number of consecutive failed PIN attempts
    /// </summary>
    public int FailedPinAttempts { get; set; } = 0;
    
    /// <summary>
    /// Date when card was last blocked
    /// </summary>
    public DateTime? LastBlockedDate { get; set; }
    
    /// <summary>
    /// Reason for last block
    /// </summary>
    public CardBlockReason? LastBlockReason { get; set; }
    
    /// <summary>
    /// Merchant categories that are blocked for this card
    /// </summary>
    public string? BlockedMerchantCategories { get; set; } // JSON array of MerchantCategory values
    
    /// <summary>
    /// Card name/label for customer identification
    /// </summary>
    public string? CardName { get; set; }
    
    /// <summary>
    /// Card transactions
    /// </summary>
    public ICollection<CardTransaction> Transactions { get; set; } = new List<CardTransaction>();
    
    /// <summary>
    /// Card status history
    /// </summary>
    public ICollection<CardStatusHistory> StatusHistory { get; set; } = new List<CardStatusHistory>();

    // Domain methods
    
    /// <summary>
    /// Check if card is active and can be used for transactions
    /// </summary>
    public bool IsActive()
    {
        return Status == CardStatus.Active && 
               ExpiryDate > DateTime.UtcNow &&
               FailedPinAttempts < 3;
    }
    
    /// <summary>
    /// Check if card is blocked
    /// </summary>
    public bool IsBlocked()
    {
        return Status == CardStatus.Blocked || 
               Status == CardStatus.Lost || 
               Status == CardStatus.Stolen ||
               FailedPinAttempts >= 3;
    }
    
    /// <summary>
    /// Check if card is expired
    /// </summary>
    public bool IsExpired()
    {
        return ExpiryDate <= DateTime.UtcNow;
    }
    
    /// <summary>
    /// Activate the card
    /// </summary>
    public void Activate(CardActivationChannel channel)
    {
        if (Status == CardStatus.Inactive)
        {
            Status = CardStatus.Active;
            ActivationDate = DateTime.UtcNow;
            ActivationChannel = channel;
        }
    }
    
    /// <summary>
    /// Block the card with specified reason
    /// </summary>
    public void Block(CardBlockReason reason)
    {
        if (Status == CardStatus.Active)
        {
            Status = CardStatus.Blocked;
            LastBlockedDate = DateTime.UtcNow;
            LastBlockReason = reason;
        }
    }
    
    /// <summary>
    /// Unblock the card
    /// </summary>
    public void Unblock()
    {
        if (Status == CardStatus.Blocked)
        {
            Status = CardStatus.Active;
            FailedPinAttempts = 0;
        }
    }
    
    /// <summary>
    /// Check if transaction amount is within limits
    /// </summary>
    public bool IsWithinLimits(decimal amount, DateTime transactionDate)
    {
        // Check daily limit
        var dailySpent = GetDailySpent(transactionDate);
        if (dailySpent + amount > DailyLimit)
            return false;
            
        // Check monthly limit
        var monthlySpent = GetMonthlySpent(transactionDate);
        if (monthlySpent + amount > MonthlyLimit)
            return false;
            
        return true;
    }
    
    /// <summary>
    /// Check if merchant category is allowed
    /// </summary>
    public bool IsMerchantCategoryAllowed(MerchantCategory category)
    {
        if (string.IsNullOrEmpty(BlockedMerchantCategories))
            return true;
            
        try
        {
            var blockedCategories = System.Text.Json.JsonSerializer.Deserialize<List<MerchantCategory>>(BlockedMerchantCategories);
            return blockedCategories?.Contains(category) != true;
        }
        catch
        {
            // If deserialization fails, allow the transaction
            return true;
        }
    }
    
    /// <summary>
    /// Increment failed PIN attempts
    /// </summary>
    public void IncrementFailedPinAttempts()
    {
        FailedPinAttempts++;
        if (FailedPinAttempts >= 3)
        {
            Block(CardBlockReason.ExcessiveDeclines);
        }
    }
    
    /// <summary>
    /// Reset failed PIN attempts
    /// </summary>
    public void ResetFailedPinAttempts()
    {
        FailedPinAttempts = 0;
    }
    
    /// <summary>
    /// Generate masked card number for display
    /// </summary>
    public static string GenerateMaskedCardNumber(string cardNumber)
    {
        if (string.IsNullOrEmpty(cardNumber) || cardNumber.Length < 4)
            return "****-****-****-****";
            
        var lastFour = cardNumber.Substring(cardNumber.Length - 4);
        return $"****-****-****-{lastFour}";
    }
    
    private decimal GetDailySpent(DateTime date)
    {
        return Transactions
            .Where(t => t.TransactionDate.Date == date.Date && 
                       t.Status == CardTransactionStatus.Settled &&
                       (t.TransactionType == CardTransactionType.Purchase || t.TransactionType == CardTransactionType.Withdrawal))
            .Sum(t => t.Amount);
    }
    
    private decimal GetMonthlySpent(DateTime date)
    {
        return Transactions
            .Where(t => t.TransactionDate.Year == date.Year && 
                       t.TransactionDate.Month == date.Month &&
                       t.Status == CardTransactionStatus.Settled &&
                       (t.TransactionType == CardTransactionType.Purchase || t.TransactionType == CardTransactionType.Withdrawal))
            .Sum(t => t.Amount);
    }
}