using Bank.Domain.Entities;
using Bank.Domain.Enums;

namespace Bank.Domain.Interfaces;

/// <summary>
/// Repository interface for card transaction operations
/// </summary>
public interface ICardTransactionRepository
{
    /// <summary>
    /// Add a new card transaction
    /// </summary>
    Task<CardTransaction> AddTransactionAsync(CardTransaction transaction);

    /// <summary>
    /// Update an existing card transaction
    /// </summary>
    Task UpdateTransactionAsync(CardTransaction transaction);

    /// <summary>
    /// Get card transaction by ID
    /// </summary>
    Task<CardTransaction?> GetTransactionByIdAsync(Guid transactionId);

    /// <summary>
    /// Get card transactions by date range
    /// </summary>
    Task<List<CardTransaction>> GetTransactionsByDateRangeAsync(Guid cardId, DateTime fromDate, DateTime toDate);

    /// <summary>
    /// Get today's transactions for a card
    /// </summary>
    Task<List<CardTransaction>> GetTodayTransactionsAsync(Guid cardId);

    /// <summary>
    /// Search card transactions with criteria
    /// </summary>
    Task<(List<CardTransaction> transactions, int totalCount)> SearchTransactionsAsync(
        Guid cardId, 
        DateTime? fromDate = null, 
        DateTime? toDate = null, 
        CardTransactionType? type = null,
        CardTransactionStatus? status = null,
        decimal? minAmount = null,
        decimal? maxAmount = null,
        string? merchantName = null,
        int page = 1,
        int pageSize = 20);

    /// <summary>
    /// Get transactions for settlement
    /// </summary>
    Task<List<CardTransaction>> GetTransactionsForSettlementAsync(DateTime settlementDate, List<string>? transactionIds = null, CardNetwork? network = null);

    /// <summary>
    /// Get settled transactions by date
    /// </summary>
    Task<List<CardTransaction>> GetSettledTransactionsByDateAsync(DateTime settlementDate);

    /// <summary>
    /// Add card authorization
    /// </summary>
    Task<CardAuthorization> AddAuthorizationAsync(CardAuthorization authorization);

    /// <summary>
    /// Update card authorization
    /// </summary>
    Task UpdateAuthorizationAsync(CardAuthorization authorization);

    /// <summary>
    /// Get authorization by code
    /// </summary>
    Task<CardAuthorization?> GetAuthorizationByCodeAsync(string authorizationCode);

    /// <summary>
    /// Add card statement
    /// </summary>
    Task<CardStatement> AddStatementAsync(CardStatement statement);

    /// <summary>
    /// Get card statement by ID
    /// </summary>
    Task<CardStatement?> GetStatementByIdAsync(Guid statementId);

    /// <summary>
    /// Get card statements for a card
    /// </summary>
    Task<List<CardStatement>> GetCardStatementsAsync(Guid cardId, int? limit = null);
}