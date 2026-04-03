using Bank.Api.Helpers;
using Bank.Application.DTOs;
using Bank.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Bank.Api.Controllers.Card;

/// <summary>
/// Controller for card management operations
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CardController : ControllerBase
{
    private readonly ICardService _cardService;
    private readonly ILogger<CardController> _logger;

    public CardController(ICardService cardService, ILogger<CardController> logger)
    {
        _cardService = cardService;
        _logger = logger;
    }

    /// <summary>
    /// Issue a new card for a customer account
    /// </summary>
    [HttpPost("issue")]
    public async Task<ActionResult<CardIssuanceResult>> IssueCard([FromBody] CardIssuanceRequest request)
    {
        try
        {
            var userId = GetCurrentUserId();
            if (request.CustomerId != userId)
            {
                return Forbid("You can only issue cards for your own account");
            }

            var result = await _cardService.IssueCardAsync(request);
            
            if (result.Success)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error issuing card for customer {CustomerId}", request.CustomerId);
            return StatusCode(500, "An error occurred while issuing the card");
        }
    }

    /// <summary>
    /// Activate a card
    /// </summary>
    [HttpPost("{cardId}/activate")]
    public async Task<ActionResult<CardActivationResult>> ActivateCard(Guid cardId, [FromBody] CardActivationRequest request)
    {
        try
        {
            var userId = GetCurrentUserId();
            if (request.CustomerId != userId)
            {
                return Forbid("You can only activate your own cards");
            }

            request.CardId = cardId;
            var result = await _cardService.ActivateCardAsync(request);
            
            if (result.Success)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error activating card {CardId}", cardId);
            return StatusCode(500, "An error occurred while activating the card");
        }
    }

    /// <summary>
    /// Block a card
    /// </summary>
    [HttpPost("{cardId}/block")]
    public async Task<ActionResult<CardBlockResult>> BlockCard(Guid cardId, [FromBody] CardBlockRequest request)
    {
        try
        {
            var userId = GetCurrentUserId();
            if (request.CustomerId != userId)
            {
                return Forbid("You can only block your own cards");
            }

            request.CardId = cardId;
            var result = await _cardService.BlockCardAsync(request);
            
            if (result.Success)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error blocking card {CardId}", cardId);
            return StatusCode(500, "An error occurred while blocking the card");
        }
    }

    /// <summary>
    /// Unblock a card
    /// </summary>
    [HttpPost("{cardId}/unblock")]
    public async Task<ActionResult<CardBlockResult>> UnblockCard(Guid cardId, [FromBody] CardUnblockRequest request)
    {
        try
        {
            var userId = GetCurrentUserId();
            if (request.CustomerId != userId)
            {
                return Forbid("You can only unblock your own cards");
            }

            request.CardId = cardId;
            var result = await _cardService.UnblockCardAsync(request);
            
            if (result.Success)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error unblocking card {CardId}", cardId);
            return StatusCode(500, "An error occurred while unblocking the card");
        }
    }
    /// <summary>
    /// Update card spending limits
    /// </summary>
    [HttpPut("{cardId}/limits")]
    public async Task<ActionResult<CardLimitUpdateResult>> UpdateLimits(Guid cardId, [FromBody] CardLimitUpdateRequest request)
    {
        try
        {
            var userId = GetCurrentUserId();
            if (request.CustomerId != userId)
            {
                return Forbid("You can only update limits for your own cards");
            }

            request.CardId = cardId;
            var result = await _cardService.UpdateLimitsAsync(request);
            
            if (result.Success)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating limits for card {CardId}", cardId);
            return StatusCode(500, "An error occurred while updating card limits");
        }
    }

    /// <summary>
    /// Get card details
    /// </summary>
    [HttpGet("{cardId}")]
    public async Task<ActionResult<CardDetailsDto>> GetCardDetails(Guid cardId)
    {
        try
        {
            var userId = GetCurrentUserId();
            var cardDetails = await _cardService.GetCardDetailsAsync(cardId, userId);
            
            if (cardDetails == null)
            {
                return NotFound("Card not found");
            }

            return Ok(cardDetails);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting card details for card {CardId}", cardId);
            return StatusCode(500, "An error occurred while retrieving card details");
        }
    }

    /// <summary>
    /// Get all cards for the current customer
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<List<CardSummaryDto>>> GetCustomerCards()
    {
        try
        {
            var userId = GetCurrentUserId();
            var cards = await _cardService.GetCustomerCardsAsync(userId);
            
            return Ok(cards);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting cards for customer {CustomerId}", GetCurrentUserId());
            return StatusCode(500, "An error occurred while retrieving cards");
        }
    }

    /// <summary>
    /// Get card transactions with filtering and pagination
    /// </summary>
    [HttpGet("{cardId}/transactions")]
    public async Task<ActionResult<PagedResult<CardTransactionDto>>> GetCardTransactions(
        Guid cardId,
        [FromQuery] DateTime? fromDate = null,
        [FromQuery] DateTime? toDate = null,
        [FromQuery] string? transactionType = null,
        [FromQuery] string? status = null,
        [FromQuery] decimal? minAmount = null,
        [FromQuery] decimal? maxAmount = null,
        [FromQuery] string? merchantName = null,
        [FromQuery] string? merchantCategory = null,
        [FromQuery] bool? isInternational = null,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? sortBy = null,
        [FromQuery] bool sortDescending = true)
    {
        try
        {
            var userId = GetCurrentUserId();
            
            var request = new CardTransactionSearchRequest
            {
                CardId = cardId,
                CustomerId = userId,
                FromDate = fromDate,
                ToDate = toDate,
                MinAmount = minAmount,
                MaxAmount = maxAmount,
                MerchantName = merchantName,
                IsInternational = isInternational,
                Page = page,
                PageSize = Math.Min(pageSize, 100), // Limit page size
                SortBy = sortBy,
                SortDescending = sortDescending
            };

            // Parse enums if provided
            if (Enum.TryParse<Domain.Enums.CardTransactionType>(transactionType, true, out var txType))
                request.TransactionType = txType;
            
            if (Enum.TryParse<Domain.Enums.CardTransactionStatus>(status, true, out var txStatus))
                request.Status = txStatus;
            
            if (Enum.TryParse<Domain.Enums.MerchantCategory>(merchantCategory, true, out var category))
                request.MerchantCategory = category;

            var result = await _cardService.GetCardTransactionsAsync(request);
            
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting transactions for card {CardId}", cardId);
            return StatusCode(500, "An error occurred while retrieving card transactions");
        }
    }
    /// <summary>
    /// Change card PIN
    /// </summary>
    [HttpPost("{cardId}/change-pin")]
    public async Task<ActionResult<CardPinChangeResult>> ChangePin(Guid cardId, [FromBody] CardPinChangeRequest request)
    {
        try
        {
            var userId = GetCurrentUserId();
            if (request.CustomerId != userId)
            {
                return Forbid("You can only change PIN for your own cards");
            }

            request.CardId = cardId;
            var result = await _cardService.ChangePinAsync(request);
            
            if (result.Success)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error changing PIN for card {CardId}", cardId);
            return StatusCode(500, "An error occurred while changing PIN");
        }
    }

    /// <summary>
    /// Reset card PIN
    /// </summary>
    [HttpPost("{cardId}/reset-pin")]
    public async Task<ActionResult<CardPinResetResult>> ResetPin(Guid cardId, [FromBody] CardPinResetRequest request)
    {
        try
        {
            var userId = GetCurrentUserId();
            if (request.CustomerId != userId)
            {
                return Forbid("You can only reset PIN for your own cards");
            }

            request.CardId = cardId;
            var result = await _cardService.ResetPinAsync(request);
            
            if (result.Success)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error resetting PIN for card {CardId}", cardId);
            return StatusCode(500, "An error occurred while resetting PIN");
        }
    }

    /// <summary>
    /// Update merchant category restrictions
    /// </summary>
    [HttpPut("{cardId}/merchant-restrictions")]
    public async Task<ActionResult<CardMerchantRestrictionsResult>> UpdateMerchantRestrictions(
        Guid cardId, 
        [FromBody] CardMerchantRestrictionsRequest request)
    {
        try
        {
            var userId = GetCurrentUserId();
            if (request.CustomerId != userId)
            {
                return Forbid("You can only update restrictions for your own cards");
            }

            request.CardId = cardId;
            var result = await _cardService.UpdateMerchantRestrictionsAsync(request);
            
            if (result.Success)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating merchant restrictions for card {CardId}", cardId);
            return StatusCode(500, "An error occurred while updating merchant restrictions");
        }
    }

    /// <summary>
    /// Update contactless payment settings
    /// </summary>
    [HttpPut("{cardId}/contactless")]
    public async Task<ActionResult<CardContactlessResult>> UpdateContactlessSettings(
        Guid cardId, 
        [FromBody] CardContactlessRequest request)
    {
        try
        {
            var userId = GetCurrentUserId();
            if (request.CustomerId != userId)
            {
                return Forbid("You can only update settings for your own cards");
            }

            request.CardId = cardId;
            var result = await _cardService.UpdateContactlessSettingsAsync(request);
            
            if (result.Success)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating contactless settings for card {CardId}", cardId);
            return StatusCode(500, "An error occurred while updating contactless settings");
        }
    }

    /// <summary>
    /// Update online transactions settings
    /// </summary>
    [HttpPut("{cardId}/online-transactions")]
    public async Task<ActionResult<CardOnlineTransactionsResult>> UpdateOnlineTransactions(
        Guid cardId, 
        [FromBody] CardOnlineTransactionsRequest request)
    {
        try
        {
            var userId = GetCurrentUserId();
            if (request.CustomerId != userId)
            {
                return Forbid("You can only update settings for your own cards");
            }

            request.CardId = cardId;
            var result = await _cardService.UpdateOnlineTransactionsAsync(request);
            
            if (result.Success)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating online transactions settings for card {CardId}", cardId);
            return StatusCode(500, "An error occurred while updating online transactions settings");
        }
    }

    /// <summary>
    /// Update international transactions settings
    /// </summary>
    [HttpPut("{cardId}/international-transactions")]
    public async Task<ActionResult<CardInternationalTransactionsResult>> UpdateInternationalTransactions(
        Guid cardId, 
        [FromBody] CardInternationalTransactionsRequest request)
    {
        try
        {
            var userId = GetCurrentUserId();
            if (request.CustomerId != userId)
            {
                return Forbid("You can only update settings for your own cards");
            }

            request.CardId = cardId;
            var result = await _cardService.UpdateInternationalTransactionsAsync(request);
            
            if (result.Success)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating international transactions settings for card {CardId}", cardId);
            return StatusCode(500, "An error occurred while updating international transactions settings");
        }
    }

    /// <summary>
    /// Get card usage statistics
    /// </summary>
    [HttpGet("{cardId}/usage-stats")]
    public async Task<ActionResult<CardUsageStatsDto>> GetCardUsageStats(
        Guid cardId,
        [FromQuery] DateTime? fromDate = null,
        [FromQuery] DateTime? toDate = null)
    {
        try
        {
            var userId = GetCurrentUserId();
            
            // Default to last 30 days if no dates provided
            var from = fromDate ?? DateTime.UtcNow.AddDays(-30);
            var to = toDate ?? DateTime.UtcNow;

            var stats = await _cardService.GetCardUsageStatsAsync(cardId, userId, from, to);
            
            return Ok(stats);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting usage stats for card {CardId}", cardId);
            return StatusCode(500, "An error occurred while retrieving card usage statistics");
        }
    }

    private Guid GetCurrentUserId() => this.GetCurrentUserIdRequired();
}