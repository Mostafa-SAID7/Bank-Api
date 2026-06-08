using Bank.Api.Helpers;
using Bank.Application.DTOs;
using Bank.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bank.Api.Controllers.Auth;

/// <summary>
/// Controller for account lockout management
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AccountLockoutController : ControllerBase
{
    private readonly IAccountLockoutService _accountLockoutService;

    public AccountLockoutController(IAccountLockoutService accountLockoutService)
    {
        _accountLockoutService = accountLockoutService;
    }

    /// <summary>
    /// Gets lockout status for the current user
    /// </summary>
    [HttpGet("status")]
    public async Task<ActionResult<AccountLockoutInfo>> GetLockoutStatus()
    {
        var userId = this.GetCurrentUserIdRequired();
        var lockout = await _accountLockoutService.GetLockoutStatusAsync(userId);

        if (lockout == null)
        {
            return Ok(new AccountLockoutInfo
            {
                UserId = userId,
                FailedAttempts = 0,
                IsCurrentlyLocked = false
            });
        }

        var lockoutInfo = new AccountLockoutInfo
        {
            UserId = lockout.UserId,
            FailedAttempts = lockout.FailedAttempts,
            LockedUntil = lockout.LockedUntil,
            LockoutReason = lockout.LockoutReason,
            IsCurrentlyLocked = lockout.IsCurrentlyLocked,
            LastFailedAttempt = lockout.LastFailedAttempt,
            LastSuccessfulLogin = lockout.LastSuccessfulLogin,
            CreatedAt = lockout.CreatedAt
        };

        return Ok(lockoutInfo);
    }

    /// <summary>
    /// Gets all locked accounts (admin only)
    /// </summary>
    [HttpGet("locked-accounts")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<List<AccountLockoutInfo>>> GetLockedAccounts()
    {
        var lockouts = await _accountLockoutService.GetLockedAccountsAsync();
        
        var lockoutInfos = lockouts.Select(l => new AccountLockoutInfo
        {
            UserId = l.UserId,
            UserName = l.User?.UserName ?? "Unknown",
            Email = l.User?.Email ?? "Unknown",
            FailedAttempts = l.FailedAttempts,
            LockedUntil = l.LockedUntil,
            LockoutReason = l.LockoutReason,
            IsCurrentlyLocked = l.IsCurrentlyLocked,
            LockoutNotes = l.LockoutNotes,
            LastFailedAttempt = l.LastFailedAttempt,
            LastSuccessfulLogin = l.LastSuccessfulLogin,
            LockedByUserName = l.LockedByUser?.UserName,
            CreatedAt = l.CreatedAt
        }).ToList();

        return Ok(lockoutInfos);
    }

    /// <summary>
    /// Manually locks an account (admin only)
    /// </summary>
    [HttpPost("{userId}/lock")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> LockAccount(Guid userId, [FromBody] LockAccountRequest request)
    {
        var lockedByUserId = this.GetCurrentUserIdRequired();
        var success = await _accountLockoutService.LockAccountAsync(
            userId,
            request.Reason,
            request.LockoutDuration,
            request.Notes,
            lockedByUserId);

        if (!success)
        {
            return this.CreateErrorResponse("Failed to lock account", 400);
        }

        return this.CreateSuccessResponse("Account locked successfully");
    }

    /// <summary>
    /// Manually unlocks an account (admin only)
    /// </summary>
    [HttpPost("{userId}/unlock")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> UnlockAccount(Guid userId)
    {
        var unlockedByUserId = this.GetCurrentUserIdRequired();
        var success = await _accountLockoutService.UnlockAccountAsync(userId, unlockedByUserId);

        if (!success)
        {
            return this.CreateNotFoundResponse("Account not found or not locked");
        }

        return this.CreateSuccessResponse("Account unlocked successfully");
    }

    /// <summary>
    /// Gets lockout statistics (admin only)
    /// </summary>
    [HttpGet("statistics")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<LockoutStatistics>> GetLockoutStatistics()
    {
        var statistics = await _accountLockoutService.GetLockoutStatisticsAsync();
        return Ok(statistics);
    }
}
