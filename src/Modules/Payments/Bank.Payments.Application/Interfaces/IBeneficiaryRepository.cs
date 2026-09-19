using Bank.Payments.Domain.Entities;

namespace Bank.Payments.Application;

/// <summary>
/// Repository interface for Beneficiary aggregate
/// Owned by Payments module - defines persistence contract
/// </summary>
public interface IBeneficiaryRepository
{
    Task AddAsync(Beneficiary beneficiary, CancellationToken cancellationToken = default);
    Task UpdateAsync(Beneficiary beneficiary, CancellationToken cancellationToken = default);
    Task<Beneficiary?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Beneficiary>> GetByCustomerIdAsync(Guid customerId, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
