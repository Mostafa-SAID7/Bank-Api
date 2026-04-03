using Bank.Domain.Common;
using Bank.Domain.Enums;

namespace Bank.Domain.Entities;

/// <summary>
/// Represents a deposit product offering with configurable terms and interest rates
/// </summary>
public class DepositProduct : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DepositProductType ProductType { get; set; }
    public bool IsActive { get; set; } = true;
    
    // Term configuration
    public int? MinimumTermDays { get; set; }
    public int? MaximumTermDays { get; set; }
    public int? DefaultTermDays { get; set; }
    
    // Balance requirements
    public decimal MinimumBalance { get; set; }
    public decimal? MaximumBalance { get; set; }
    public decimal MinimumOpeningBalance { get; set; }
    
    // Interest configuration
    public decimal BaseInterestRate { get; set; }
    public InterestCalculationMethod InterestCalculationMethod { get; set; }
    public InterestCompoundingFrequency CompoundingFrequency { get; set; }
    public bool HasTieredRates { get; set; }
    
    // Withdrawal and penalty settings
    public bool AllowPartialWithdrawals { get; set; }
    public WithdrawalPenaltyType PenaltyType { get; set; }
    public decimal? PenaltyAmount { get; set; }
    public decimal? PenaltyPercentage { get; set; }
    public int? PenaltyFreeDays { get; set; }
    
    // Maturity settings (for fixed deposits)
    public MaturityAction DefaultMaturityAction { get; set; }
    public bool AllowAutoRenewal { get; set; }
    public int? AutoRenewalNoticeDays { get; set; }
    
    // Promotional settings
    public DateTime? PromotionalRateStartDate { get; set; }
    public DateTime? PromotionalRateEndDate { get; set; }
    public decimal? PromotionalRate { get; set; }
    
    // Navigation properties
    public virtual ICollection<InterestTier> InterestTiers { get; set; } = new List<InterestTier>();
    public virtual ICollection<FixedDeposit> FixedDeposits { get; set; } = new List<FixedDeposit>();
    
    /// <summary>
    /// Gets the applicable interest rate for a given balance and term
    /// </summary>
    public decimal GetApplicableRate(decimal balance, int? termDays = null)
    {
        // Check for promotional rate first
        if (IsPromotionalRateActive())
        {
            return PromotionalRate ?? BaseInterestRate;
        }
        
        // If tiered rates are enabled, find the applicable tier
        if (HasTieredRates && InterestTiers.Any())
        {
            var applicableTier = InterestTiers
                .Where(t => t.IsActive && balance >= t.MinimumBalance && 
                           (t.MaximumBalance == null || balance <= t.MaximumBalance))
                .OrderByDescending(t => t.MinimumBalance)
                .FirstOrDefault();
                
            if (applicableTier != null)
            {
                return applicableTier.InterestRate;
            }
        }
        
        return BaseInterestRate;
    }
    
    /// <summary>
    /// Checks if promotional rate is currently active
    /// </summary>
    public bool IsPromotionalRateActive()
    {
        var now = DateTime.UtcNow;
        return PromotionalRate.HasValue &&
               PromotionalRateStartDate.HasValue &&
               PromotionalRateEndDate.HasValue &&
               now >= PromotionalRateStartDate.Value &&
               now <= PromotionalRateEndDate.Value;
    }
    
    /// <summary>
    /// Validates if the term is within allowed limits
    /// </summary>
    public bool IsValidTerm(int termDays)
    {
        if (MinimumTermDays.HasValue && termDays < MinimumTermDays.Value)
            return false;
            
        if (MaximumTermDays.HasValue && termDays > MaximumTermDays.Value)
            return false;
            
        return true;
    }
    
    /// <summary>
    /// Validates if the balance meets requirements
    /// </summary>
    public bool IsValidBalance(decimal balance)
    {
        if (balance < MinimumBalance)
            return false;
            
        if (MaximumBalance.HasValue && balance > MaximumBalance.Value)
            return false;
            
        return true;
    }
}