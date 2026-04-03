using Bank.Domain.Common;
using Bank.Domain.Enums;

namespace Bank.Domain.Entities;

/// <summary>
/// Represents an interest rate tier for deposit products with tiered interest rates
/// </summary>
public class InterestTier : BaseEntity
{
    public Guid DepositProductId { get; set; }
    public string TierName { get; set; } = string.Empty;
    public decimal MinimumBalance { get; set; }
    public decimal? MaximumBalance { get; set; }
    public decimal InterestRate { get; set; }
    public InterestTierBasis TierBasis { get; set; }
    public bool IsActive { get; set; } = true;
    public int DisplayOrder { get; set; }
    
    // Promotional tier settings
    public DateTime? EffectiveFromDate { get; set; }
    public DateTime? EffectiveToDate { get; set; }
    public bool IsPromotional { get; set; }
    
    // Navigation properties
    public virtual DepositProduct DepositProduct { get; set; } = null!;
    
    /// <summary>
    /// Checks if this tier is currently effective
    /// </summary>
    public bool IsEffective()
    {
        if (!IsActive) return false;
        
        var now = DateTime.UtcNow;
        
        if (EffectiveFromDate.HasValue && now < EffectiveFromDate.Value)
            return false;
            
        if (EffectiveToDate.HasValue && now > EffectiveToDate.Value)
            return false;
            
        return true;
    }
    
    /// <summary>
    /// Checks if a balance falls within this tier's range
    /// </summary>
    public bool IsBalanceInRange(decimal balance)
    {
        if (balance < MinimumBalance)
            return false;
            
        if (MaximumBalance.HasValue && balance > MaximumBalance.Value)
            return false;
            
        return true;
    }
}