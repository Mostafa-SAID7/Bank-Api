using Bank.Application.Interfaces;
using Bank.Domain.Entities;
using Bank.Domain.Enums;
using Bank.Domain.Interfaces;
using Bank.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Bank.Infrastructure.Repositories.Deposit;

/// <summary>
/// Repository implementation for InterestTier entity
/// </summary>
public class InterestTierRepository : Repository<InterestTier>, IInterestTierRepository
{
    public InterestTierRepository(BankDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<InterestTier>> GetTiersByProductAsync(Guid productId)
    {
        return await _context.Set<InterestTier>()
            .Where(t => t.DepositProductId == productId)
            .OrderBy(t => t.DisplayOrder)
            .ThenBy(t => t.MinimumBalance)
            .ToListAsync();
    }

    public async Task<IEnumerable<InterestTier>> GetActiveTiersAsync(Guid productId)
    {
        var now = DateTime.UtcNow;
        return await _context.Set<InterestTier>()
            .Where(t => t.DepositProductId == productId &&
                       t.IsActive &&
                       (!t.EffectiveFromDate.HasValue || t.EffectiveFromDate.Value <= now) &&
                       (!t.EffectiveToDate.HasValue || t.EffectiveToDate.Value >= now))
            .OrderBy(t => t.DisplayOrder)
            .ThenBy(t => t.MinimumBalance)
            .ToListAsync();
    }

    public async Task<InterestTier?> GetApplicableTierAsync(Guid productId, decimal balance)
    {
        var now = DateTime.UtcNow;
        return await _context.Set<InterestTier>()
            .Where(t => t.DepositProductId == productId &&
                       t.IsActive &&
                       t.MinimumBalance <= balance &&
                       (!t.MaximumBalance.HasValue || t.MaximumBalance.Value >= balance) &&
                       (!t.EffectiveFromDate.HasValue || t.EffectiveFromDate.Value <= now) &&
                       (!t.EffectiveToDate.HasValue || t.EffectiveToDate.Value >= now))
            .OrderByDescending(t => t.MinimumBalance)
            .FirstOrDefaultAsync();
    }
}