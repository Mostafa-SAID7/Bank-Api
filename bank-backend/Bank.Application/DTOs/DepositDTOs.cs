using Bank.Domain.Enums;

namespace Bank.Application.DTOs;

// Deposit Product DTOs
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

// Interest Tier DTOs
public class InterestTierDto
{
    public Guid Id { get; set; }
    public Guid DepositProductId { get; set; }
    public string TierName { get; set; } = string.Empty;
    public decimal MinimumBalance { get; set; }
    public decimal? MaximumBalance { get; set; }
    public decimal InterestRate { get; set; }
    public InterestTierBasis TierBasis { get; set; }
    public bool IsActive { get; set; }
    public int DisplayOrder { get; set; }
    
    public DateTime? EffectiveFromDate { get; set; }
    public DateTime? EffectiveToDate { get; set; }
    public bool IsPromotional { get; set; }
    public bool IsEffective { get; set; }
}

public class CreateInterestTierRequest
{
    public string TierName { get; set; } = string.Empty;
    public decimal MinimumBalance { get; set; }
    public decimal? MaximumBalance { get; set; }
    public decimal InterestRate { get; set; }
    public InterestTierBasis TierBasis { get; set; }
    public int DisplayOrder { get; set; }
    
    public DateTime? EffectiveFromDate { get; set; }
    public DateTime? EffectiveToDate { get; set; }
    public bool IsPromotional { get; set; }
}

public class UpdateInterestTierRequest
{
    public string? TierName { get; set; }
    public decimal? InterestRate { get; set; }
    public bool? IsActive { get; set; }
    public int? DisplayOrder { get; set; }
    
    public DateTime? EffectiveFromDate { get; set; }
    public DateTime? EffectiveToDate { get; set; }
}

// Fixed Deposit DTOs
public class FixedDepositDto
{
    public Guid Id { get; set; }
    public string DepositNumber { get; set; } = string.Empty;
    public Guid CustomerId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public Guid DepositProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public Guid LinkedAccountId { get; set; }
    public string LinkedAccountNumber { get; set; } = string.Empty;
    
    public decimal PrincipalAmount { get; set; }
    public decimal InterestRate { get; set; }
    public int TermDays { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime MaturityDate { get; set; }
    public FixedDepositStatus Status { get; set; }
    
    public InterestCalculationMethod InterestCalculationMethod { get; set; }
    public InterestCompoundingFrequency CompoundingFrequency { get; set; }
    public decimal AccruedInterest { get; set; }
    public DateTime LastInterestCalculationDate { get; set; }
    
    public MaturityAction MaturityAction { get; set; }
    public bool AutoRenewalEnabled { get; set; }
    public int? RenewalTermDays { get; set; }
    public DateTime? RenewalNoticeDate { get; set; }
    public bool CustomerConsentReceived { get; set; }
    
    public WithdrawalPenaltyType PenaltyType { get; set; }
    public decimal? PenaltyAmount { get; set; }
    public decimal? PenaltyPercentage { get; set; }
    
    public DateTime? ClosureDate { get; set; }
    public string? ClosureReason { get; set; }
    public decimal? PenaltyApplied { get; set; }
    public decimal? NetAmountPaid { get; set; }
    
    public int RenewalCount { get; set; }
    public decimal MaturityAmount { get; set; }
    public decimal InterestAtMaturity { get; set; }
    public int DaysToMaturity { get; set; }
    public bool HasMatured { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class CreateFixedDepositRequest
{
    public Guid DepositProductId { get; set; }
    public Guid LinkedAccountId { get; set; }
    public decimal PrincipalAmount { get; set; }
    public int? TermDays { get; set; } // Optional, will use product default if not provided
    public MaturityAction? MaturityAction { get; set; } // Optional, will use product default
    public bool? AutoRenewalEnabled { get; set; }
    public int? RenewalTermDays { get; set; }
}

public class RenewDepositRequest
{
    public int? TermDays { get; set; }
    public decimal? InterestRate { get; set; }
    public MaturityAction? MaturityAction { get; set; }
    public bool? AutoRenewalEnabled { get; set; }
}

// Withdrawal DTOs
public class WithdrawalDetailsDto
{
    public Guid DepositId { get; set; }
    public decimal RequestedAmount { get; set; }
    public decimal AvailableBalance { get; set; }
    public decimal PenaltyAmount { get; set; }
    public decimal NetAmount { get; set; }
    public WithdrawalPenaltyType PenaltyType { get; set; }
    public string PenaltyDescription { get; set; } = string.Empty;
    public bool IsEarlyWithdrawal { get; set; }
    public int DaysBeforeMaturity { get; set; }
}

public class EarlyWithdrawalRequest
{
    public decimal WithdrawalAmount { get; set; }
    public string Reason { get; set; } = string.Empty;
    public bool AcknowledgePenalty { get; set; }
}

public class PartialWithdrawalRequest
{
    public decimal WithdrawalAmount { get; set; }
    public string Reason { get; set; } = string.Empty;
}

// Maturity DTOs
public class MaturityDetailsDto
{
    public Guid DepositId { get; set; }
    public DateTime MaturityDate { get; set; }
    public decimal PrincipalAmount { get; set; }
    public decimal AccruedInterest { get; set; }
    public decimal MaturityAmount { get; set; }
    public MaturityAction DefaultAction { get; set; }
    public bool AutoRenewalEnabled { get; set; }
    public int? RenewalTermDays { get; set; }
    public bool CustomerConsentReceived { get; set; }
    public List<MaturityActionOption> AvailableActions { get; set; } = new();
}

public class MaturityActionOption
{
    public MaturityAction Action { get; set; }
    public string Description { get; set; } = string.Empty;
    public bool RequiresCustomerConsent { get; set; }
}

// Certificate DTOs
public class DepositCertificateDto
{
    public Guid Id { get; set; }
    public Guid FixedDepositId { get; set; }
    public string CertificateNumber { get; set; } = string.Empty;
    public DepositCertificateStatus Status { get; set; }
    public DateTime IssueDate { get; set; }
    public DateTime? DeliveryDate { get; set; }
    public string DeliveryMethod { get; set; } = string.Empty;
    public string? DeliveryAddress { get; set; }
    public string? DeliveryReference { get; set; }
    public string? PdfFileName { get; set; }
    public bool HasPdf { get; set; }
}

// Notice DTOs
public class MaturityNoticeDto
{
    public Guid Id { get; set; }
    public Guid FixedDepositId { get; set; }
    public string NoticeNumber { get; set; } = string.Empty;
    public MaturityNoticeType NoticeType { get; set; }
    public DateTime NoticeDate { get; set; }
    public DateTime MaturityDate { get; set; }
    public NotificationStatus Status { get; set; }
    
    public string Subject { get; set; } = string.Empty;
    public NotificationChannel DeliveryChannel { get; set; }
    public string? DeliveryAddress { get; set; }
    public DateTime? DeliveryDate { get; set; }
    public int DeliveryAttempts { get; set; }
    
    public DateTime? CustomerResponseDate { get; set; }
    public MaturityAction? CustomerChoice { get; set; }
    public string? CustomerInstructions { get; set; }
    public bool ConsentReceived { get; set; }
}

// Transaction DTOs
public class DepositTransactionDto
{
    public Guid Id { get; set; }
    public Guid FixedDepositId { get; set; }
    public string TransactionReference { get; set; } = string.Empty;
    public DepositTransactionType TransactionType { get; set; }
    public decimal Amount { get; set; }
    public string Description { get; set; } = string.Empty;
    public DateTime TransactionDate { get; set; }
    public TransactionStatus Status { get; set; }
    
    public DateTime? InterestPeriodStart { get; set; }
    public DateTime? InterestPeriodEnd { get; set; }
    public decimal? InterestRate { get; set; }
    public int? InterestDays { get; set; }
    
    public WithdrawalPenaltyType? PenaltyType { get; set; }
    public string? PenaltyReason { get; set; }
}

// Summary and Portfolio DTOs
public class DepositSummaryDto
{
    public Guid CustomerId { get; set; }
    public int TotalDeposits { get; set; }
    public decimal TotalPrincipal { get; set; }
    public decimal TotalAccruedInterest { get; set; }
    public decimal TotalMaturityValue { get; set; }
    public int ActiveDeposits { get; set; }
    public int MaturingThisMonth { get; set; }
    public decimal AverageInterestRate { get; set; }
}

public class DepositPortfolioDto
{
    public Guid CustomerId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public DepositSummaryDto Summary { get; set; } = new();
    public List<FixedDepositDto> ActiveDeposits { get; set; } = new();
    public List<FixedDepositDto> MaturingDeposits { get; set; } = new();
    public List<DepositTransactionDto> RecentTransactions { get; set; } = new();
}
// Enhanced Withdrawal DTOs
public class DetailedWithdrawalCalculation
{
    public Guid DepositId { get; set; }
    public string DepositNumber { get; set; } = string.Empty;
    public decimal RequestedAmount { get; set; }
    public decimal PrincipalAmount { get; set; }
    public decimal AccruedInterest { get; set; }
    public decimal AvailableBalance { get; set; }
    public DateTime MaturityDate { get; set; }
    public int DaysToMaturity { get; set; }
    public bool IsEarlyWithdrawal { get; set; }
    
    public List<PenaltyDetail> PenaltyDetails { get; set; } = new();
    public decimal TotalPenalty { get; set; }
    public decimal NetAmount { get; set; }
    public decimal RemainingBalance { get; set; }
    
    public WithdrawalValidationResult ValidationResults { get; set; } = new();
}

public class PenaltyDetail
{
    public string Type { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Description { get; set; } = string.Empty;
}

public class WithdrawalValidationResult
{
    public bool IsValid { get; set; }
    public List<string> Errors { get; set; } = new();
}

public class WithdrawalResult
{
    public Guid DepositId { get; set; }
    public bool Success { get; set; }
    public decimal NetAmountPaid { get; set; }
    public decimal PenaltyApplied { get; set; }
    public string TransactionReference { get; set; } = string.Empty;
    public List<string> Errors { get; set; } = new();
}

public class PenaltyFreePeriodsDto
{
    public Guid DepositId { get; set; }
    public bool HasPenaltyFreePeriods { get; set; }
    public int PenaltyFreeDays { get; set; }
    public DateTime? PenaltyFreeUntil { get; set; }
    public bool IsCurrentlyPenaltyFree { get; set; }
    public int? DaysUntilPenaltyFree { get; set; }
}

public class WithdrawalHistoryDto
{
    public Guid TransactionId { get; set; }
    public string TransactionReference { get; set; } = string.Empty;
    public DepositTransactionType WithdrawalType { get; set; }
    public decimal Amount { get; set; }
    public decimal PenaltyApplied { get; set; }
    public DateTime TransactionDate { get; set; }
    public string Reason { get; set; } = string.Empty;
    public Guid? ProcessedBy { get; set; }
}

// Enhanced Maturity DTOs
public class MaturityReminderDto
{
    public Guid DepositId { get; set; }
    public string DepositNumber { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public string CustomerEmail { get; set; } = string.Empty;
    public DateTime MaturityDate { get; set; }
    public int DaysToMaturity { get; set; }
    public decimal MaturityAmount { get; set; }
    public bool AutoRenewalEnabled { get; set; }
    public bool CustomerConsentReceived { get; set; }
    public MaturityAction DefaultAction { get; set; }
    public List<MaturityNoticeDto> SentNotices { get; set; } = new();
}

public class CustomerConsentRequest
{
    public bool ConsentGiven { get; set; }
    public MaturityAction? PreferredAction { get; set; }
    public int? PreferredRenewalTerm { get; set; }
    public string? CustomerNotes { get; set; }
}

public class AutoRenewalSummaryDto
{
    public int TotalEligible { get; set; }
    public int SuccessfulRenewals { get; set; }
    public int FailedRenewals { get; set; }
    public int PendingConsent { get; set; }
    public decimal TotalRenewedAmount { get; set; }
    public List<string> Errors { get; set; } = new();
}