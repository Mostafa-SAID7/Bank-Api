using Bank.CoreBanking.Domain.Entities;

namespace Bank.CoreBanking.Application;

/// <summary>
/// Repository interface for Ledger entries
/// Owned by Core Banking module
/// Immutable entries - append-only
/// </summary>
public interface ILedgerRepository
{
    Task AddAsync(Ledger entry, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Ledger>> GetByAccountAsync(Guid accountId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Ledger>> GetByTransactionAsync(Guid transactionId, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
