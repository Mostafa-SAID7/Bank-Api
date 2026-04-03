using Bank.Application.DTOs;
using Bank.Application.Interfaces;
using Bank.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bank.Api.Controllers.Payment;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PaymentTemplateController : ControllerBase
{
    private readonly IPaymentTemplateService _templateService;
    private readonly ILogger<PaymentTemplateController> _logger;

    public PaymentTemplateController(
        IPaymentTemplateService templateService,
        ILogger<PaymentTemplateController> logger)
    {
        _templateService = templateService;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> CreateTemplate([FromBody] CreatePaymentTemplateRequest request)
    {
        try
        {
            var template = await _templateService.CreateTemplateAsync(request);
            return CreatedAtAction(nameof(GetTemplate), new { id = template.Id }, template);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating payment template");
            return StatusCode(500, "An error occurred while creating the payment template");
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTemplate(Guid id)
    {
        try
        {
            var template = await _templateService.GetTemplateAsync(id);
            if (template == null)
                return NotFound();

            return Ok(template);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving payment template {TemplateId}", id);
            return StatusCode(500, "An error occurred while retrieving the payment template");
        }
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetUserTemplates(Guid userId)
    {
        try
        {
            var templates = await _templateService.GetUserTemplatesAsync(userId);
            return Ok(templates);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving payment templates for user {UserId}", userId);
            return StatusCode(500, "An error occurred while retrieving payment templates");
        }
    }

    [HttpGet("account/{accountId}")]
    public async Task<IActionResult> GetAccountTemplates(Guid accountId)
    {
        try
        {
            var templates = await _templateService.GetAccountTemplatesAsync(accountId);
            return Ok(templates);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving payment templates for account {AccountId}", accountId);
            return StatusCode(500, "An error occurred while retrieving payment templates");
        }
    }

    [HttpGet("user/{userId}/category/{category}")]
    public async Task<IActionResult> GetTemplatesByCategory(Guid userId, PaymentTemplateCategory category)
    {
        try
        {
            var templates = await _templateService.GetTemplatesByCategoryAsync(userId, category);
            return Ok(templates);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving payment templates by category for user {UserId}", userId);
            return StatusCode(500, "An error occurred while retrieving payment templates");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTemplate(Guid id, [FromBody] UpdatePaymentTemplateRequest request)
    {
        try
        {
            var success = await _templateService.UpdateTemplateAsync(id, request);
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
            _logger.LogError(ex, "Error updating payment template {TemplateId}", id);
            return StatusCode(500, "An error occurred while updating the payment template");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTemplate(Guid id)
    {
        try
        {
            var success = await _templateService.DeleteTemplateAsync(id);
            if (!success)
                return NotFound();

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting payment template {TemplateId}", id);
            return StatusCode(500, "An error occurred while deleting the payment template");
        }
    }

    [HttpPost("{id}/activate")]
    public async Task<IActionResult> ActivateTemplate(Guid id)
    {
        try
        {
            var success = await _templateService.ActivateTemplateAsync(id);
            if (!success)
                return NotFound();

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error activating payment template {TemplateId}", id);
            return StatusCode(500, "An error occurred while activating the payment template");
        }
    }

    [HttpPost("{id}/deactivate")]
    public async Task<IActionResult> DeactivateTemplate(Guid id)
    {
        try
        {
            var success = await _templateService.DeactivateTemplateAsync(id);
            if (!success)
                return NotFound();

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deactivating payment template {TemplateId}", id);
            return StatusCode(500, "An error occurred while deactivating the payment template");
        }
    }

    [HttpPost("{id}/execute")]
    public async Task<IActionResult> ExecuteTemplate(Guid id, [FromBody] ExecuteTemplateRequest request)
    {
        try
        {
            var transaction = await _templateService.ExecuteTemplateAsync(id, request);
            return Ok(transaction);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error executing payment template {TemplateId}", id);
            return StatusCode(500, "An error occurred while executing the payment template");
        }
    }

    [HttpGet("user/{userId}/most-used")]
    public async Task<IActionResult> GetMostUsedTemplates(Guid userId, [FromQuery] int count = 10)
    {
        try
        {
            var templates = await _templateService.GetMostUsedTemplatesAsync(userId, count);
            return Ok(templates);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving most used templates for user {UserId}", userId);
            return StatusCode(500, "An error occurred while retrieving most used templates");
        }
    }

    [HttpGet("user/{userId}/recently-used")]
    public async Task<IActionResult> GetRecentlyUsedTemplates(Guid userId, [FromQuery] int count = 10)
    {
        try
        {
            var templates = await _templateService.GetRecentlyUsedTemplatesAsync(userId, count);
            return Ok(templates);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving recently used templates for user {UserId}", userId);
            return StatusCode(500, "An error occurred while retrieving recently used templates");
        }
    }

    [HttpGet("user/{userId}/search")]
    public async Task<IActionResult> SearchTemplates(Guid userId, [FromQuery] string searchTerm)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return BadRequest("Search term is required");

            var templates = await _templateService.SearchTemplatesAsync(userId, searchTerm);
            return Ok(templates);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching templates for user {UserId}", userId);
            return StatusCode(500, "An error occurred while searching templates");
        }
    }

    [HttpGet("user/{userId}/tags")]
    public async Task<IActionResult> GetTemplatesByTags(Guid userId, [FromQuery] string[] tags)
    {
        try
        {
            if (tags == null || tags.Length == 0)
                return BadRequest("At least one tag is required");

            var templates = await _templateService.GetTemplatesByTagsAsync(userId, tags);
            return Ok(templates);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving templates by tags for user {UserId}", userId);
            return StatusCode(500, "An error occurred while retrieving templates by tags");
        }
    }
}