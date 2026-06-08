using Bank.Domain.Entities;
using Bank.Domain.Enums;

namespace Bank.Domain.Interfaces;

/// <summary>
/// Repository interface for DepositProduct entity
/// </summary>
public interface IDepositProductRepository : IRepository<DepositProduct>
{
    Task<IEnumerable<DepositProduct>> GetActiveProductsAsync();
    Task<IEnumerable<DepositProduct>> GetProductsByTypeAsync(DepositProductType productType);
    Task<DepositProduct?> GetProductWithTiersAsync(Guid productId);
    Task<IEnumerable<DepositProduct>> GetProductsWithPromotionalRatesAsync();
}
