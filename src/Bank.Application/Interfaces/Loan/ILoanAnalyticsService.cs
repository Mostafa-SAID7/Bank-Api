using Bank.Application.DTOs;
using Bank.Domain.Enums;

namespace Bank.Application.Interfaces;

/// <summary>
/// Service interface for loan analytics and reporting
/// </summary>
public interface ILoanAnalyticsService
{
    /// <summary>
    /// Get comprehensive loan analytics for a date range
    /// </summary>
    Task<LoanAnalyticsDto> GetLoanAnalyticsAsync(DateTime? fromDate = null, DateTime? toDate = null);
    
    /// <summary>
    /// Get loan performance metrics for a specific loan
    /// </summary>
    Task<LoanPerformanceMetrics> GetLoanPerformanceAsync(Guid loanId);
    
    /// <summary>
    /// Get loan portfolio summary by loan type
    /// </summary>
    Task<Dictionary<LoanType, LoanAnalyticsDto>> GetPortfolioByTypeAsync();
    
    /// <summary>
    /// Get delinquency report
    /// </summary>
    Task<List<LoanDto>> GetDelinquencyReportAsync();
    
    /// <summary>
    /// Get loans approaching maturity
    /// </summary>
    Task<List<LoanDto>> GetLoansApproachingMaturityAsync(int daysAhead = 30);
    
    /// <summary>
    /// Calculate portfolio risk metrics
    /// </summary>
    Task<PortfolioRiskMetrics> CalculatePortfolioRiskAsync();
    
    /// <summary>
    /// Get loan origination trends
    /// </summary>
    Task<List<LoanOriginationTrend>> GetOriginationTrendsAsync(int months = 12);
    
    /// <summary>
    /// Get customer loan summary
    /// </summary>
    Task<CustomerLoanSummary> GetCustomerLoanSummaryAsync(Guid customerId);
}