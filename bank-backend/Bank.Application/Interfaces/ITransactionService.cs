using Bank.Application.DTOs;
using Bank.Domain.Entities;
using Bank.Domain.Enums;

namespace Bank.Application.Interfaces;

public interface ITransactionService
{
    Task<Transaction> InitiateTransactionAsync(Guid fromAccountId, Guid toAccountId, decimal amount, TransactionType type, string description);
    Task<Transaction> CreateTransactionAsync(CreateTransactionRequest request); // New method for consistency
    Task<IEnumerable<Transaction>> GetTransactionHistoryAsync(Guid accountId);
    Task<Transaction?> GetTransactionByIdAsync(Guid id);
    
    // Advanced search and filtering
    Task<(IEnumerable<Transaction> Transactions, int TotalCount)> SearchTransactionsAsync(
        TransactionSearchCriteria criteria, 
        int pageNumber = 1, 
        int pageSize = 50);
    
    Task<IEnumerable<Transaction>> GetTransactionsByDateRangeAsync(
        Guid accountId, 
        DateTime fromDate, 
        DateTime toDate);
    
    Task<IEnumerable<Transaction>> GetTransactionsByTypeAsync(
        Guid accountId, 
        TransactionType type);
    
    Task<IEnumerable<Transaction>> GetTransactionsByAmountRangeAsync(
        Guid accountId, 
        decimal minAmount, 
        decimal maxAmount);
    
    Task<IEnumerable<Transaction>> GetTransactionsByStatusAsync(
        Guid accountId, 
        TransactionStatus status);
    
    // Export functionality
    Task<byte[]> ExportTransactionsToCsvAsync(TransactionSearchCriteria criteria);
    Task<byte[]> ExportTransactionsToExcelAsync(TransactionSearchCriteria criteria);
    
    // Statistics
    Task<TransactionStatistics> GetTransactionStatisticsAsync(
        Guid accountId, 
        DateTime fromDate, 
        DateTime toDate);
}
