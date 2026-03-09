using Bank.Domain.Entities;
using Bank.Domain.Enums;

namespace Bank.Domain.Interfaces;

/// <summary>
/// Repository interface for Card entity operations
/// </summary>
public interface ICardRepository : IRepository<Card>
{
    /// <summary>
    /// Get card with account information
    /// </summary>
    Task<Card?> GetCardWithAccountAsync(Guid cardId);
    
    /// <summary>
    /// Get all cards for a specific customer
    /// </summary>
    Task<List<Card>> GetCardsByCustomerIdAsync(Guid customerId);
    
    /// <summary>
    /// Get cards by account ID
    /// </summary>
    Task<List<Card>> GetCardsByAccountIdAsync(Guid accountId);
    
    /// <summary>
    /// Check if card number is unique
    /// </summary>
    Task<bool> IsCardNumberUniqueAsync(string cardNumber);
    
    /// <summary>
    /// Get card by card number (for transaction processing)
    /// </summary>
    Task<Card?> GetCardByCardNumberAsync(string cardNumber);
    
    /// <summary>
    /// Get cards expiring within specified days
    /// </summary>
    Task<List<Card>> GetCardsExpiringWithinDaysAsync(int days);
    
    /// <summary>
    /// Get cards expiring in specified days (for renewal processing)
    /// </summary>
    Task<List<Card>> GetCardsExpiringInDaysAsync(int daysBeforeExpiry);
    
    /// <summary>
    /// Get blocked cards for a customer
    /// </summary>
    Task<List<Card>> GetBlockedCardsByCustomerIdAsync(Guid customerId);
    
    /// <summary>
    /// Get active cards for a customer
    /// </summary>
    Task<List<Card>> GetActiveCardsByCustomerIdAsync(Guid customerId);
    
    /// <summary>
    /// Update card status and create history record
    /// </summary>
    Task UpdateCardStatusAsync(Guid cardId, CardStatus newStatus, string? reason = null, Guid? changedBy = null);
}