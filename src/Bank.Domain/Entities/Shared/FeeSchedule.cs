using Bank.Domain.Common;
using Bank.Domain.Enums;
using AccountEntity = Bank.Domain.Entities.Account;

namespace Bank.Domain.Entities;

public class FeeSchedule : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public FeeType Type { get; set; }
    public AccountType? AccountType { get; set; } // Null means applies to all account types
    
    public decimal Amount { get; set; }
    public FeeFrequency Frequency { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime EffectiveDate { get; set; } = DateTime.UtcNow;
    public DateTime? ExpiryDate { get; set; }
    
    // Conditions for fee application
    public decimal? MinimumBalanceThreshold { get; set; }
    public decimal? MaximumBalanceThreshold { get; set; }
    public int? DormancyDaysThreshold { get; set; }
    public int? TransactionCountThreshold { get; set; }
    
    // Waiver conditions
    public decimal? WaiverMinimumBalance { get; set; }
    public bool WaiverForPremiumAccounts { get; set; } = false;
    public string? WaiverConditions { get; set; }
    
    public Guid CreatedByUserId { get; set; }
    public User CreatedByUser { get; set; } = null!;
    
    public bool IsApplicable(AccountEntity account)
    {
        if (!IsActive || DateTime.UtcNow < EffectiveDate || (ExpiryDate.HasValue && DateTime.UtcNow > ExpiryDate))
            return false;
            
        if (AccountType.HasValue && account.Type != AccountType.Value)
            return false;
            
        if (MinimumBalanceThreshold.HasValue && account.Balance < MinimumBalanceThreshold.Value)
            return false;
            
        if (MaximumBalanceThreshold.HasValue && account.Balance > MaximumBalanceThreshold.Value)
            return false;
            
        if (DormancyDaysThreshold.HasValue && Type == FeeType.DormancyFee)
        {
            var daysSinceActivity = (DateTime.UtcNow - account.LastActivityDate).TotalDays;
            if (daysSinceActivity < DormancyDaysThreshold.Value)
                return false;
        }
        
        return true;
    }
    
    public bool IsWaiverEligible(AccountEntity account)
    {
        if (WaiverMinimumBalance.HasValue && account.Balance >= WaiverMinimumBalance.Value)
            return true;
            
        if (WaiverForPremiumAccounts && account.Type == Enums.AccountType.Premium)
            return true;
            
        if (account.FeeWaiverEligible)
            return true;
            
        return false;
    }
}
