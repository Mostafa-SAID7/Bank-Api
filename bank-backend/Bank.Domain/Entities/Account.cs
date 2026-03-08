using Bank.Domain.Common;
using Bank.Domain.Enums;

namespace Bank.Domain.Entities;

public class Account : BaseEntity
{
    public string AccountNumber { get; set; } = string.Empty;
    public string AccountHolderName { get; set; } = string.Empty;
    public decimal Balance { get; set; }
    
    // Existing properties
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    // Account lifecycle properties
    public AccountStatus Status { get; set; } = AccountStatus.Active;
    public AccountType Type { get; set; } = AccountType.Checking;
    public DateTime OpenedDate { get; set; } = DateTime.UtcNow;
    public DateTime? ClosedDate { get; set; }
    public string? ClosureReason { get; set; }
    
    // Dormancy tracking
    public DateTime LastActivityDate { get; set; } = DateTime.UtcNow;
    public DateTime? DormancyDate { get; set; }
    public int DormancyPeriodDays { get; set; } = 365; // Default 1 year
    
    // Fee management
    public decimal MinimumBalance { get; set; } = 0;
    public decimal MonthlyMaintenanceFee { get; set; } = 0;
    public bool FeeWaiverEligible { get; set; } = false;
    public DateTime? LastFeeCalculationDate { get; set; }
    
    // Interest calculation
    public decimal InterestRate { get; set; } = 0;
    public DateTime? LastInterestCalculationDate { get; set; }
    public InterestCompoundingFrequency CompoundingFrequency { get; set; } = InterestCompoundingFrequency.Monthly;
    
    // Joint account properties
    public bool IsJointAccount { get; set; } = false;
    public bool RequiresMultipleSignatures { get; set; } = false;
    public decimal? MultipleSignatureThreshold { get; set; } // Amount above which multiple signatures are required
    public int MinimumSignaturesRequired { get; set; } = 1;
    
    // Account restrictions and holds
    public bool HasHolds { get; set; } = false;
    public bool HasRestrictions { get; set; } = false;

    public ICollection<Transaction> SentTransactions { get; set; } = new List<Transaction>();
    public ICollection<Transaction> ReceivedTransactions { get; set; } = new List<Transaction>();
    public ICollection<AccountFee> Fees { get; set; } = new List<AccountFee>();
    public ICollection<AccountHold> Holds { get; set; } = new List<AccountHold>();
    public ICollection<AccountRestriction> Restrictions { get; set; } = new List<AccountRestriction>();
    public ICollection<AccountStatusHistory> StatusHistory { get; set; } = new List<AccountStatusHistory>();
    public ICollection<JointAccountHolder> JointHolders { get; set; } = new List<JointAccountHolder>();
    
    // Domain methods
    public bool IsDormant()
    {
        return Status == AccountStatus.Dormant || 
               (Status == AccountStatus.Active && 
                DateTime.UtcNow.Subtract(LastActivityDate).TotalDays >= DormancyPeriodDays);
    }
    
    public bool IsActive()
    {
        return Status == AccountStatus.Active && !HasHolds;
    }
    
    public bool CanDebit(decimal amount)
    {
        return IsActive() && 
               !HasRestrictions && 
               Balance >= amount &&
               !Restrictions.Any(r => r.Type == AccountRestrictionType.NoDebits && r.IsActive);
    }
    
    public bool CanCredit()
    {
        return (Status == AccountStatus.Active || Status == AccountStatus.Inactive) && 
               !Restrictions.Any(r => r.Type == AccountRestrictionType.NoCredits && r.IsActive);
    }
    
    public void UpdateActivity()
    {
        LastActivityDate = DateTime.UtcNow;
        if (Status == AccountStatus.Dormant)
        {
            Status = AccountStatus.Active;
            DormancyDate = null;
        }
    }
    
    public void MarkAsDormant()
    {
        if (Status == AccountStatus.Active)
        {
            Status = AccountStatus.Dormant;
            DormancyDate = DateTime.UtcNow;
        }
    }
    
    public bool HasJointHolder(Guid userId)
    {
        return IsJointAccount && JointHolders.Any(jh => jh.UserId == userId && jh.IsActive);
    }
    
    public bool CanUserAccess(Guid userId)
    {
        if (UserId == userId) return true; // Primary account holder
        return HasJointHolder(userId);
    }
    
    public bool RequiresMultipleSignaturesForAmount(decimal amount)
    {
        return IsJointAccount && 
               RequiresMultipleSignatures && 
               MultipleSignatureThreshold.HasValue && 
               amount >= MultipleSignatureThreshold.Value;
    }
    
    public int GetActiveJointHoldersCount()
    {
        return JointHolders.Count(jh => jh.IsActive) + 1; // +1 for primary holder
    }
}
