using Bank.CoreBanking.Application;
using Bank.CoreBanking.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Bank.CoreBanking.Infrastructure.Data.Repositories;

public sealed class TransactionRepository : ITransactionRepository
{
    private readonly CoreBankingDbContext _context;

    public TransactionRepository(CoreBankingDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Transaction transaction, CancellationToken cancellationToken = default)
    {
        await _context.Transactions.AddAsync(transaction, cancellationToken);
    }

    public async Task UpdateAsync(Transaction transaction, CancellationToken cancellationToken = default)
    {
        _context.Transactions.Update(transaction);
        await Task.CompletedTask;
    }

    public async Task<Transaction?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Transactions.FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
    }

    public async Task<Transaction?> FindByIdempotencyKeyAsync(string idempotencyKey, CancellationToken cancellationToken = default)
    {
        return await _context.Transactions.FirstOrDefaultAsync(t => t.IdempotencyKey == idempotencyKey, cancellationToken);
    }

    public async Task<IReadOnlyList<Transaction>> GetByAccountAsync(Guid accountId, CancellationToken cancellationToken = default)
    {
        return await _context.Transactions
            .Where(t => t.FromAccountId == accountId || t.ToAccountId == accountId)
            .ToListAsync(cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}
