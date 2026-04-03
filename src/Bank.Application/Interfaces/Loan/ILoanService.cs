using Bank.Application.DTOs;
using Bank.Domain.Entities;

namespace Bank.Application.Interfaces;

/// <summary>
/// Service interface for loan management operations
/// </summary>
public interface ILoanService
{
    /// <summary>
    /// Submit a new loan application
    /// </summary>
    Task<LoanApplicationResult> SubmitApplicationAsync(Guid customerId, LoanApplicationRequest request);
    
    /// <summary>
    /// Perform automated credit scoring for a customer
    /// </summary>
    Task<CreditScoreResult> PerformCreditScoringAsync(Guid customerId, Guid loanId);
    
    /// <summary>
    /// Process loan approval or rejection
    /// </summary>
    Task<LoanApprovalResult> ProcessApprovalAsync(Guid loanId, ApprovalDecision decision, Guid approvedBy);
    
    /// <summary>
    /// Disburse an approved loan
    /// </summary>
    Task<DisbursementResult> DisburseLoanAsync(Guid loanId, Guid disbursedBy);
    
    /// <summary>
    /// Process a loan payment
    /// </summary>
    Task<PaymentResult> ProcessPaymentAsync(LoanPaymentRequest request, Guid processedBy);
    
    /// <summary>
    /// Get all loans for a customer
    /// </summary>
    Task<List<LoanDto>> GetCustomerLoansAsync(Guid customerId);
    
    /// <summary>
    /// Get loan details by ID
    /// </summary>
    Task<LoanDto?> GetLoanByIdAsync(Guid loanId);
    
    /// <summary>
    /// Generate repayment schedule for a loan
    /// </summary>
    Task<RepaymentSchedule> GenerateRepaymentScheduleAsync(Guid loanId);
    
    /// <summary>
    /// Search loans with filtering and pagination
    /// </summary>
    Task<(List<LoanDto> Loans, int TotalCount)> SearchLoansAsync(LoanSearchRequest request);
    
    /// <summary>
    /// Get loan payment history
    /// </summary>
    Task<List<LoanPayment>> GetLoanPaymentHistoryAsync(Guid loanId);
    
    /// <summary>
    /// Mark overdue loans as delinquent
    /// </summary>
    Task<int> ProcessDelinquentLoansAsync();
    
    /// <summary>
    /// Calculate next payment details for a loan
    /// </summary>
    Task<RepaymentScheduleEntry?> GetNextPaymentDetailsAsync(Guid loanId);
    
    /// <summary>
    /// Update loan status with history tracking
    /// </summary>
    Task UpdateLoanStatusAsync(Guid loanId, Domain.Enums.LoanStatus newStatus, Guid? changedBy = null, string? reason = null);
}