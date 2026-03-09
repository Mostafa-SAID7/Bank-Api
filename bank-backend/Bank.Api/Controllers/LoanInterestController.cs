using Bank.Application.DTOs;
using Bank.Application.Interfaces;
using Bank.Domain.Enums;
using Bank.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bank.Api.Controllers;

/// <summary>
/// Controller for loan interest calculations and amortization
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class LoanInterestController : ControllerBase
{
    private readonly ILoanInterestCalculationService _loanInterestService;
    private readonly ILoanService _loanService;
    private readonly ILoanRepository _loanRepository;
    private readonly ILogger<LoanInterestController> _logger;

    public LoanInterestController(
        ILoanInterestCalculationService loanInterestService,
        ILoanService loanService,
        ILoanRepository loanRepository,
        ILogger<LoanInterestController> logger)
    {
        _loanInterestService = loanInterestService;
        _loanService = loanService;
        _loanRepository = loanRepository;
        _logger = logger;
    }

    /// <summary>
    /// Calculate monthly payment for loan parameters
    /// </summary>
    [HttpPost("calculate-monthly-payment")]
    public async Task<ActionResult<decimal>> CalculateMonthlyPayment([FromBody] MonthlyPaymentRequest request)
    {
        try
        {
            var monthlyPayment = await _loanInterestService.CalculateMonthlyPaymentAsync(
                request.Principal, request.AnnualRate, request.TermInMonths, request.CalculationMethod);

            return Ok(monthlyPayment);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calculating monthly payment");
            return StatusCode(500, "An error occurred while calculating monthly payment");
        }
    }

    /// <summary>
    /// Generate amortization schedule for a loan
    /// </summary>
    [HttpGet("{loanId}/amortization-schedule")]
    public async Task<ActionResult<AmortizationSchedule>> GetAmortizationSchedule(Guid loanId)
    {
        try
        {
            var loan = await _loanService.GetLoanByIdAsync(loanId);
            if (loan == null)
            {
                return NotFound($"Loan {loanId} not found");
            }

            // Get the actual loan entity for calculations
            var loanEntity = await GetLoanEntityAsync(loanId);
            if (loanEntity == null)
            {
                return NotFound($"Loan entity {loanId} not found");
            }

            var schedule = await _loanInterestService.GenerateAmortizationScheduleAsync(loanEntity);
            return Ok(schedule);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating amortization schedule for loan {LoanId}", loanId);
            return StatusCode(500, "An error occurred while generating amortization schedule");
        }
    }

    /// <summary>
    /// Calculate early payoff amount for a loan
    /// </summary>
    [HttpPost("{loanId}/early-payoff")]
    public async Task<ActionResult<EarlyPayoffCalculation>> CalculateEarlyPayoff(Guid loanId, [FromBody] EarlyPayoffRequest request)
    {
        try
        {
            var calculation = await _loanInterestService.CalculateEarlyPayoffAmountAsync(loanId, request.PayoffDate);
            return Ok(calculation);
        }
        catch (ArgumentException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calculating early payoff for loan {LoanId}", loanId);
            return StatusCode(500, "An error occurred while calculating early payoff amount");
        }
    }

    /// <summary>
    /// Get interest rate for loan type and credit score
    /// </summary>
    [HttpPost("get-interest-rate")]
    public async Task<ActionResult<decimal>> GetInterestRate([FromBody] InterestRateRequest request)
    {
        try
        {
            var rate = await _loanInterestService.GetInterestRateForLoanTypeAsync(
                request.LoanType, request.CreditScore, request.LoanAmount);

            return Ok(rate);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting interest rate for loan type {LoanType}", request.LoanType);
            return StatusCode(500, "An error occurred while getting interest rate");
        }
    }

    /// <summary>
    /// Get loan type configuration
    /// </summary>
    [HttpGet("loan-type-config/{loanType}")]
    public async Task<ActionResult<LoanTypeConfiguration>> GetLoanTypeConfiguration(LoanType loanType)
    {
        try
        {
            var config = await _loanInterestService.GetLoanTypeConfigurationAsync(loanType);
            return Ok(config);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting loan type configuration for {LoanType}", loanType);
            return StatusCode(500, "An error occurred while getting loan type configuration");
        }
    }

    /// <summary>
    /// Calculate remaining interest for a loan
    /// </summary>
    [HttpGet("{loanId}/remaining-interest")]
    public async Task<ActionResult<decimal>> GetRemainingInterest(Guid loanId)
    {
        try
        {
            var loanEntity = await GetLoanEntityAsync(loanId);
            if (loanEntity == null)
            {
                return NotFound($"Loan {loanId} not found");
            }

            var remainingInterest = await _loanInterestService.CalculateRemainingInterestAsync(loanEntity);
            return Ok(remainingInterest);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calculating remaining interest for loan {LoanId}", loanId);
            return StatusCode(500, "An error occurred while calculating remaining interest");
        }
    }

    /// <summary>
    /// Update loan interest rate (Admin only)
    /// </summary>
    [HttpPut("{loanId}/update-rate")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<bool>> UpdateInterestRate(Guid loanId, [FromBody] UpdateLoanRateRequest request)
    {
        try
        {
            var userId = GetCurrentUserId();
            var success = await _loanInterestService.UpdateLoanInterestRateAsync(loanId, request.NewRate, userId);
            
            if (success)
            {
                return Ok(new { Success = true, Message = "Interest rate updated successfully" });
            }
            
            return BadRequest("Failed to update interest rate");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating interest rate for loan {LoanId}", loanId);
            return StatusCode(500, "An error occurred while updating interest rate");
        }
    }

    #region Private Helper Methods

    private async Task<Domain.Entities.Loan?> GetLoanEntityAsync(Guid loanId)
    {
        return await _loanRepository.GetByIdAsync(loanId);
    }

    private Guid GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst("sub") ?? User.FindFirst("id");
        return userIdClaim != null && Guid.TryParse(userIdClaim.Value, out var userId) ? userId : Guid.Empty;
    }

    #endregion
}

