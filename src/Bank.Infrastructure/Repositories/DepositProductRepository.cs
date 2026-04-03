using Bank.Application.Interfaces;
using Bank.Domain.Entities;
using Bank.Domain.Enums;
using Bank.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Bank.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for DepositProduct entity
/// </summary>
public class DepositProductRepository : Repository<DepositProduct>, IDepositProductRepository
{
    public DepositProductRepository(BankDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<DepositProduct>> GetActiveProductsAsync()
    {
        return await _context.Set<DepositProduct>()
            .Where(p => p.IsActive)
            .Include(p => p.InterestTiers)
            .OrderBy(p => p.Name)
            .ToListAsync();
    }

    public async Task<IEnumerable<DepositProduct>> GetProductsByTypeAsync(DepositProductType productType)
    {
        return await _context.Set<DepositProduct>()
            .Where(p => p.IsActive && p.ProductType == productType)
            .Include(p => p.InterestTiers)
            .OrderBy(p => p.Name)
            .ToListAsync();
    }

    public async Task<DepositProduct?> GetProductWithTiersAsync(Guid productId)
    {
        return await _context.Set<DepositProduct>()
            .Include(p => p.InterestTiers.Where(t => t.IsActive))
            .FirstOrDefaultAsync(p => p.Id == productId);
    }

    public async Task<IEnumerable<DepositProduct>> GetProductsWithPromotionalRatesAsync()
    {
        var now = DateTime.UtcNow;
        return await _context.Set<DepositProduct>()
            .Where(p => p.IsActive && 
                       p.PromotionalRate.HasValue &&
                       p.PromotionalRateStartDate.HasValue &&
                       p.PromotionalRateEndDate.HasValue &&
                       now >= p.PromotionalRateStartDate.Value &&
                       now <= p.PromotionalRateEndDate.Value)
            .Include(p => p.InterestTiers)
            .ToListAsync();
    }
}