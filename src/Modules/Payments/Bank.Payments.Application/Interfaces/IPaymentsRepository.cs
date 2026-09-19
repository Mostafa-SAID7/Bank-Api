using Bank.Payments.Domain.Entities;

namespace Bank.Payments.Application;

/// <summary>
/// Repository interface for Payment aggregate
/// Owned by Payments module - defines persistence contract
/// </summary>
public interface IPaymentsRepository
{
    Task AddAsync(Payment payment, CancellationToken cancellationToken = default);
    Task UpdateAsync(Payment payment, CancellationToken cancellationToken = default);
    Task<Payment?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Payment?> FindByIdempotencyKeyAsync(string idempotencyKey, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
