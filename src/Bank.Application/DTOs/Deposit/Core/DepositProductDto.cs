using Bank.Domain.Enums;

namespace Bank.Application.DTOs.Deposit.Core;

/// <summary>
/// Deposit product data transfer object
/// </summary>
public class DepositProductDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DepositProductType ProductType { get; set; }
    public bool IsActive { get; set; }
    
    public int? MinimumTermDays { get; set; }
    public int? MaximumTermDays { get; set; }
    public int? DefaultTermDays { get; set; }
    
    public decimal MinimumBalance { get; set; }
    public decimal? MaximumBalance { get; set; }
    public decimal MinimumOpeningBalance { get; set; }
    
    public decimal BaseInterestRate { get; set; }
    public InterestCalculationMethod InterestCalculationMethod { get; set; }
    public InterestCompoundingFrequency CompoundingFrequency { get; set; }
    public bool HasTieredRates { get; set; }
    
    public bool AllowPartialWithdrawals { get; set; }
    public WithdrawalPenaltyType PenaltyType { get; set; }
    public decimal? PenaltyAmount { get; set; }
    public decimal? PenaltyPercentage { get; set; }
    public int? PenaltyFreeDays { get; set; }
    
    public MaturityAction DefaultMaturityAction { get; set; }
    public bool AllowAutoRenewal { get; set; }
    public int? AutoRenewalNoticeDays { get; set; }
    
    public DateTime? PromotionalRateStartDate { get; set; }
    public DateTime? PromotionalRateEndDate { get; set; }
    public decimal? PromotionalRate { get; set; }
    public bool IsPromotionalRateActive { get; set; }
    
    public List<InterestTierDto> InterestTiers { get; set; } = new();
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

/// <summary>
/// Request to create a new deposit product
/// </summary>
public class CreateDepositProductRequest
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DepositProductType ProductType { get; set; }
    
    public int? MinimumTermDays { get; set; }
    public int? MaximumTermDays { get; set; }
    public int? DefaultTermDays { get; set; }
    
    public decimal MinimumBalance { get; set; }
    public decimal? MaximumBalance { get; set; }
    public decimal MinimumOpeningBalance { get; set; }
    
    public decimal BaseInterestRate { get; set; }
    public InterestCalculationMethod InterestCalculationMethod { get; set; }
    public InterestCompoundingFrequency CompoundingFrequency { get; set; }
    public bool HasTieredRates { get; set; }
    
    public bool AllowPartialWithdrawals { get; set; }
    public WithdrawalPenaltyType PenaltyType { get; set; }
    public decimal? PenaltyAmount { get; set; }
    public decimal? PenaltyPercentage { get; set; }
    public int? PenaltyFreeDays { get; set; }
    
    public MaturityAction DefaultMaturityAction { get; set; }
    public bool AllowAutoRenewal { get; set; }
    public int? AutoRenewalNoticeDays { get; set; }
    
    public DateTime? PromotionalRateStartDate { get; set; }
    public DateTime? PromotionalRateEndDate { get; set; }
    public decimal? PromotionalRate { get; set; }
}

/// <summary>
/// Request to update a deposit product
/// </summary>
public class UpdateDepositProductRequest
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public bool? IsActive { get; set; }
    
    public decimal? BaseInterestRate { get; set; }
    public bool? AllowPartialWithdrawals { get; set; }
    public WithdrawalPenaltyType? PenaltyType { get; set; }
    public decimal? PenaltyAmount { get; set; }
    public decimal? PenaltyPercentage { get; set; }
    
    public MaturityAction? DefaultMaturityAction { get; set; }
    public bool? AllowAutoRenewal { get; set; }
    public int? AutoRenewalNoticeDays { get; set; }
    
    public DateTime? PromotionalRateStartDate { get; set; }
    public DateTime? PromotionalRateEndDate { get; set; }
    public decimal? PromotionalRate { get; set; }
}


