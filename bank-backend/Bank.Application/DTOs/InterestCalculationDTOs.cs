using Bank.Domain.Enums;

namespace Bank.Application.DTOs;

/// <summary>
/// Request to calculate interest for an account
/// </summary>
public class InterestCalculationRequest
{
    public Guid AccountId { get; set; }
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    public int? CompoundingFrequency { get; set; } // Optional override
}

/// <summary>
/// Result of interest calculation
/// </summary>
public class InterestCalculationResult
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public decimal InterestAmount { get; set; }
    public decimal PrincipalAmount { get; set; }
    public decimal InterestRate { get; set; }
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    public int DaysCalculated { get; set; }
    public InterestCompoundingFrequency CompoundingFrequency { get; set; }
}

/// <summary>
/// Request to apply interest to an account
/// </summary>
public class ApplyInterestRequest
{
    public Guid AccountId { get; set; }
    public Guid UserId { get; set; }
}

/// <summary>
/// Request to update interest rate for an account
/// </summary>
public class UpdateInterestRateRequest
{
    public Guid AccountId { get; set; }
    public decimal NewInterestRate { get; set; }
    public Guid UserId { get; set; }
}

/// <summary>
/// Interest rate information for different account types and balances
/// </summary>
public class InterestRateInfo
{
    public AccountType AccountType { get; set; }
    public decimal MinimumBalance { get; set; }
    public decimal MaximumBalance { get; set; }
    public decimal InterestRate { get; set; }
}

/// <summary>
/// Monthly interest processing summary
/// </summary>
public class MonthlyInterestProcessingSummary
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public int TotalAccountsProcessed { get; set; }
    public int SuccessfulApplications { get; set; }
    public int FailedApplications { get; set; }
    public decimal TotalInterestApplied { get; set; }
    public DateTime ProcessingDate { get; set; }
}