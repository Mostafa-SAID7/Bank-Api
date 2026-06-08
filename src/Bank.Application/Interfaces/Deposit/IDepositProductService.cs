using Bank.Application.DTOs;
using Bank.Domain.Enums;

namespace Bank.Application.Interfaces;

/// <summary>
/// Service for managing deposit products (CRUD operations)
/// </summary>
public interface IDepositProductService
{
    Task<DepositProductDto?> GetDepositProductAsync(Guid productId);
    Task<IEnumerable<DepositProductDto>> GetActiveDepositProductsAsync();
    Task<IEnumerable<DepositProductDto>> GetDepositProductsByTypeAsync(DepositProductType productType);
    Task<DepositProductDto> CreateDepositProductAsync(CreateDepositProductRequest request, Guid createdByUserId);
    Task<DepositProductDto> UpdateDepositProductAsync(Guid productId, UpdateDepositProductRequest request, Guid updatedByUserId);
    Task<bool> DeactivateDepositProductAsync(Guid productId, Guid deactivatedByUserId);
}
