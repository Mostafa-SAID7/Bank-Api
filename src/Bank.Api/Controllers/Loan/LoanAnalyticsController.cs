using Bank.Application.DTOs;
using Bank.Application.Interfaces;
using Bank.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bank.Api.Controllers.Loan;

/// <summary>
/// Controller for loan analytics and reporting
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class LoanAnalyticsController : ControllerBase
{
    private readonly ILoanAnalyticsService _loanAnalyticsService;
    private readonly ILogger<LoanAnalyticsController> _logger;

    public LoanAnalyticsController(
        ILoanAnalyticsService loanAnalyticsService,
        ILogger<LoanAnalyticsController> logger)
    {
        _loanAnalyticsService = loanAnalyticsService;
        _logger = logger;
    }

    /// <summary>
    /// Get comprehensive loan analytics
    /// </summary>
    [HttpGet("overview")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<ActionResult<LoanAnalyticsDto>> GetLoanAnalytics([FromQuery] DateTime? fromDate, [FromQuery] DateTime? toDate)
    {
        try
        {
            var analytics = await _loanAnalyticsService.GetLoanAnalyticsAsync(fromDate, toDate);
            return Ok(analytics);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting loan analytics");
            return StatusCode(500, "An error occurred while generating loan analytics");
        }
    }

    /// <summary>
    /// Get loan performance metrics for a specific loan
    /// </summary>
    [HttpGet("performance/{loanId}")]
    public async Task<ActionResult<LoanPerformanceMetrics>> GetLoanPerformance(Guid loanId)
    {
        try
        {
            var performance = await _loanAnalyticsService.GetLoanPerformanceAsync(loanId);
            return Ok(performance);
        }
        catch (ArgumentException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting loan performance for loan {LoanId}", loanId);
            return StatusCode(500, "An error occurred while getting loan performance");
        }
    }

    /// <summary>
    /// Get loan portfolio summary by type
    /// </summary>
    [HttpGet("portfolio-by-type")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<ActionResult<Dictionary<LoanType, LoanAnalyticsDto>>> GetPortfolioByType()
    {
        try
        {
            var portfolio = await _loanAnalyticsService.GetPortfolioByTypeAsync();
            return Ok(portfolio);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting portfolio by type");
            return StatusCode(500, "An error occurred while getting portfolio by type");
        }
    }

    /// <summary>
    /// Get delinquency report
    /// </summary>
    [HttpGet("delinquency-report")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<ActionResult<List<LoanDto>>> GetDelinquencyReport()
    {
        try
        {
            var report = await _loanAnalyticsService.GetDelinquencyReportAsync();
            return Ok(report);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating delinquency report");
            return StatusCode(500, "An error occurred while generating delinquency report");
        }
    }

    /// <summary>
    /// Get loans approaching maturity
    /// </summary>
    [HttpGet("approaching-maturity")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<ActionResult<List<LoanDto>>> GetLoansApproachingMaturity([FromQuery] int daysAhead = 30)
    {
        try
        {
            var loans = await _loanAnalyticsService.GetLoansApproachingMaturityAsync(daysAhead);
            return Ok(loans);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting loans approaching maturity");
            return StatusCode(500, "An error occurred while getting loans approaching maturity");
        }
    }

    /// <summary>
    /// Calculate portfolio risk metrics
    /// </summary>
    [HttpGet("portfolio-risk")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<ActionResult<PortfolioRiskMetrics>> GetPortfolioRisk()
    {
        try
        {
            var risk = await _loanAnalyticsService.CalculatePortfolioRiskAsync();
            return Ok(risk);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calculating portfolio risk");
            return StatusCode(500, "An error occurred while calculating portfolio risk");
        }
    }

    /// <summary>
    /// Get loan origination trends
    /// </summary>
    [HttpGet("origination-trends")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<ActionResult<List<LoanOriginationTrend>>> GetOriginationTrends([FromQuery] int months = 12)
    {
        try
        {
            var trends = await _loanAnalyticsService.GetOriginationTrendsAsync(months);
            return Ok(trends);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting origination trends");
            return StatusCode(500, "An error occurred while getting origination trends");
        }
    }

    /// <summary>
    /// Get customer loan summary
    /// </summary>
    [HttpGet("customer-summary/{customerId}")]
    public async Task<ActionResult<CustomerLoanSummary>> GetCustomerLoanSummary(Guid customerId)
    {
        try
        {
            // Ensure user can only access their own data or is admin/manager
            var currentUserId = GetCurrentUserId();
            if (currentUserId != customerId && !User.IsInRole("Admin") && !User.IsInRole("Manager"))
            {
                return Forbid("You can only access your own loan summary");
            }

            var summary = await _loanAnalyticsService.GetCustomerLoanSummaryAsync(customerId);
            return Ok(summary);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting customer loan summary for customer {CustomerId}", customerId);
            return StatusCode(500, "An error occurred while getting customer loan summary");
        }
    }

    /// <summary>
    /// Get my loan summary (current user)
    /// </summary>
    [HttpGet("my-summary")]
    public async Task<ActionResult<CustomerLoanSummary>> GetMyLoanSummary()
    {
        try
        {
            var customerId = GetCurrentUserId();
            var summary = await _loanAnalyticsService.GetCustomerLoanSummaryAsync(customerId);
            return Ok(summary);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting loan summary for current user");
            return StatusCode(500, "An error occurred while getting your loan summary");
        }
    }

    #region Private Helper Methods

    private Guid GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst("sub") ?? User.FindFirst("id");
        return userIdClaim != null && Guid.TryParse(userIdClaim.Value, out var userId) ? userId : Guid.Empty;
    }

    #endregion
}