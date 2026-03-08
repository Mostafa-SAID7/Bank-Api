using Bank.Application.DTOs;
using Bank.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bank.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class RecurringPaymentController : ControllerBase
{
    private readonly IRecurringPaymentService _recurringPaymentService;
    private readonly ILogger<RecurringPaymentController> _logger;

    public RecurringPaymentController(
        IRecurringPaymentService recurringPaymentService,
        ILogger<RecurringPaymentController> logger)
    {
        _recurringPaymentService = recurringPaymentService;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> CreateRecurringPayment([FromBody] CreateRecurringPaymentRequest request)
    {
        try
        {
            var recurringPayment = await _recurringPaymentService.CreateRecurringPaymentAsync(request);
            return CreatedAtAction(nameof(GetRecurringPayment), new { id = recurringPayment.Id }, recurringPayment);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating recurring payment");
            return StatusCode(500, "An error occurred while creating the recurring payment");
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetRecurringPayment(Guid id)
    {
        try
        {
            var recurringPayment = await _recurringPaymentService.GetRecurringPaymentAsync(id);
            if (recurringPayment == null)
                return NotFound();

            return Ok(recurringPayment);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving recurring payment {RecurringPaymentId}", id);
            return StatusCode(500, "An error occurred while retrieving the recurring payment");
        }
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetUserRecurringPayments(Guid userId)
    {
        try
        {
            var recurringPayments = await _recurringPaymentService.GetUserRecurringPaymentsAsync(userId);
            return Ok(recurringPayments);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving recurring payments for user {UserId}", userId);
            return StatusCode(500, "An error occurred while retrieving recurring payments");
        }
    }

    [HttpGet("account/{accountId}")]
    public async Task<IActionResult> GetAccountRecurringPayments(Guid accountId)
    {
        try
        {
            var recurringPayments = await _recurringPaymentService.GetAccountRecurringPaymentsAsync(accountId);
            return Ok(recurringPayments);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving recurring payments for account {AccountId}", accountId);
            return StatusCode(500, "An error occurred while retrieving recurring payments");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateRecurringPayment(Guid id, [FromBody] UpdateRecurringPaymentRequest request)
    {
        try
        {
            var success = await _recurringPaymentService.UpdateRecurringPaymentAsync(id, request);
            if (!success)
                return NotFound();

            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating recurring payment {RecurringPaymentId}", id);
            return StatusCode(500, "An error occurred while updating the recurring payment");
        }
    }

    [HttpPost("{id}/pause")]
    public async Task<IActionResult> PauseRecurringPayment(Guid id, [FromBody] string reason)
    {
        try
        {
            var success = await _recurringPaymentService.PauseRecurringPaymentAsync(id, reason);
            if (!success)
                return NotFound();

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error pausing recurring payment {RecurringPaymentId}", id);
            return StatusCode(500, "An error occurred while pausing the recurring payment");
        }
    }

    [HttpPost("{id}/resume")]
    public async Task<IActionResult> ResumeRecurringPayment(Guid id)
    {
        try
        {
            var success = await _recurringPaymentService.ResumeRecurringPaymentAsync(id);
            if (!success)
                return NotFound();

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error resuming recurring payment {RecurringPaymentId}", id);
            return StatusCode(500, "An error occurred while resuming the recurring payment");
        }
    }

    [HttpPost("{id}/cancel")]
    public async Task<IActionResult> CancelRecurringPayment(Guid id, [FromBody] string reason)
    {
        try
        {
            var success = await _recurringPaymentService.CancelRecurringPaymentAsync(id, reason);
            if (!success)
                return NotFound();

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error cancelling recurring payment {RecurringPaymentId}", id);
            return StatusCode(500, "An error occurred while cancelling the recurring payment");
        }
    }

    [HttpGet("{id}/executions")]
    public async Task<IActionResult> GetExecutionHistory(Guid id)
    {
        try
        {
            var executions = await _recurringPaymentService.GetExecutionHistoryAsync(id);
            return Ok(executions);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving execution history for recurring payment {RecurringPaymentId}", id);
            return StatusCode(500, "An error occurred while retrieving execution history");
        }
    }

    [HttpPost("bulk-transfer")]
    public async Task<IActionResult> ProcessBulkTransfers([FromBody] BulkTransferRequest request)
    {
        try
        {
            var result = await _recurringPaymentService.ProcessBulkTransfersAsync(request);
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing bulk transfers");
            return StatusCode(500, "An error occurred while processing bulk transfers");
        }
    }

    [HttpGet("bulk-transfer/{batchId}/status")]
    public async Task<IActionResult> GetBulkTransferStatus(Guid batchId)
    {
        try
        {
            var result = await _recurringPaymentService.GetBulkTransferStatusAsync(batchId);
            return Ok(result);
        }
        catch (NotImplementedException)
        {
            return StatusCode(501, "Bulk transfer status retrieval not yet implemented");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving bulk transfer status for batch {BatchId}", batchId);
            return StatusCode(500, "An error occurred while retrieving bulk transfer status");
        }
    }

    [HttpGet("due")]
    [Authorize(Roles = "Admin,System")]
    public async Task<IActionResult> GetDueRecurringPayments()
    {
        try
        {
            var duePayments = await _recurringPaymentService.GetDueRecurringPaymentsAsync();
            return Ok(duePayments);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving due recurring payments");
            return StatusCode(500, "An error occurred while retrieving due recurring payments");
        }
    }

    [HttpPost("process-due")]
    [Authorize(Roles = "Admin,System")]
    public async Task<IActionResult> ProcessDueRecurringPayments()
    {
        try
        {
            var processedCount = await _recurringPaymentService.ProcessDueRecurringPaymentsAsync();
            return Ok(new { ProcessedCount = processedCount });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing due recurring payments");
            return StatusCode(500, "An error occurred while processing due recurring payments");
        }
    }
}