using Bank.Domain.Enums;

namespace Bank.Application.Interfaces;

/// <summary>
/// Interface for financial calculation services
/// </summary>
public interface ICalculationService
{
    /// <summary>
    /// Calculates compound interest
    /// </summary>
    decimal CalculateCompoundInterest(decimal principal, decimal rate, int compoundingFrequency, decimal time);

    /// <summary>
    /// Calculates simple interest
    /// </summary>
    decimal CalculateSimpleInterest(decimal principal, decimal rate, decimal time);

    /// <summary>
    /// Calculates interest rate from credit score
    /// </summary>
    decimal CalculateInterestRateFromScore(int creditScore, decimal baseRate = 0.03m, decimal maxRate = 0.25m);

    /// <summary>
    /// Calculates maximum loan amount
    /// </summary>
    decimal CalculateMaxLoanAmount(decimal monthlyIncome, decimal existingDebt, 
        decimal maxDebtToIncomeRatio, int loanTermMonths, decimal interestRate);

    /// <summary>
    /// Calculates loan payment allocation
    /// </summary>
    (decimal PrincipalPayment, decimal InterestPayment) CalculatePaymentAllocation(
        decimal remainingBalance, decimal monthlyPayment, decimal monthlyInterestRate);

    /// <summary>
    /// Calculates processing fee
    /// </summary>
    decimal CalculateProcessingFee(decimal amount, PaymentMethod paymentMethod);

    /// <summary>
    /// Calculates penalty amount
    /// </summary>
    decimal CalculatePenaltyAmount(decimal amount, WithdrawalPenaltyType penaltyType, 
        decimal? penaltyAmount = null, decimal? penaltyPercentage = null);

    /// <summary>
    /// Calculates delinquency rate
    /// </summary>
    decimal CalculateDelinquencyRate(int delinquentLoans, int totalLoans);

    /// <summary>
    /// Calculates default rate
    /// </summary>
    decimal CalculateDefaultRate(int defaultedLoans, int totalLoans);

    /// <summary>
    /// Calculates risk level
    /// </summary>
    string CalculateRiskLevel(int creditScore, decimal debtToIncomeRatio, decimal loanToValueRatio = 0);

    /// <summary>
    /// Calculates monthly payment for a loan
    /// </summary>
    decimal CalculateMonthlyPayment(decimal principal, decimal annualRate, int termMonths);
}