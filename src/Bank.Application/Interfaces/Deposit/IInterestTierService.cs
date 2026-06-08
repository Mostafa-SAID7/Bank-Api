using Bank.Application.DTOs;

namespace Bank.Application.Interfaces;

/// <summary>
/// Service for managing interest tiers for deposit products
/// </summary>
public interface IInterestTierService
{
    Task<InterestTierDto> CreateInterestTierAsync(Guid productId, CreateInterestTierRequest request, Guid createdByUserId);
    Task<InterestTierDto> UpdateInterestTierAsync(Guid tierId, UpdateInterestTierRequest request, Guid updatedByUserId);
    Task<bool> DeleteInterestTierAsync(Guid tierId, Guid deletedByUserId);
    Task<IEnumerable<InterestTierDto>> GetInterestTiersAsync(Guid productId);
}
