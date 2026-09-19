using Bank.CoreBanking.Application;
using Bank.CoreBanking.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Bank.CoreBanking.Infrastructure.Data.Repositories;

/// <summary>
/// Repository for Ledger entries (append-only, immutable)
/// Implements double-entry bookkeeping
/// </summary>
public sealed class LedgerRepository : ILedgerRepository
{
    private readonly CoreBankingDbContext _context;

    public LedgerRepository(CoreBankingDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Ledger entry, CancellationToken cancellationToken = default)
    {
        await _context.LedgerEntries.AddAsync(entry, cancellationToken);
    }

    public async Task<IReadOnlyList<Ledger>> GetByAccountAsync(Guid accountId, CancellationToken cancellationToken = default)
    {
        return await _context.LedgerEntries
            .Where(l => l.AccountId == accountId)
            .OrderBy(l => l.PostedAtUtc)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Ledger>> GetByTransactionAsync(Guid transactionId, CancellationToken cancellationToken = default)
    {
        return await _context.LedgerEntries
            .Where(l => l.TransactionId == transactionId)
            .OrderBy(l => l.PostedAtUtc)
            .ToListAsync(cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}
