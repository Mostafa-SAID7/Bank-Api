using Bank.CoreBanking.Domain.Entities;

namespace Bank.CoreBanking.Application;

/// <summary>
/// Repository interface for Transaction aggregate
/// Owned by Core Banking module
/// </summary>
public interface ITransactionRepository
{
    Task AddAsync(Transaction transaction, CancellationToken cancellationToken = default);
    Task UpdateAsync(Transaction transaction, CancellationToken cancellationToken = default);
    Task<Transaction?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Transaction?> FindByIdempotencyKeyAsync(string idempotencyKey, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Transaction>> GetByAccountAsync(Guid accountId, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
