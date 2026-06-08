using Bank.Domain.Entities;

namespace Bank.Domain.Interfaces;

/// <summary>
/// Repository interface for InterestTier entity
/// </summary>
public interface IInterestTierRepository : IRepository<InterestTier>
{
    Task<IEnumerable<InterestTier>> GetTiersByProductAsync(Guid productId);
    Task<IEnumerable<InterestTier>> GetActiveTiersAsync(Guid productId);
    Task<InterestTier?> GetApplicableTierAsync(Guid productId, decimal balance);
}
