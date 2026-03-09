using Bank.Domain.Common;
using Bank.Domain.Enums;

namespace Bank.Domain.Entities;

/// <summary>
/// Represents a fixed deposit account with term locking and maturity handling
/// </summary>
public class FixedDeposit : BaseEntity
{
    public string DepositNumber { get; set; } = string.Empty;
    public Guid CustomerId { get; set; }
    public Guid DepositProductId { get; set; }
    public Guid LinkedAccountId { get; set; }
    
    // Deposit details
    public decimal PrincipalAmount { get; set; }
    public decimal InterestRate { get; set; }
    public int TermDays { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime MaturityDate { get; set; }
    public FixedDepositStatus Status { get; set; }
    
    // Interest calculation
    public InterestCalculationMethod InterestCalculationMethod { get; set; }
    public InterestCompoundingFrequency CompoundingFrequency { get; set; }
    public decimal AccruedInterest { get; set; }
    public DateTime LastInterestCalculationDate { get; set; }
    
    // Maturity settings
    public MaturityAction MaturityAction { get; set; }
    public bool AutoRenewalEnabled { get; set; }
    public int? RenewalTermDays { get; set; }
    public DateTime? RenewalNoticeDate { get; set; }
    public bool CustomerConsentReceived { get; set; }
    
    // Penalty settings
    public WithdrawalPenaltyType PenaltyType { get; set; }
    public decimal? PenaltyAmount { get; set; }
    public decimal? PenaltyPercentage { get; set; }
    
    // Closure details
    public DateTime? ClosureDate { get; set; }
    public Guid? ClosedByUserId { get; set; }
    public string? ClosureReason { get; set; }
    public decimal? PenaltyApplied { get; set; }
    public decimal? NetAmountPaid { get; set; }
    
    // Renewal tracking
    public Guid? RenewedFromDepositId { get; set; }
    public Guid? RenewedToDepositId { get; set; }
    public int RenewalCount { get; set; }
    
    // Navigation properties
    public virtual User Customer { get; set; } = null!;
    public virtual DepositProduct DepositProduct { get; set; } = null!;
    public virtual Account LinkedAccount { get; set; } = null!;
    public virtual User? ClosedByUser { get; set; }
    public virtual FixedDeposit? RenewedFromDeposit { get; set; }
    public virtual FixedDeposit? RenewedToDeposit { get; set; }
    public virtual ICollection<DepositTransaction> Transactions { get; set; } = new List<DepositTransaction>();
    public virtual ICollection<DepositCertificate> Certificates { get; set; } = new List<DepositCertificate>();
    public virtual ICollection<MaturityNotice> MaturityNotices { get; set; } = new List<MaturityNotice>();
    
    /// <summary>
    /// Calculates the maturity amount including principal and interest
    /// </summary>
    public decimal CalculateMaturityAmount()
    {
        return PrincipalAmount + CalculateInterestAtMaturity();
    }
    
    /// <summary>
    /// Calculates the total interest at maturity
    /// </summary>
    public decimal CalculateInterestAtMaturity()
    {
        var principal = PrincipalAmount;
        var rate = InterestRate / 100;
        var termYears = TermDays / 365.0m;
        
        return InterestCalculationMethod switch
        {
            InterestCalculationMethod.Simple => principal * rate * termYears,
            InterestCalculationMethod.CompoundDaily => CalculateCompoundInterest(365),
            InterestCalculationMethod.CompoundMonthly => CalculateCompoundInterest(12),
            _ => principal * rate * termYears
        };
    }
    
    /// <summary>
    /// Calculates compound interest based on compounding frequency
    /// </summary>
    private decimal CalculateCompoundInterest(int compoundingPerYear)
    {
        var principal = PrincipalAmount;
        var rate = InterestRate / 100;
        var termYears = TermDays / 365.0m;
        
        var amount = principal * (decimal)Math.Pow((double)(1 + rate / compoundingPerYear), (double)(compoundingPerYear * termYears));
        return amount - principal;
    }
    
    /// <summary>
    /// Calculates early withdrawal penalty
    /// </summary>
    public decimal CalculateEarlyWithdrawalPenalty(decimal withdrawalAmount)
    {
        if (Status != FixedDepositStatus.Active)
            return 0;
            
        return PenaltyType switch
        {
            WithdrawalPenaltyType.None => 0,
            WithdrawalPenaltyType.FixedAmount => PenaltyAmount ?? 0,
            WithdrawalPenaltyType.Percentage => withdrawalAmount * (PenaltyPercentage ?? 0) / 100,
            WithdrawalPenaltyType.InterestForfeiture => AccruedInterest,
            WithdrawalPenaltyType.Combined => (PenaltyAmount ?? 0) + (withdrawalAmount * (PenaltyPercentage ?? 0) / 100),
            _ => 0
        };
    }
    
    /// <summary>
    /// Checks if the deposit has matured
    /// </summary>
    public bool HasMatured()
    {
        return DateTime.UtcNow >= MaturityDate && Status == FixedDepositStatus.Active;
    }
    
    /// <summary>
    /// Checks if renewal notice should be sent
    /// </summary>
    public bool ShouldSendRenewalNotice()
    {
        if (!AutoRenewalEnabled || RenewalNoticeDate.HasValue)
            return false;
            
        var noticeDays = DepositProduct?.AutoRenewalNoticeDays ?? 30;
        var noticeDate = MaturityDate.AddDays(-noticeDays);
        
        return DateTime.UtcNow >= noticeDate;
    }
    
    /// <summary>
    /// Generates a unique deposit number
    /// </summary>
    public void GenerateDepositNumber()
    {
        var timestamp = DateTime.UtcNow.ToString("yyyyMMdd");
        var random = new Random().Next(1000, 9999);
        DepositNumber = $"FD{timestamp}{random}";
    }
}