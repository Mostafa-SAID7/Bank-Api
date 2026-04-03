using Bank.Api.Helpers;
using Bank.Application.DTOs;
using Bank.Application.Interfaces;
using Bank.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bank.Api.Controllers.Payment;

/// <summary>
/// Controller for bill payment operations
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BillPaymentController : ControllerBase
{
    private readonly IBillPaymentService _billPaymentService;
    private readonly ILogger<BillPaymentController> _logger;

    public BillPaymentController(
        IBillPaymentService billPaymentService,
        ILogger<BillPaymentController> logger)
    {
        _billPaymentService = billPaymentService;
        _logger = logger;
    }

    /// <summary>
    /// Get all available billers
    /// </summary>
    [HttpGet("billers")]
    public async Task<ActionResult<List<BillerDto>>> GetAvailableBillers()
    {
        try
        {
            var billers = await _billPaymentService.GetAvailableBillersAsync();
            return Ok(billers);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving available billers");
            return StatusCode(500, "An error occurred while retrieving billers");
        }
    }

    /// <summary>
    /// Get billers by category
    /// </summary>
    [HttpGet("billers/category/{category}")]
    public async Task<ActionResult<List<BillerDto>>> GetBillersByCategory(BillerCategory category)
    {
        try
        {
            var billers = await _billPaymentService.GetBillersByCategoryAsync(category);
            return Ok(billers);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving billers for category {Category}", category);
            return StatusCode(500, "An error occurred while retrieving billers");
        }
    }

    /// <summary>
    /// Search billers
    /// </summary>
    [HttpPost("billers/search")]
    public async Task<ActionResult<List<BillerDto>>> SearchBillers([FromBody] BillerSearchRequest request)
    {
        try
        {
            var billers = await _billPaymentService.SearchBillersAsync(request);
            return Ok(billers);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching billers");
            return StatusCode(500, "An error occurred while searching billers");
        }
    }

    /// <summary>
    /// Get biller details by ID
    /// </summary>
    [HttpGet("billers/{billerId}")]
    public async Task<ActionResult<BillerDto>> GetBillerById(Guid billerId)
    {
        try
        {
            var biller = await _billPaymentService.GetBillerByIdAsync(billerId);
            if (biller == null)
            {
                return NotFound("Biller not found");
            }
            return Ok(biller);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving biller {BillerId}", billerId);
            return StatusCode(500, "An error occurred while retrieving biller details");
        }
    }

    /// <summary>
    /// Schedule a bill payment
    /// </summary>
    [HttpPost("schedule")]
    public async Task<ActionResult<ScheduleBillPaymentResponse>> ScheduleBillPayment([FromBody] ScheduleBillPaymentRequest request)
    {
        try
        {
            var customerId = this.GetCurrentUserId();
            var response = await _billPaymentService.ScheduleBillPaymentAsync(customerId, request);
            
            if (response.Status == BillPaymentStatus.Failed)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error scheduling bill payment");
            return StatusCode(500, "An error occurred while scheduling the payment");
        }
    }

    /// <summary>
    /// Get bill payment history for the current customer
    /// </summary>
    [HttpPost("history")]
    public async Task<ActionResult<Bank.Domain.Common.PagedResult<BillPaymentHistoryDto>>> GetBillPaymentHistory([FromBody] BillPaymentHistoryRequest request)
    {
        try
        {
            var customerId = this.GetCurrentUserId();
            var history = await _billPaymentService.GetBillPaymentHistoryAsync(customerId, request);
            return Ok(history);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving bill payment history");
            return StatusCode(500, "An error occurred while retrieving payment history");
        }
    }

    /// <summary>
    /// Get pending bill payments for the current customer
    /// </summary>
    [HttpGet("pending")]
    public async Task<ActionResult<List<BillPaymentDto>>> GetPendingBillPayments()
    {
        try
        {
            var customerId = this.GetCurrentUserId();
            var payments = await _billPaymentService.GetPendingBillPaymentsAsync(customerId);
            return Ok(payments);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving pending bill payments");
            return StatusCode(500, "An error occurred while retrieving pending payments");
        }
    }

    /// <summary>
    /// Get bill payment details by ID
    /// </summary>
    [HttpGet("{paymentId}")]
    public async Task<ActionResult<BillPaymentDto>> GetBillPaymentById(Guid paymentId)
    {
        try
        {
            var customerId = this.GetCurrentUserId();
            var payment = await _billPaymentService.GetBillPaymentByIdAsync(customerId, paymentId);
            
            if (payment == null)
            {
                return NotFound("Payment not found");
            }

            return Ok(payment);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving bill payment {PaymentId}", paymentId);
            return StatusCode(500, "An error occurred while retrieving payment details");
        }
    }

    /// <summary>
    /// Cancel a scheduled bill payment
    /// </summary>
    [HttpPost("{paymentId}/cancel")]
    public async Task<ActionResult> CancelScheduledPayment(Guid paymentId)
    {
        try
        {
            var customerId = this.GetCurrentUserId();
            var success = await _billPaymentService.CancelScheduledPaymentAsync(customerId, paymentId);
            
            if (!success)
            {
                return BadRequest("Payment cannot be cancelled");
            }

            return Ok(new { message = "Payment cancelled successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error cancelling bill payment {PaymentId}", paymentId);
            return StatusCode(500, "An error occurred while cancelling the payment");
        }
    }

    /// <summary>
    /// Update a scheduled bill payment
    /// </summary>
    [HttpPut("{paymentId}")]
    public async Task<ActionResult> UpdateScheduledPayment(Guid paymentId, [FromBody] UpdateBillPaymentRequest request)
    {
        try
        {
            var customerId = this.GetCurrentUserId();
            var success = await _billPaymentService.UpdateScheduledPaymentAsync(customerId, paymentId, request);
            
            if (!success)
            {
                return BadRequest("Payment cannot be updated");
            }

            return Ok(new { message = "Payment updated successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating bill payment {PaymentId}", paymentId);
            return StatusCode(500, "An error occurred while updating the payment");
        }
    }
}