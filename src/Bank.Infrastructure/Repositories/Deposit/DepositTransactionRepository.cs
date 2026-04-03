using Bank.Application.Interfaces;
using Bank.Domain.Entities;
using Bank.Domain.Enums;
using Bank.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Bank.Infrastructure.Repositories.Deposit;

/// <summary>
/// Repository implementation for DepositTransaction entity
/// </summary>
public class DepositTransactionRepository : Repository<DepositTransaction>, IDepositTransactionRepository
{
    public DepositTransactionRepository(BankDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<DepositTransaction>> GetDepositTransactionsAsync(Guid depositId)
    {
        return await _context.Set<DepositTransaction>()
            .Where(t => t.FixedDepositId == depositId)
            .OrderByDescending(t => t.TransactionDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<DepositTransaction>> GetTransactionsByTypeAsync(Guid depositId, DepositTransactionType transactionType)
    {
        return await _context.Set<DepositTransaction>()
            .Where(t => t.FixedDepositId == depositId && t.TransactionType == transactionType)
            .OrderByDescending(t => t.TransactionDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<DepositTransaction>> GetTransactionsByDateRangeAsync(Guid depositId, DateTime fromDate, DateTime toDate)
    {
        return await _context.Set<DepositTransaction>()
            .Where(t => t.FixedDepositId == depositId &&
                       t.TransactionDate >= fromDate &&
                       t.TransactionDate <= toDate)
            .OrderByDescending(t => t.TransactionDate)
            .ToListAsync();
    }

    public async Task<decimal> GetTotalInterestCreditedAsync(Guid depositId)
    {
        return await _context.Set<DepositTransaction>()
            .Where(t => t.FixedDepositId == depositId &&
                       t.TransactionType == DepositTransactionType.InterestCredit &&
                       t.Status == TransactionStatus.Completed)
            .SumAsync(t => t.Amount);
    }
}
