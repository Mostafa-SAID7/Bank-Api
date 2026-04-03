using Bank.Application.DTOs;
using Bank.Domain.Entities;
using Bank.Domain.Enums;

namespace Bank.Application.Interfaces;

/// <summary>
/// Service interface for loan-specific interest calculations
/// </summary>
public interface ILoanInterestCalculationService
{
    /// <summary>
    /// Calculate interest for a loan payment using the loan's calculation method
    /// </summary>
    Task<LoanInterestCalculationResult> CalculateLoanInterestAsync(Loan loan, decimal paymentAmount);
    
    /// <summary>
    /// Calculate monthly payment amount for a loan
    /// </summary>
    Task<decimal> CalculateMonthlyPaymentAsync(decimal principal, decimal annualRate, int termInMonths, InterestCalculationMethod method);
    
    /// <summary>
    /// Generate complete amortization schedule for a loan
    /// </summary>
    Task<AmortizationSchedule> GenerateAmortizationScheduleAsync(Loan loan);
    
    /// <summary>
    /// Calculate remaining interest for a loan
    /// </summary>
    Task<decimal> CalculateRemainingInterestAsync(Loan loan);
    
    /// <summary>
    /// Calculate early payoff amount for a loan
    /// </summary>
    Task<EarlyPayoffCalculation> CalculateEarlyPayoffAmountAsync(Guid loanId, DateTime payoffDate);
    
    /// <summary>
    /// Get interest rate for a specific loan type and credit score
    /// </summary>
    Task<decimal> GetInterestRateForLoanTypeAsync(LoanType loanType, int creditScore, decimal loanAmount);
    
    /// <summary>
    /// Calculate late fees for overdue loan payments
    /// </summary>
    Task<decimal> CalculateLateFeeAsync(Loan loan, int daysOverdue);
    
    /// <summary>
    /// Update loan interest rate and recalculate schedule
    /// </summary>
    Task<bool> UpdateLoanInterestRateAsync(Guid loanId, decimal newRate, Guid updatedBy);
    
    /// <summary>
    /// Calculate interest accrued between two dates
    /// </summary>
    Task<decimal> CalculateAccruedInterestAsync(Loan loan, DateTime fromDate, DateTime toDate);
    
    /// <summary>
    /// Get loan type configuration including default rates and terms
    /// </summary>
    Task<LoanTypeConfiguration> GetLoanTypeConfigurationAsync(LoanType loanType);
}