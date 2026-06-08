using Bank.Domain.Entities;
using Bank.Domain.Enums;

namespace Bank.Domain.Interfaces;

/// <summary>
/// Repository interface for DepositTransaction entity
/// </summary>
public interface IDepositTransactionRepository : IRepository<DepositTransaction>
{
    Task<IEnumerable<DepositTransaction>> GetDepositTransactionsAsync(Guid depositId);
    Task<IEnumerable<DepositTransaction>> GetTransactionsByTypeAsync(Guid depositId, DepositTransactionType transactionType);
    Task<IEnumerable<DepositTransaction>> GetTransactionsByDateRangeAsync(Guid depositId, DateTime fromDate, DateTime toDate);
    Task<decimal> GetTotalInterestCreditedAsync(Guid depositId);
}
