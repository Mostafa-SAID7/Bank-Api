using Bank.Domain.Enums;

namespace Bank.Application.DTOs;

/// <summary>
/// Request for external account validation
/// </summary>
public class ExternalAccountValidationRequest
{
    public string AccountNumber { get; set; } = string.Empty;
    public string BankCode { get; set; } = string.Empty;
    public string? SwiftCode { get; set; }
    public string? IbanNumber { get; set; }
    public string? RoutingNumber { get; set; }
    public string CountryCode { get; set; } = "US";
    public BeneficiaryType BeneficiaryType { get; set; }
}

/// <summary>
/// Result of account validation
/// </summary>
public class AccountValidationResult
{
    public bool IsValid { get; set; }
    public string? AccountHolderName { get; set; }
    public string? BankName { get; set; }
    public string? BankAddress { get; set; }
    public List<string> ValidationErrors { get; set; } = new();
    public List<string> Warnings { get; set; } = new();
    public DateTime ValidationDate { get; set; } = DateTime.UtcNow;
    public string? ValidationReference { get; set; }
}

/// <summary>
/// SWIFT code validation result
/// </summary>
public class SwiftValidationResult
{
    public bool IsValid { get; set; }
    public string? BankName { get; set; }
    public string? BankCode { get; set; }
    public string? CountryCode { get; set; }
    public string? LocationCode { get; set; }
    public string? BranchCode { get; set; }
    public string? BankAddress { get; set; }
    public bool IsActive { get; set; }
    public List<string> ValidationErrors { get; set; } = new();
}

/// <summary>
/// IBAN validation result
/// </summary>
public class IbanValidationResult
{
    public bool IsValid { get; set; }
    public string? CountryCode { get; set; }
    public string? CheckDigits { get; set; }
    public string? BankCode { get; set; }
    public string? AccountNumber { get; set; }
    public int IbanLength { get; set; }
    public bool ChecksumValid { get; set; }
    public List<string> ValidationErrors { get; set; } = new();
}

/// <summary>
/// Routing number validation result
/// </summary>
public class RoutingNumberValidationResult
{
    public bool IsValid { get; set; }
    public string? BankName { get; set; }
    public string? BankAddress { get; set; }
    public string? FedwireParticipant { get; set; }
    public string? ACHParticipant { get; set; }
    public List<string> ValidationErrors { get; set; } = new();
}

/// <summary>
/// Bank information result
/// </summary>
public class BankInformationResult
{
    public bool Found { get; set; }
    public string? BankName { get; set; }
    public string? BankCode { get; set; }
    public string? SwiftCode { get; set; }
    public string? CountryCode { get; set; }
    public string? CountryName { get; set; }
    public string? City { get; set; }
    public string? Address { get; set; }
    public List<string> SupportedCurrencies { get; set; } = new();
    public List<string> SupportedServices { get; set; } = new();
}

/// <summary>
/// Account number format validation result
/// </summary>
public class AccountNumberValidationResult
{
    public bool IsValid { get; set; }
    public string? FormattedAccountNumber { get; set; }
    public int ExpectedLength { get; set; }
    public int ActualLength { get; set; }
    public string? AccountType { get; set; }
    public List<string> ValidationErrors { get; set; } = new();
}

/// <summary>
/// Request for comprehensive beneficiary account validation
/// </summary>
public class BeneficiaryAccountValidationRequest
{
    public string BeneficiaryName { get; set; } = string.Empty;
    public string AccountNumber { get; set; } = string.Empty;
    public string BankCode { get; set; } = string.Empty;
    public string? SwiftCode { get; set; }
    public string? IbanNumber { get; set; }
    public string? RoutingNumber { get; set; }
    public string CountryCode { get; set; } = "US";
    public BeneficiaryType BeneficiaryType { get; set; }
    public bool PerformNameMatching { get; set; } = true;
    public bool CheckSanctionsList { get; set; } = true;
}

/// <summary>
/// Comprehensive validation result
/// </summary>
public class ComprehensiveValidationResult
{
    public bool IsValid { get; set; }
    public bool AccountExists { get; set; }
    public bool NameMatches { get; set; }
    public bool PassesSanctionsCheck { get; set; }
    public string? MatchedAccountHolderName { get; set; }
    public string? BankName { get; set; }
    public AccountValidationResult? AccountValidation { get; set; }
    public SwiftValidationResult? SwiftValidation { get; set; }
    public IbanValidationResult? IbanValidation { get; set; }
    public RoutingNumberValidationResult? RoutingValidation { get; set; }
    public List<string> ValidationErrors { get; set; } = new();
    public List<string> Warnings { get; set; } = new();
    public string ValidationSummary { get; set; } = string.Empty;
    public DateTime ValidationDate { get; set; } = DateTime.UtcNow;
    public string? ValidationReference { get; set; }
}

/// <summary>
/// Transfer eligibility check request
/// </summary>
public class TransferEligibilityRequest
{
    public Guid BeneficiaryId { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "USD";
    public DateTime ProposedTransferDate { get; set; } = DateTime.UtcNow;
}

/// <summary>
/// Transfer eligibility result
/// </summary>
public class TransferEligibilityResult
{
    public bool IsEligible { get; set; }
    public bool BeneficiaryActive { get; set; }
    public bool BeneficiaryVerified { get; set; }
    public bool WithinTransferLimits { get; set; }
    public bool WithinDailyLimits { get; set; }
    public bool WithinMonthlyLimits { get; set; }
    public decimal? RemainingDailyLimit { get; set; }
    public decimal? RemainingMonthlyLimit { get; set; }
    public List<string> EligibilityIssues { get; set; } = new();
    public string? RecommendedAction { get; set; }
}

/// <summary>
/// Beneficiary limits result
/// </summary>
public class BeneficiaryLimitsResult
{
    public Guid BeneficiaryId { get; set; }
    public decimal? DailyLimit { get; set; }
    public decimal? MonthlyLimit { get; set; }
    public decimal? SingleTransferLimit { get; set; }
    public decimal DailyUsed { get; set; }
    public decimal MonthlyUsed { get; set; }
    public decimal? RemainingDaily { get; set; }
    public decimal? RemainingMonthly { get; set; }
    public DateTime CalculationDate { get; set; } = DateTime.UtcNow;
}

/// <summary>
/// Limit check result
/// </summary>
public class LimitCheckResult
{
    public bool WithinLimit { get; set; }
    public decimal? Limit { get; set; }
    public decimal CurrentUsage { get; set; }
    public decimal RequestedAmount { get; set; }
    public decimal? RemainingLimit { get; set; }
    public string LimitType { get; set; } = string.Empty; // "Daily", "Monthly", "Single"
    public List<string> LimitIssues { get; set; } = new();
}

/// <summary>
/// Transfer history summary for limit calculations
/// </summary>
public class TransferHistorySummary
{
    public Guid BeneficiaryId { get; set; }
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    public int TransactionCount { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal LargestTransfer { get; set; }
    public DateTime? LastTransferDate { get; set; }
    public List<DailyTransferSummary> DailySummaries { get; set; } = new();
}

/// <summary>
/// Daily transfer summary
/// </summary>
public class DailyTransferSummary
{
    public DateTime Date { get; set; }
    public int TransactionCount { get; set; }
    public decimal TotalAmount { get; set; }
}

/// <summary>
/// Transfer pre-validation request
/// </summary>
public class TransferPreValidationRequest
{
    public Guid FromAccountId { get; set; }
    public Guid BeneficiaryId { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "USD";
    public DateTime ProposedTransferDate { get; set; } = DateTime.UtcNow;
    public string? Purpose { get; set; }
    public bool CheckAccountBalance { get; set; } = true;
    public bool CheckBeneficiaryStatus { get; set; } = true;
    public bool CheckTransferLimits { get; set; } = true;
    public bool CheckFraudRules { get; set; } = true;
}

/// <summary>
/// Transfer pre-validation result
/// </summary>
public class TransferPreValidationResult
{
    public bool IsValid { get; set; }
    public bool SufficientBalance { get; set; }
    public bool BeneficiaryEligible { get; set; }
    public bool WithinLimits { get; set; }
    public bool PassesFraudCheck { get; set; }
    public decimal AvailableBalance { get; set; }
    public TransferEligibilityResult? EligibilityResult { get; set; }
    public BeneficiaryLimitsResult? LimitsResult { get; set; }
    public List<string> ValidationErrors { get; set; } = new();
    public List<string> Warnings { get; set; } = new();
    public string? RecommendedAction { get; set; }
    public DateTime ValidationDate { get; set; } = DateTime.UtcNow;
}