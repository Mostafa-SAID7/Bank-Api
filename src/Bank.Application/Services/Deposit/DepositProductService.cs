using AutoMapper;
using Bank.Application.DTOs;
using Bank.Application.Interfaces;
using Bank.Domain.Entities;
using Bank.Domain.Enums;
using Bank.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Bank.Application.Services;

/// <summary>
/// Service for managing deposit products (CRUD operations)
/// Handles creation, retrieval, update, and deactivation of deposit product definitions
/// </summary>
public sealed class DepositProductService : IDepositProductService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAuditLogService _auditLogService;
    private readonly ILogger<DepositProductService> _logger;
    private readonly IMapper _mapper;

    public DepositProductService(
        IUnitOfWork unitOfWork,
        IAuditLogService auditLogService,
        ILogger<DepositProductService> logger,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _auditLogService = auditLogService;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<DepositProductDto?> GetDepositProductAsync(Guid productId)
    {
        var product = await _unitOfWork.Repository<DepositProduct>().GetByIdAsync(productId);
        return product == null ? null : MapToDepositProductDto(product);
    }

    public async Task<IEnumerable<DepositProductDto>> GetActiveDepositProductsAsync()
    {
        var products = await _unitOfWork.Repository<DepositProduct>()
            .FindAsync(p => p.IsActive);

        return products.Select(MapToDepositProductDto);
    }

    public async Task<IEnumerable<DepositProductDto>> GetDepositProductsByTypeAsync(DepositProductType productType)
    {
        var products = await _unitOfWork.Repository<DepositProduct>()
            .FindAsync(p => p.IsActive && p.ProductType == productType);

        return products.Select(MapToDepositProductDto);
    }

    public async Task<DepositProductDto> CreateDepositProductAsync(CreateDepositProductRequest request, Guid createdByUserId)
    {
        var product = new DepositProduct
        {
            Name = request.Name,
            Description = request.Description,
            ProductType = request.ProductType,
            MinimumTermDays = request.MinimumTermDays,
            MaximumTermDays = request.MaximumTermDays,
            DefaultTermDays = request.DefaultTermDays,
            MinimumBalance = request.MinimumBalance,
            MaximumBalance = request.MaximumBalance,
            MinimumOpeningBalance = request.MinimumOpeningBalance,
            BaseInterestRate = request.BaseInterestRate,
            InterestCalculationMethod = request.InterestCalculationMethod,
            CompoundingFrequency = request.CompoundingFrequency,
            HasTieredRates = request.HasTieredRates,
            AllowPartialWithdrawals = request.AllowPartialWithdrawals,
            PenaltyType = request.PenaltyType,
            PenaltyAmount = request.PenaltyAmount,
            PenaltyPercentage = request.PenaltyPercentage,
            PenaltyFreeDays = request.PenaltyFreeDays,
            DefaultMaturityAction = request.DefaultMaturityAction,
            AllowAutoRenewal = request.AllowAutoRenewal,
            AutoRenewalNoticeDays = request.AutoRenewalNoticeDays,
            PromotionalRateStartDate = request.PromotionalRateStartDate,
            PromotionalRateEndDate = request.PromotionalRateEndDate,
            PromotionalRate = request.PromotionalRate
        };

        await _unitOfWork.Repository<DepositProduct>().AddAsync(product);
        await _unitOfWork.SaveChangesAsync();

        await _auditLogService.LogUserActionAsync(
            createdByUserId,
            "DepositProduct",
            "Create",
            product.Id.ToString(),
            $"Created deposit product: {product.Name}");

        _logger.LogInformation("Created deposit product {ProductId} by user {UserId}", product.Id, createdByUserId);

        return MapToDepositProductDto(product);
    }

    public async Task<DepositProductDto> UpdateDepositProductAsync(Guid productId, UpdateDepositProductRequest request, Guid updatedByUserId)
    {
        var product = await _unitOfWork.Repository<DepositProduct>().GetByIdAsync(productId);
        if (product == null)
            throw new InvalidOperationException($"Deposit product {productId} not found");

        var originalName = product.Name;

        if (!string.IsNullOrEmpty(request.Name))
            product.Name = request.Name;
        if (!string.IsNullOrEmpty(request.Description))
            product.Description = request.Description;
        if (request.IsActive.HasValue)
            product.IsActive = request.IsActive.Value;
        if (request.BaseInterestRate.HasValue)
            product.BaseInterestRate = request.BaseInterestRate.Value;
        if (request.AllowPartialWithdrawals.HasValue)
            product.AllowPartialWithdrawals = request.AllowPartialWithdrawals.Value;
        if (request.PenaltyType.HasValue)
            product.PenaltyType = request.PenaltyType.Value;
        if (request.PenaltyAmount.HasValue)
            product.PenaltyAmount = request.PenaltyAmount.Value;
        if (request.PenaltyPercentage.HasValue)
            product.PenaltyPercentage = request.PenaltyPercentage.Value;
        if (request.DefaultMaturityAction.HasValue)
            product.DefaultMaturityAction = request.DefaultMaturityAction.Value;
        if (request.AllowAutoRenewal.HasValue)
            product.AllowAutoRenewal = request.AllowAutoRenewal.Value;
        if (request.AutoRenewalNoticeDays.HasValue)
            product.AutoRenewalNoticeDays = request.AutoRenewalNoticeDays.Value;
        if (request.PromotionalRateStartDate.HasValue)
            product.PromotionalRateStartDate = request.PromotionalRateStartDate.Value;
        if (request.PromotionalRateEndDate.HasValue)
            product.PromotionalRateEndDate = request.PromotionalRateEndDate.Value;
        if (request.PromotionalRate.HasValue)
            product.PromotionalRate = request.PromotionalRate.Value;

        _unitOfWork.Repository<DepositProduct>().Update(product);
        await _unitOfWork.SaveChangesAsync();

        await _auditLogService.LogUserActionAsync(
            updatedByUserId,
            "DepositProduct",
            "Update",
            product.Id.ToString(),
            $"Updated deposit product: {originalName} -> {product.Name}");

        _logger.LogInformation("Updated deposit product {ProductId} by user {UserId}", productId, updatedByUserId);

        return MapToDepositProductDto(product);
    }

    public async Task<bool> DeactivateDepositProductAsync(Guid productId, Guid deactivatedByUserId)
    {
        var product = await _unitOfWork.Repository<DepositProduct>().GetByIdAsync(productId);
        if (product == null)
            return false;

        product.IsActive = false;
        _unitOfWork.Repository<DepositProduct>().Update(product);
        await _unitOfWork.SaveChangesAsync();

        await _auditLogService.LogUserActionAsync(
            deactivatedByUserId,
            "DepositProduct",
            "Deactivate",
            product.Id.ToString(),
            $"Deactivated deposit product: {product.Name}");

        _logger.LogInformation("Deactivated deposit product {ProductId} by user {UserId}", productId, deactivatedByUserId);

        return true;
    }

    private static DepositProductDto MapToDepositProductDto(DepositProduct product)
    {
        return new DepositProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            ProductType = product.ProductType,
            MinimumTermDays = product.MinimumTermDays,
            MaximumTermDays = product.MaximumTermDays,
            DefaultTermDays = product.DefaultTermDays,
            MinimumBalance = product.MinimumBalance,
            MaximumBalance = product.MaximumBalance,
            MinimumOpeningBalance = product.MinimumOpeningBalance,
            BaseInterestRate = product.BaseInterestRate,
            InterestCalculationMethod = product.InterestCalculationMethod,
            CompoundingFrequency = product.CompoundingFrequency,
            HasTieredRates = product.HasTieredRates,
            AllowPartialWithdrawals = product.AllowPartialWithdrawals,
            PenaltyType = product.PenaltyType,
            PenaltyAmount = product.PenaltyAmount,
            PenaltyPercentage = product.PenaltyPercentage,
            PenaltyFreeDays = product.PenaltyFreeDays,
            DefaultMaturityAction = product.DefaultMaturityAction,
            AllowAutoRenewal = product.AllowAutoRenewal,
            AutoRenewalNoticeDays = product.AutoRenewalNoticeDays,
            PromotionalRateStartDate = product.PromotionalRateStartDate,
            PromotionalRateEndDate = product.PromotionalRateEndDate,
            PromotionalRate = product.PromotionalRate,
            IsActive = product.IsActive
        };
    }
}
