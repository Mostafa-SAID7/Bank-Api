using Bank.Api.Helpers;
using Bank.Application.DTOs;
using Bank.Application.Interfaces;
using Bank.Application.Services;
using Bank.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Bank.Api.Controllers;

/// <summary>
/// Controller for managing deposit products and fixed deposits
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DepositController : ControllerBase
{
    private readonly IDepositService _depositService;
    private readonly ILogger<DepositController> _logger;

    public DepositController(IDepositService depositService, ILogger<DepositController> logger)
    {
        _depositService = depositService;
        _logger = logger;
    }

    #region Deposit Products

    /// <summary>
    /// Get all active deposit products
    /// </summary>
    [HttpGet("products")]
    public async Task<ActionResult<IEnumerable<DepositProductDto>>> GetDepositProducts()
    {
        try
        {
            var products = await _depositService.GetActiveDepositProductsAsync();
            return Ok(products);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving deposit products");
            return StatusCode(500, "An error occurred while retrieving deposit products");
        }
    }

    /// <summary>
    /// Get deposit products by type
    /// </summary>
    [HttpGet("products/type/{productType}")]
    public async Task<ActionResult<IEnumerable<DepositProductDto>>> GetDepositProductsByType(DepositProductType productType)
    {
        try
        {
            var products = await _depositService.GetDepositProductsByTypeAsync(productType);
            return Ok(products);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving deposit products by type {ProductType}", productType);
            return StatusCode(500, "An error occurred while retrieving deposit products");
        }
    }

    /// <summary>
    /// Get deposit product by ID
    /// </summary>
    [HttpGet("products/{productId}")]
    public async Task<ActionResult<DepositProductDto>> GetDepositProduct(Guid productId)
    {
        try
        {
            var product = await _depositService.GetDepositProductAsync(productId);
            if (product == null)
                return NotFound($"Deposit product {productId} not found");

            return Ok(product);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving deposit product {ProductId}", productId);
            return StatusCode(500, "An error occurred while retrieving the deposit product");
        }
    }

    /// <summary>
    /// Create a new deposit product (Admin only)
    /// </summary>
    [HttpPost("products")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<DepositProductDto>> CreateDepositProduct([FromBody] CreateDepositProductRequest request)
    {
        try
        {
            var userId = this.GetCurrentUserId();
            var product = await _depositService.CreateDepositProductAsync(request, userId);
            return CreatedAtAction(nameof(GetDepositProduct), new { productId = product.Id }, product);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating deposit product");
            return StatusCode(500, "An error occurred while creating the deposit product");
        }
    }

    /// <summary>
    /// Update deposit product (Admin only)
    /// </summary>
    [HttpPut("products/{productId}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<DepositProductDto>> UpdateDepositProduct(Guid productId, [FromBody] UpdateDepositProductRequest request)
    {
        try
        {
            var userId = this.GetCurrentUserId();
            var product = await _depositService.UpdateDepositProductAsync(productId, request, userId);
            return Ok(product);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating deposit product {ProductId}", productId);
            return StatusCode(500, "An error occurred while updating the deposit product");
        }
    }

    /// <summary>
    /// Deactivate deposit product (Admin only)
    /// </summary>
    [HttpDelete("products/{productId}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> DeactivateDepositProduct(Guid productId)
    {
        try
        {
            var userId = this.GetCurrentUserId();
            var success = await _depositService.DeactivateDepositProductAsync(productId, userId);
            if (!success)
                return NotFound($"Deposit product {productId} not found");

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deactivating deposit product {ProductId}", productId);
            return StatusCode(500, "An error occurred while deactivating the deposit product");
        }
    }

    #endregion

    #region Interest Tiers

    /// <summary>
    /// Get interest tiers for a deposit product
    /// </summary>
    [HttpGet("products/{productId}/tiers")]
    public async Task<ActionResult<IEnumerable<InterestTierDto>>> GetInterestTiers(Guid productId)
    {
        try
        {
            var tiers = await _depositService.GetInterestTiersAsync(productId);
            return Ok(tiers);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving interest tiers for product {ProductId}", productId);
            return StatusCode(500, "An error occurred while retrieving interest tiers");
        }
    }

    /// <summary>
    /// Create interest tier (Admin only)
    /// </summary>
    [HttpPost("products/{productId}/tiers")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<InterestTierDto>> CreateInterestTier(Guid productId, [FromBody] CreateInterestTierRequest request)
    {
        try
        {
            var userId = this.GetCurrentUserId();
            var tier = await _depositService.CreateInterestTierAsync(productId, request, userId);
            return CreatedAtAction(nameof(GetInterestTiers), new { productId }, tier);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating interest tier for product {ProductId}", productId);
            return StatusCode(500, "An error occurred while creating the interest tier");
        }
    }

    /// <summary>
    /// Update interest tier (Admin only)
    /// </summary>
    [HttpPut("tiers/{tierId}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<InterestTierDto>> UpdateInterestTier(Guid tierId, [FromBody] UpdateInterestTierRequest request)
    {
        try
        {
            var userId = this.GetCurrentUserId();
            var tier = await _depositService.UpdateInterestTierAsync(tierId, request, userId);
            return Ok(tier);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating interest tier {TierId}", tierId);
            return StatusCode(500, "An error occurred while updating the interest tier");
        }
    }

    /// <summary>
    /// Delete interest tier (Admin only)
    /// </summary>
    [HttpDelete("tiers/{tierId}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> DeleteInterestTier(Guid tierId)
    {
        try
        {
            var userId = this.GetCurrentUserId();
            var success = await _depositService.DeleteInterestTierAsync(tierId, userId);
            if (!success)
                return NotFound($"Interest tier {tierId} not found");

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting interest tier {TierId}", tierId);
            return StatusCode(500, "An error occurred while deleting the interest tier");
        }
    }

    #endregion
    #region Fixed Deposits

    /// <summary>
    /// Create a new fixed deposit
    /// </summary>
    [HttpPost("fixed-deposits")]
    public async Task<ActionResult<FixedDepositDto>> CreateFixedDeposit([FromBody] CreateFixedDepositRequest request)
    {
        try
        {
            var customerId = this.GetCurrentUserId();
            var deposit = await _depositService.CreateFixedDepositAsync(request, customerId);
            return CreatedAtAction(nameof(GetFixedDeposit), new { depositId = deposit.Id }, deposit);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating fixed deposit");
            return StatusCode(500, "An error occurred while creating the fixed deposit");
        }
    }

    /// <summary>
    /// Get fixed deposit by ID
    /// </summary>
    [HttpGet("fixed-deposits/{depositId}")]
    public async Task<ActionResult<FixedDepositDto>> GetFixedDeposit(Guid depositId)
    {
        try
        {
            var deposit = await _depositService.GetFixedDepositAsync(depositId);
            if (deposit == null)
                return NotFound($"Fixed deposit {depositId} not found");

            // Ensure user can only access their own deposits (unless admin)
            var userId = this.GetCurrentUserId();
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
            if (userRole != "Admin" && deposit.CustomerId != userId)
                return Forbid("You can only access your own deposits");

            return Ok(deposit);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving fixed deposit {DepositId}", depositId);
            return StatusCode(500, "An error occurred while retrieving the fixed deposit");
        }
    }

    /// <summary>
    /// Get fixed deposit by deposit number
    /// </summary>
    [HttpGet("fixed-deposits/number/{depositNumber}")]
    public async Task<ActionResult<FixedDepositDto>> GetFixedDepositByNumber(string depositNumber)
    {
        try
        {
            var deposit = await _depositService.GetFixedDepositByNumberAsync(depositNumber);
            if (deposit == null)
                return NotFound($"Fixed deposit {depositNumber} not found");

            // Ensure user can only access their own deposits (unless admin)
            var userId = this.GetCurrentUserId();
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
            if (userRole != "Admin" && deposit.CustomerId != userId)
                return Forbid("You can only access your own deposits");

            return Ok(deposit);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving fixed deposit by number {DepositNumber}", depositNumber);
            return StatusCode(500, "An error occurred while retrieving the fixed deposit");
        }
    }

    /// <summary>
    /// Get customer's fixed deposits
    /// </summary>
    [HttpGet("fixed-deposits")]
    public async Task<ActionResult<IEnumerable<FixedDepositDto>>> GetCustomerFixedDeposits()
    {
        try
        {
            var customerId = this.GetCurrentUserId();
            var deposits = await _depositService.GetCustomerFixedDepositsAsync(customerId);
            return Ok(deposits);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving customer fixed deposits");
            return StatusCode(500, "An error occurred while retrieving fixed deposits");
        }
    }

    /// <summary>
    /// Get maturing deposits (Admin only)
    /// </summary>
    [HttpGet("fixed-deposits/maturing")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<IEnumerable<FixedDepositDto>>> GetMaturingDeposits([FromQuery] DateTime? fromDate, [FromQuery] DateTime? toDate)
    {
        try
        {
            var from = fromDate ?? DateTime.UtcNow;
            var to = toDate ?? DateTime.UtcNow.AddDays(30);
            
            var deposits = await _depositService.GetMaturingDepositsAsync(from, to);
            return Ok(deposits);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving maturing deposits");
            return StatusCode(500, "An error occurred while retrieving maturing deposits");
        }
    }

    #endregion

    #region Maturity and Renewal

    /// <summary>
    /// Get maturity details for a fixed deposit
    /// </summary>
    [HttpGet("fixed-deposits/{depositId}/maturity")]
    public async Task<ActionResult<MaturityDetailsDto>> GetMaturityDetails(Guid depositId)
    {
        try
        {
            var details = await _depositService.GetMaturityDetailsAsync(depositId);
            
            // Ensure user can only access their own deposits (unless admin)
            var userId = this.GetCurrentUserId();
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
            var deposit = await _depositService.GetFixedDepositAsync(depositId);
            if (userRole != "Admin" && deposit?.CustomerId != userId)
                return Forbid("You can only access your own deposits");

            return Ok(details);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving maturity details for deposit {DepositId}", depositId);
            return StatusCode(500, "An error occurred while retrieving maturity details");
        }
    }

    /// <summary>
    /// Process maturity action for a fixed deposit
    /// </summary>
    [HttpPost("fixed-deposits/{depositId}/maturity")]
    public async Task<ActionResult> ProcessMaturity(Guid depositId, [FromBody] MaturityAction action)
    {
        try
        {
            var userId = this.GetCurrentUserId();
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
            
            // Ensure user can only process their own deposits (unless admin)
            if (userRole != "Admin")
            {
                var deposit = await _depositService.GetFixedDepositAsync(depositId);
                if (deposit?.CustomerId != userId)
                    return Forbid("You can only process your own deposits");
            }

            var success = await _depositService.ProcessMaturityAsync(depositId, action, userId);
            if (!success)
                return BadRequest("Unable to process maturity action");

            return Ok(new { message = "Maturity action processed successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing maturity for deposit {DepositId}", depositId);
            return StatusCode(500, "An error occurred while processing maturity");
        }
    }

    /// <summary>
    /// Renew a fixed deposit
    /// </summary>
    [HttpPost("fixed-deposits/{depositId}/renew")]
    public async Task<ActionResult<FixedDepositDto>> RenewFixedDeposit(Guid depositId, [FromBody] RenewDepositRequest request)
    {
        try
        {
            var userId = this.GetCurrentUserId();
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
            
            // Ensure user can only renew their own deposits (unless admin)
            if (userRole != "Admin")
            {
                var deposit = await _depositService.GetFixedDepositAsync(depositId);
                if (deposit?.CustomerId != userId)
                    return Forbid("You can only renew your own deposits");
            }

            var renewedDeposit = await _depositService.RenewFixedDepositAsync(depositId, request, userId);
            return Ok(renewedDeposit);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error renewing fixed deposit {DepositId}", depositId);
            return StatusCode(500, "An error occurred while renewing the fixed deposit");
        }
    }

    #endregion

    #region Withdrawals

    /// <summary>
    /// Calculate early withdrawal details
    /// </summary>
    [HttpPost("fixed-deposits/{depositId}/withdrawal/calculate")]
    public async Task<ActionResult<WithdrawalDetailsDto>> CalculateEarlyWithdrawal(Guid depositId, [FromBody] decimal withdrawalAmount)
    {
        try
        {
            var userId = this.GetCurrentUserId();
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
            
            // Ensure user can only access their own deposits (unless admin)
            if (userRole != "Admin")
            {
                var deposit = await _depositService.GetFixedDepositAsync(depositId);
                if (deposit?.CustomerId != userId)
                    return Forbid("You can only access your own deposits");
            }

            var details = await _depositService.CalculateEarlyWithdrawalAsync(depositId, withdrawalAmount);
            return Ok(details);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calculating early withdrawal for deposit {DepositId}", depositId);
            return StatusCode(500, "An error occurred while calculating withdrawal details");
        }
    }

    /// <summary>
    /// Process early withdrawal
    /// </summary>
    [HttpPost("fixed-deposits/{depositId}/withdrawal/early")]
    public async Task<ActionResult> ProcessEarlyWithdrawal(Guid depositId, [FromBody] EarlyWithdrawalRequest request)
    {
        try
        {
            var userId = this.GetCurrentUserId();
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
            
            // Ensure user can only withdraw from their own deposits (unless admin)
            if (userRole != "Admin")
            {
                var deposit = await _depositService.GetFixedDepositAsync(depositId);
                if (deposit?.CustomerId != userId)
                    return Forbid("You can only withdraw from your own deposits");
            }

            var success = await _depositService.ProcessEarlyWithdrawalAsync(depositId, request, userId);
            if (!success)
                return BadRequest("Unable to process early withdrawal");

            return Ok(new { message = "Early withdrawal processed successfully" });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing early withdrawal for deposit {DepositId}", depositId);
            return StatusCode(500, "An error occurred while processing early withdrawal");
        }
    }

    /// <summary>
    /// Process partial withdrawal
    /// </summary>
    [HttpPost("fixed-deposits/{depositId}/withdrawal/partial")]
    public async Task<ActionResult> ProcessPartialWithdrawal(Guid depositId, [FromBody] PartialWithdrawalRequest request)
    {
        try
        {
            var userId = this.GetCurrentUserId();
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
            
            // Ensure user can only withdraw from their own deposits (unless admin)
            if (userRole != "Admin")
            {
                var deposit = await _depositService.GetFixedDepositAsync(depositId);
                if (deposit?.CustomerId != userId)
                    return Forbid("You can only withdraw from your own deposits");
            }

            var success = await _depositService.ProcessPartialWithdrawalAsync(depositId, request, userId);
            if (!success)
                return BadRequest("Unable to process partial withdrawal");

            return Ok(new { message = "Partial withdrawal processed successfully" });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing partial withdrawal for deposit {DepositId}", depositId);
            return StatusCode(500, "An error occurred while processing partial withdrawal");
        }
    }

    #endregion
    #region Certificates and Notices

    /// <summary>
    /// Generate deposit certificate
    /// </summary>
    [HttpPost("fixed-deposits/{depositId}/certificate")]
    public async Task<ActionResult<DepositCertificateDto>> GenerateCertificate(Guid depositId)
    {
        try
        {
            var userId = this.GetCurrentUserId();
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
            
            // Ensure user can only generate certificates for their own deposits (unless admin)
            if (userRole != "Admin")
            {
                var deposit = await _depositService.GetFixedDepositAsync(depositId);
                if (deposit?.CustomerId != userId)
                    return Forbid("You can only generate certificates for your own deposits");
            }

            var certificate = await _depositService.GenerateCertificateAsync(depositId, userId);
            return Ok(certificate);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating certificate for deposit {DepositId}", depositId);
            return StatusCode(500, "An error occurred while generating the certificate");
        }
    }

    /// <summary>
    /// Get certificate by ID
    /// </summary>
    [HttpGet("certificates/{certificateId}")]
    public async Task<ActionResult<DepositCertificateDto>> GetCertificate(Guid certificateId)
    {
        try
        {
            var certificate = await _depositService.GetCertificateAsync(certificateId);
            if (certificate == null)
                return NotFound($"Certificate {certificateId} not found");

            // Ensure user can only access certificates for their own deposits (unless admin)
            var userId = this.GetCurrentUserId();
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
            if (userRole != "Admin")
            {
                var deposit = await _depositService.GetFixedDepositAsync(certificate.FixedDepositId);
                if (deposit?.CustomerId != userId)
                    return Forbid("You can only access certificates for your own deposits");
            }

            return Ok(certificate);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving certificate {CertificateId}", certificateId);
            return StatusCode(500, "An error occurred while retrieving the certificate");
        }
    }

    /// <summary>
    /// Download certificate PDF
    /// </summary>
    [HttpGet("certificates/{certificateId}/pdf")]
    public async Task<ActionResult> GetCertificatePdf(Guid certificateId)
    {
        try
        {
            var certificate = await _depositService.GetCertificateAsync(certificateId);
            if (certificate == null)
                return NotFound($"Certificate {certificateId} not found");

            // Ensure user can only download certificates for their own deposits (unless admin)
            var userId = this.GetCurrentUserId();
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
            if (userRole != "Admin")
            {
                var deposit = await _depositService.GetFixedDepositAsync(certificate.FixedDepositId);
                if (deposit?.CustomerId != userId)
                    return Forbid("You can only download certificates for your own deposits");
            }

            var pdfBytes = await _depositService.GetCertificatePdfAsync(certificateId);
            return File(pdfBytes, "application/pdf", $"certificate_{certificate.CertificateNumber}.pdf");
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error downloading certificate PDF {CertificateId}", certificateId);
            return StatusCode(500, "An error occurred while downloading the certificate");
        }
    }

    #endregion

    #region Reporting and Analytics

    /// <summary>
    /// Get deposit summary for customer
    /// </summary>
    [HttpGet("summary")]
    public async Task<ActionResult<DepositSummaryDto>> GetDepositSummary()
    {
        try
        {
            var customerId = this.GetCurrentUserId();
            var summary = await _depositService.GetDepositSummaryAsync(customerId);
            return Ok(summary);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving deposit summary");
            return StatusCode(500, "An error occurred while retrieving deposit summary");
        }
    }

    /// <summary>
    /// Get deposit transactions
    /// </summary>
    [HttpGet("fixed-deposits/{depositId}/transactions")]
    public async Task<ActionResult<IEnumerable<DepositTransactionDto>>> GetDepositTransactions(
        Guid depositId, 
        [FromQuery] DateTime? fromDate, 
        [FromQuery] DateTime? toDate)
    {
        try
        {
            var userId = this.GetCurrentUserId();
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
            
            // Ensure user can only access transactions for their own deposits (unless admin)
            if (userRole != "Admin")
            {
                var deposit = await _depositService.GetFixedDepositAsync(depositId);
                if (deposit?.CustomerId != userId)
                    return Forbid("You can only access transactions for your own deposits");
            }

            var transactions = await _depositService.GetDepositTransactionsAsync(depositId, fromDate, toDate);
            return Ok(transactions);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving deposit transactions for {DepositId}", depositId);
            return StatusCode(500, "An error occurred while retrieving deposit transactions");
        }
    }

    /// <summary>
    /// Get customer deposit portfolio
    /// </summary>
    [HttpGet("portfolio")]
    public async Task<ActionResult<DepositPortfolioDto>> GetDepositPortfolio()
    {
        try
        {
            var customerId = this.GetCurrentUserId();
            var portfolio = await _depositService.GetCustomerDepositPortfolioAsync(customerId);
            return Ok(portfolio);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving deposit portfolio");
            return StatusCode(500, "An error occurred while retrieving deposit portfolio");
        }
    }

    #endregion

    #region Admin Operations

    /// <summary>
    /// Process interest for a specific deposit (Admin only)
    /// </summary>
    [HttpPost("fixed-deposits/{depositId}/interest")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> ProcessInterest(Guid depositId)
    {
        try
        {
            var userId = this.GetCurrentUserId();
            var success = await _depositService.ProcessInterestCreditAsync(depositId, userId);
            if (!success)
                return BadRequest("Unable to process interest");

            return Ok(new { message = "Interest processed successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing interest for deposit {DepositId}", depositId);
            return StatusCode(500, "An error occurred while processing interest");
        }
    }

    /// <summary>
    /// Process daily interest for all deposits (Admin only)
    /// </summary>
    [HttpPost("interest/daily")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> ProcessDailyInterest()
    {
        try
        {
            var success = await _depositService.ProcessDailyInterestAsync();
            return Ok(new { message = "Daily interest processing completed", success });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing daily interest");
            return StatusCode(500, "An error occurred while processing daily interest");
        }
    }

    /// <summary>
    /// Process auto-renewals (Admin only)
    /// </summary>
    [HttpPost("auto-renewals")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> ProcessAutoRenewals()
    {
        try
        {
            var success = await _depositService.ProcessAutoRenewalsAsync();
            return Ok(new { message = "Auto-renewals processing completed", success });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing auto-renewals");
            return StatusCode(500, "An error occurred while processing auto-renewals");
        }
    }

    /// <summary>
    /// Send maturity notices (Admin only)
    /// </summary>
    [HttpPost("maturity-notices")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> SendMaturityNotices()
    {
        try
        {
            var success = await _depositService.SendMaturityNoticesAsync();
            return Ok(new { message = "Maturity notices processing completed", success });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending maturity notices");
            return StatusCode(500, "An error occurred while sending maturity notices");
        }
    }

    #endregion

    #region Enhanced Withdrawal Management

    /// <summary>
    /// Get detailed withdrawal calculation with penalty breakdown
    /// </summary>
    [HttpPost("fixed-deposits/{depositId}/withdrawal/detailed-calculation")]
    public async Task<ActionResult<DetailedWithdrawalCalculation>> GetDetailedWithdrawalCalculation(Guid depositId, [FromBody] decimal withdrawalAmount)
    {
        try
        {
            var userId = this.GetCurrentUserId();
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
            
            // Ensure user can only access their own deposits (unless admin)
            if (userRole != "Admin")
            {
                var deposit = await _depositService.GetFixedDepositAsync(depositId);
                if (deposit?.CustomerId != userId)
                    return Forbid("You can only access your own deposits");
            }

            // Assuming we have a withdrawal service injected
            var withdrawalService = HttpContext.RequestServices.GetRequiredService<IDepositWithdrawalService>();
            var calculation = await withdrawalService.CalculateDetailedWithdrawalAsync(depositId, withdrawalAmount);
            return Ok(calculation);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calculating detailed withdrawal for deposit {DepositId}", depositId);
            return StatusCode(500, "An error occurred while calculating withdrawal details");
        }
    }

    /// <summary>
    /// Get penalty-free periods for a deposit
    /// </summary>
    [HttpGet("fixed-deposits/{depositId}/penalty-free-periods")]
    public async Task<ActionResult<PenaltyFreePeriodsDto>> GetPenaltyFreePeriods(Guid depositId)
    {
        try
        {
            var userId = this.GetCurrentUserId();
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
            
            // Ensure user can only access their own deposits (unless admin)
            if (userRole != "Admin")
            {
                var deposit = await _depositService.GetFixedDepositAsync(depositId);
                if (deposit?.CustomerId != userId)
                    return Forbid("You can only access your own deposits");
            }

            var withdrawalService = HttpContext.RequestServices.GetRequiredService<IDepositWithdrawalService>();
            var periods = await withdrawalService.GetPenaltyFreePeriodsAsync(depositId);
            return Ok(periods);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving penalty-free periods for deposit {DepositId}", depositId);
            return StatusCode(500, "An error occurred while retrieving penalty-free periods");
        }
    }

    /// <summary>
    /// Get withdrawal history for a deposit
    /// </summary>
    [HttpGet("fixed-deposits/{depositId}/withdrawal-history")]
    public async Task<ActionResult<IEnumerable<WithdrawalHistoryDto>>> GetWithdrawalHistory(Guid depositId)
    {
        try
        {
            var userId = this.GetCurrentUserId();
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
            
            // Ensure user can only access their own deposits (unless admin)
            if (userRole != "Admin")
            {
                var deposit = await _depositService.GetFixedDepositAsync(depositId);
                if (deposit?.CustomerId != userId)
                    return Forbid("You can only access your own deposits");
            }

            var withdrawalService = HttpContext.RequestServices.GetRequiredService<IDepositWithdrawalService>();
            var history = await withdrawalService.GetWithdrawalHistoryAsync(depositId);
            return Ok(history);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving withdrawal history for deposit {DepositId}", depositId);
            return StatusCode(500, "An error occurred while retrieving withdrawal history");
        }
    }

    #endregion

    #region Enhanced Maturity Management

    /// <summary>
    /// Process customer consent for auto-renewal
    /// </summary>
    [HttpPost("fixed-deposits/{depositId}/consent")]
    public async Task<ActionResult> ProcessCustomerConsent(Guid depositId, [FromBody] CustomerConsentRequest request)
    {
        try
        {
            var userId = this.GetCurrentUserId();
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
            
            // Ensure user can only process consent for their own deposits (unless admin)
            if (userRole != "Admin")
            {
                var deposit = await _depositService.GetFixedDepositAsync(depositId);
                if (deposit?.CustomerId != userId)
                    return Forbid("You can only process consent for your own deposits");
            }

            var maturityService = HttpContext.RequestServices.GetRequiredService<IDepositMaturityService>();
            var success = await maturityService.ProcessCustomerConsentAsync(depositId, request.ConsentGiven, request.PreferredAction);
            
            if (!success)
                return BadRequest("Unable to process customer consent");

            return Ok(new { message = "Customer consent processed successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing customer consent for deposit {DepositId}", depositId);
            return StatusCode(500, "An error occurred while processing customer consent");
        }
    }

    /// <summary>
    /// Get maturity reminders for approaching deposits (Admin only)
    /// </summary>
    [HttpGet("maturity-reminders")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<IEnumerable<MaturityReminderDto>>> GetMaturityReminders([FromQuery] int? daysAhead = 30)
    {
        try
        {
            var fromDate = DateTime.UtcNow;
            var toDate = DateTime.UtcNow.AddDays(daysAhead ?? 30);
            
            var maturingDeposits = await _depositService.GetMaturingDepositsAsync(fromDate, toDate);
            
            var reminders = maturingDeposits.Select(d => new MaturityReminderDto
            {
                DepositId = d.Id,
                DepositNumber = d.DepositNumber,
                CustomerName = d.CustomerName,
                MaturityDate = d.MaturityDate,
                DaysToMaturity = d.DaysToMaturity,
                MaturityAmount = d.MaturityAmount,
                AutoRenewalEnabled = d.AutoRenewalEnabled,
                CustomerConsentReceived = d.CustomerConsentReceived,
                DefaultAction = d.MaturityAction
            });

            return Ok(reminders);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving maturity reminders");
            return StatusCode(500, "An error occurred while retrieving maturity reminders");
        }
    }

    /// <summary>
    /// Process all approaching maturity deposits (Admin only)
    /// </summary>
    [HttpPost("maturity/process-approaching")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<MaturityProcessingResult>> ProcessApproachingMaturity()
    {
        try
        {
            var maturityService = HttpContext.RequestServices.GetRequiredService<IDepositMaturityService>();
            var result = await maturityService.ProcessApproachingMaturityAsync();
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing approaching maturity deposits");
            return StatusCode(500, "An error occurred while processing approaching maturity deposits");
        }
    }

    /// <summary>
    /// Process all matured deposits (Admin only)
    /// </summary>
    [HttpPost("maturity/process-matured")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<MaturityProcessingResult>> ProcessMaturedDeposits()
    {
        try
        {
            var maturityService = HttpContext.RequestServices.GetRequiredService<IDepositMaturityService>();
            var result = await maturityService.ProcessMaturedDepositsAsync();
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing matured deposits");
            return StatusCode(500, "An error occurred while processing matured deposits");
        }
    }

    /// <summary>
    /// Send renewal reminders (Admin only)
    /// </summary>
    [HttpPost("maturity/send-reminders")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> SendRenewalReminders()
    {
        try
        {
            var maturityService = HttpContext.RequestServices.GetRequiredService<IDepositMaturityService>();
            var sentCount = await maturityService.SendRenewalRemindersAsync();
            return Ok(new { message = $"Sent {sentCount} renewal reminders" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending renewal reminders");
            return StatusCode(500, "An error occurred while sending renewal reminders");
        }
    }

    /// <summary>
    /// Get auto-renewal summary (Admin only)
    /// </summary>
    [HttpGet("auto-renewal-summary")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<AutoRenewalSummaryDto>> GetAutoRenewalSummary()
    {
        try
        {
            var maturingDeposits = await _depositService.GetMaturingDepositsAsync(DateTime.UtcNow, DateTime.UtcNow.AddDays(30));
            
            var summary = new AutoRenewalSummaryDto
            {
                TotalEligible = maturingDeposits.Count(d => d.AutoRenewalEnabled),
                PendingConsent = maturingDeposits.Count(d => d.AutoRenewalEnabled && !d.CustomerConsentReceived),
                TotalRenewedAmount = maturingDeposits.Where(d => d.AutoRenewalEnabled && d.CustomerConsentReceived).Sum(d => d.MaturityAmount)
            };

            return Ok(summary);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving auto-renewal summary");
            return StatusCode(500, "An error occurred while retrieving auto-renewal summary");
        }
    }

    #endregion
}
