using Bank.Domain.Entities;
using Bank.Domain.Enums;

namespace Bank.Application.Interfaces;

public interface ITransactionService
{
    Task<Transaction> InitiateTransactionAsync(Guid fromAccountId, Guid toAccountId, decimal amount, TransactionType type, string description);
    Task<IEnumerable<Transaction>> GetTransactionHistoryAsync(Guid accountId);
    Task<Transaction?> GetTransactionByIdAsync(Guid id);
}
