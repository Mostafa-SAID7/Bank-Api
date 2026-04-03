using Bank.Application.Interfaces;
using Bank.Application.Utilities.Deposit;
using Bank.Application.Utilities.Loan;
using Bank.Application.Utilities.Payment;
using Bank.Domain.Enums;

namespace Bank.Application.Services;

/// <summary>
/// Service implementation for financial calculations
/// </summary>
public class CalculationService : ICalculationService
{
    public decimal CalculateCompoundInterest(decimal principal, decimal rate, int compoundingFrequency, decimal time)
        => InterestCalculationHelper.CalculateCompoundInterest(principal, rate, compoundingFrequency, time);

    public decimal CalculateSimpleInterest(decimal principal, decimal rate, decimal time)
        => InterestCalculationHelper.CalculateSimpleInterest(principal, rate, time);

    public decimal CalculateInterestRateFromScore(int creditScore, decimal baseRate = 0.03m, decimal maxRate = 0.25m)
        => InterestCalculationHelper.CalculateInterestRateFromScore(creditScore, baseRate, maxRate);

    public decimal CalculateMaxLoanAmount(decimal monthlyIncome, decimal existingDebt, 
        decimal maxDebtToIncomeRatio, int loanTermMonths, decimal interestRate)
        => LoanCalculationHelper.CalculateMaxLoanAmount(monthlyIncome, existingDebt, maxDebtToIncomeRatio, loanTermMonths, interestRate);

    public (decimal PrincipalPayment, decimal InterestPayment) CalculatePaymentAllocation(
        decimal remainingBalance, decimal monthlyPayment, decimal monthlyInterestRate)
        => PaymentCalculationHelper.CalculatePaymentAllocation(remainingBalance, monthlyPayment, monthlyInterestRate);

    public decimal CalculateProcessingFee(decimal amount, PaymentMethod paymentMethod)
        => PaymentCalculationHelper.CalculateProcessingFee(amount, paymentMethod);

    public decimal CalculatePenaltyAmount(decimal amount, WithdrawalPenaltyType penaltyType, 
        decimal? penaltyAmount = null, decimal? penaltyPercentage = null)
        => PenaltyCalculationHelper.CalculatePenaltyAmount(amount, penaltyType, penaltyAmount, penaltyPercentage);

    public decimal CalculateDelinquencyRate(int delinquentLoans, int totalLoans)
        => RiskCalculationHelper.CalculateDelinquencyRate(delinquentLoans, totalLoans);

    public decimal CalculateDefaultRate(int defaultedLoans, int totalLoans)
        => RiskCalculationHelper.CalculateDefaultRate(defaultedLoans, totalLoans);

    public string CalculateRiskLevel(int creditScore, decimal debtToIncomeRatio, decimal loanToValueRatio = 0)
        => RiskCalculationHelper.CalculateRiskLevel(creditScore, debtToIncomeRatio, loanToValueRatio);

    public decimal CalculateMonthlyPayment(decimal principal, decimal annualRate, int termMonths)
        => LoanCalculationHelper.CalculateMonthlyPayment(principal, annualRate, termMonths);
}