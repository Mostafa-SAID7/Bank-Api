using Bank.Application.Interfaces;
using Bank.Application.Utilities;
using Bank.Domain.Enums;

namespace Bank.Application.Services;

/// <summary>
/// Service implementation for financial calculations
/// </summary>
public class CalculationService : ICalculationService
{
    public decimal CalculateCompoundInterest(decimal principal, decimal rate, int compoundingFrequency, decimal time)
        => CalculationHelper.CalculateCompoundInterest(principal, rate, compoundingFrequency, time);

    public decimal CalculateSimpleInterest(decimal principal, decimal rate, decimal time)
        => CalculationHelper.CalculateSimpleInterest(principal, rate, time);

    public decimal CalculateInterestRateFromScore(int creditScore, decimal baseRate = 0.03m, decimal maxRate = 0.25m)
        => CalculationHelper.CalculateInterestRateFromScore(creditScore, baseRate, maxRate);

    public decimal CalculateMaxLoanAmount(decimal monthlyIncome, decimal existingDebt, 
        decimal maxDebtToIncomeRatio, int loanTermMonths, decimal interestRate)
        => CalculationHelper.CalculateMaxLoanAmount(monthlyIncome, existingDebt, maxDebtToIncomeRatio, loanTermMonths, interestRate);

    public (decimal PrincipalPayment, decimal InterestPayment) CalculatePaymentAllocation(
        decimal remainingBalance, decimal monthlyPayment, decimal monthlyInterestRate)
        => CalculationHelper.CalculatePaymentAllocation(remainingBalance, monthlyPayment, monthlyInterestRate);

    public decimal CalculateProcessingFee(decimal amount, PaymentMethod paymentMethod)
        => CalculationHelper.CalculateProcessingFee(amount, paymentMethod);

    public decimal CalculatePenaltyAmount(decimal amount, WithdrawalPenaltyType penaltyType, 
        decimal? penaltyAmount = null, decimal? penaltyPercentage = null)
        => CalculationHelper.CalculatePenaltyAmount(amount, penaltyType, penaltyAmount, penaltyPercentage);

    public decimal CalculateDelinquencyRate(int delinquentLoans, int totalLoans)
        => CalculationHelper.CalculateDelinquencyRate(delinquentLoans, totalLoans);

    public decimal CalculateDefaultRate(int defaultedLoans, int totalLoans)
        => CalculationHelper.CalculateDefaultRate(defaultedLoans, totalLoans);

    public string CalculateRiskLevel(int creditScore, decimal debtToIncomeRatio, decimal loanToValueRatio = 0)
        => CalculationHelper.CalculateRiskLevel(creditScore, debtToIncomeRatio, loanToValueRatio);

    public decimal CalculateMonthlyPayment(decimal principal, decimal annualRate, int termMonths)
        => CalculationHelper.CalculateMonthlyPayment(principal, annualRate, termMonths);
}