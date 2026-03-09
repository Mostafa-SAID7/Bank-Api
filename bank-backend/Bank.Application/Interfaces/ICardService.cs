using Bank.Application.DTOs;
using Bank.Domain.Enums;

namespace Bank.Application.Interfaces;

/// <summary>
/// Service interface for card management operations
/// </summary>
public interface ICardService
{
    /// <summary>
    /// Issue a new card for a customer account
    /// </summary>
    Task<CardIssuanceResult> IssueCardAsync(CardIssuanceRequest request);
    
    /// <summary>
    /// Activate a card using activation code
    /// </summary>
    Task<CardActivationResult> ActivateCardAsync(CardActivationRequest request);
    
    /// <summary>
    /// Block a card with specified reason
    /// </summary>
    Task<CardBlockResult> BlockCardAsync(CardBlockRequest request);
    
    /// <summary>
    /// Unblock a previously blocked card
    /// </summary>
    Task<CardBlockResult> UnblockCardAsync(CardUnblockRequest request);
    
    /// <summary>
    /// Update card spending limits
    /// </summary>
    Task<CardLimitUpdateResult> UpdateLimitsAsync(CardLimitUpdateRequest request);
    
    /// <summary>
    /// Get card details by card ID
    /// </summary>
    Task<CardDetailsDto?> GetCardDetailsAsync(Guid cardId, Guid customerId);
    
    /// <summary>
    /// Get all cards for a customer
    /// </summary>
    Task<List<CardSummaryDto>> GetCustomerCardsAsync(Guid customerId);
    
    /// <summary>
    /// Get card transactions with filtering and pagination
    /// </summary>
    Task<PagedResult<CardTransactionDto>> GetCardTransactionsAsync(CardTransactionSearchRequest request);
    
    /// <summary>
    /// Change card PIN
    /// </summary>
    Task<CardPinChangeResult> ChangePinAsync(CardPinChangeRequest request);
    
    /// <summary>
    /// Reset card PIN (generates new PIN)
    /// </summary>
    Task<CardPinResetResult> ResetPinAsync(CardPinResetRequest request);
    
    /// <summary>
    /// Update card merchant category restrictions
    /// </summary>
    Task<CardMerchantRestrictionsResult> UpdateMerchantRestrictionsAsync(CardMerchantRestrictionsRequest request);
    
    /// <summary>
    /// Enable/disable contactless payments
    /// </summary>
    Task<CardContactlessResult> UpdateContactlessSettingsAsync(CardContactlessRequest request);
    
    /// <summary>
    /// Enable/disable online transactions
    /// </summary>
    Task<CardOnlineTransactionsResult> UpdateOnlineTransactionsAsync(CardOnlineTransactionsRequest request);
    
    /// <summary>
    /// Enable/disable international transactions
    /// </summary>
    Task<CardInternationalTransactionsResult> UpdateInternationalTransactionsAsync(CardInternationalTransactionsRequest request);
    
    /// <summary>
    /// Get card usage statistics
    /// </summary>
    Task<CardUsageStatsDto> GetCardUsageStatsAsync(Guid cardId, Guid customerId, DateTime fromDate, DateTime toDate);
    
    /// <summary>
    /// Validate card for transaction
    /// </summary>
    Task<CardValidationResult> ValidateCardForTransactionAsync(CardValidationRequest request);
    
    /// <summary>
    /// Generate new card number (internal use)
    /// </summary>
    Task<string> GenerateCardNumberAsync(CardType cardType);
    
    /// <summary>
    /// Generate security code (internal use)
    /// </summary>
    Task<string> GenerateSecurityCodeAsync();
}