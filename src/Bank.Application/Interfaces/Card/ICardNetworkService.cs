using Bank.Application.DTOs;
using Bank.Domain.Entities;
using Bank.Domain.Enums;

namespace Bank.Application.Interfaces;

/// <summary>
/// Service for card network integration including authorization, settlement, and transaction processing
/// </summary>
public interface ICardNetworkService
{
    /// <summary>
    /// Authorize a card transaction
    /// </summary>
    Task<CardAuthorizationResult> AuthorizeTransactionAsync(CardAuthorizationRequest request);

    /// <summary>
    /// Capture a previously authorized transaction
    /// </summary>
    Task<CardCaptureResult> CaptureTransactionAsync(CardCaptureRequest request);

    /// <summary>
    /// Void an authorized transaction
    /// </summary>
    Task<CardVoidResult> VoidTransactionAsync(CardVoidRequest request);

    /// <summary>
    /// Refund a captured transaction
    /// </summary>
    Task<CardRefundResult> RefundTransactionAsync(CardRefundRequest request);

    /// <summary>
    /// Process settlement for card transactions
    /// </summary>
    Task<CardSettlementResult> ProcessSettlementAsync(CardSettlementRequest request);

    /// <summary>
    /// Get settlement report for a specific date
    /// </summary>
    Task<CardSettlementReport> GetSettlementReportAsync(DateTime settlementDate);

    /// <summary>
    /// Process card transaction and link to account
    /// </summary>
    Task<CardTransactionResult> ProcessCardTransactionAsync(CardTransactionRequest request);

    /// <summary>
    /// Get card transaction details
    /// </summary>
    Task<CardTransactionDto> GetCardTransactionAsync(Guid transactionId);

    /// <summary>
    /// Get card transactions for a specific card
    /// </summary>
    Task<Bank.Domain.Common.PagedResult<CardTransactionDto>> GetCardTransactionsAsync(CardTransactionSearchRequest request);

    /// <summary>
    /// Generate card statement
    /// </summary>
    Task<CardStatementResult> GenerateCardStatementAsync(CardStatementRequest request);

    /// <summary>
    /// Get card statement by ID
    /// </summary>
    Task<CardStatementDto> GetCardStatementAsync(Guid statementId);

    /// <summary>
    /// Get card statements for a specific card
    /// </summary>
    Task<List<CardStatementDto>> GetCardStatementsAsync(Guid cardId, int? limit = null);

    /// <summary>
    /// Check for cards approaching expiry and process renewals
    /// </summary>
    Task<CardRenewalResult> ProcessCardRenewalsAsync(int daysBeforeExpiry = 60);

    /// <summary>
    /// Renew a specific card
    /// </summary>
    Task<CardRenewalResult> RenewCardAsync(Guid cardId);

    /// <summary>
    /// Get cards approaching expiry
    /// </summary>
    Task<List<CardSummaryDto>> GetCardsApproachingExpiryAsync(int daysBeforeExpiry = 60);

    /// <summary>
    /// Validate card for transaction
    /// </summary>
    Task<CardValidationResult> ValidateCardAsync(CardValidationRequest request);

    /// <summary>
    /// Get card network status
    /// </summary>
    Task<CardNetworkStatus> GetNetworkStatusAsync(CardNetwork network);

    /// <summary>
    /// Process batch settlement file
    /// </summary>
    Task<BatchSettlementResult> ProcessBatchSettlementAsync(string settlementFilePath);

    /// <summary>
    /// Get transaction fees for a card transaction
    /// </summary>
    Task<CardTransactionFees> CalculateTransactionFeesAsync(CardTransactionFeeRequest request);
}