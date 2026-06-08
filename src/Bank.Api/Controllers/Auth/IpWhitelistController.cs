using Bank.Api.Helpers;
using Bank.Application.DTOs;
using Bank.Application.Interfaces;
using Bank.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bank.Api.Controllers.Auth;

/// <summary>
/// Controller for IP whitelist management
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class IpWhitelistController : ControllerBase
{
    private readonly IIpWhitelistService _ipWhitelistService;

    public IpWhitelistController(IIpWhitelistService ipWhitelistService)
    {
        _ipWhitelistService = ipWhitelistService;
    }

    /// <summary>
    /// Gets all IP whitelist entries
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<List<IpWhitelistInfo>>> GetIpWhitelistEntries([FromQuery] IpWhitelistType? type = null, [FromQuery] bool activeOnly = true)
    {
        var entries = await _ipWhitelistService.GetWhitelistEntriesAsync(type, activeOnly);
        
        var entryInfos = entries.Select(e => new IpWhitelistInfo
        {
            Id = e.Id,
            IpAddress = e.IpAddress,
            IpRange = e.IpRange,
            Type = e.Type,
            Description = e.Description,
            IsActive = e.IsActive,
            ExpiresAt = e.ExpiresAt,
            CreatedByUserName = e.CreatedByUser?.UserName ?? "Unknown",
            ApprovedByUserName = e.ApprovedByUser?.UserName,
            ApprovedAt = e.ApprovedAt,
            CreatedAt = e.CreatedAt
        }).ToList();

        return Ok(entryInfos);
    }

    /// <summary>
    /// Adds an IP address to the whitelist
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<IpWhitelistResult>> AddIpToWhitelist([FromBody] AddIpWhitelistRequest request)
    {
        var userId = this.GetCurrentUserIdRequired();
        var result = await _ipWhitelistService.AddIpToWhitelistAsync(
            request.IpAddress,
            request.Type,
            request.Description,
            userId,
            request.IpRange,
            request.ExpiresAt);

        if (!result.Success)
        {
            return BadRequest(result.ErrorMessage);
        }

        return Ok(result);
    }

    /// <summary>
    /// Approves a pending IP whitelist entry
    /// </summary>
    [HttpPost("{whitelistId}/approve")]
    public async Task<ActionResult> ApproveIpWhitelist(Guid whitelistId, [FromBody] ApproveIpWhitelistRequest request)
    {
        var userId = this.GetCurrentUserIdRequired();
        var success = await _ipWhitelistService.ApproveIpWhitelistAsync(whitelistId, userId, request.Notes);

        if (!success)
        {
            return this.CreateNotFoundResponse("IP whitelist entry not found");
        }

        return this.CreateSuccessResponse("IP whitelist entry approved successfully");
    }

    /// <summary>
    /// Revokes an IP whitelist entry
    /// </summary>
    [HttpDelete("{whitelistId}")]
    public async Task<ActionResult> RevokeIpWhitelist(Guid whitelistId)
    {
        var success = await _ipWhitelistService.RevokeIpWhitelistAsync(whitelistId);

        if (!success)
        {
            return this.CreateNotFoundResponse("IP whitelist entry not found");
        }

        return this.CreateSuccessResponse("IP whitelist entry revoked successfully");
    }

    /// <summary>
    /// Gets pending IP whitelist approvals
    /// </summary>
    [HttpGet("pending")]
    public async Task<ActionResult<List<IpWhitelistInfo>>> GetPendingApprovals()
    {
        var entries = await _ipWhitelistService.GetPendingApprovalsAsync();
        
        var entryInfos = entries.Select(e => new IpWhitelistInfo
        {
            Id = e.Id,
            IpAddress = e.IpAddress,
            IpRange = e.IpRange,
            Type = e.Type,
            Description = e.Description,
            IsActive = e.IsActive,
            ExpiresAt = e.ExpiresAt,
            CreatedByUserName = e.CreatedByUser?.UserName ?? "Unknown",
            CreatedAt = e.CreatedAt
        }).ToList();

        return Ok(entryInfos);
    }
}
