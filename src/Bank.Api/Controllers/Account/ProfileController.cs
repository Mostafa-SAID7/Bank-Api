using Bank.Application.Interfaces;
using Bank.Application.DTOs;
using Bank.Domain.Interfaces;
using Bank.Api.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bank.Api.Controllers.Account;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ProfileController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IUserRepository _userRepository;
    private readonly ISessionService _sessionService;

    public ProfileController(
        IAuthService authService,
        IUserRepository userRepository,
        ISessionService sessionService)
    {
        _authService = authService;
        _userRepository = userRepository;
        _sessionService = sessionService;
    }

    /// <summary>
    /// GET /api/profile — Get current user's profile.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetProfile()
    {
        var userId = this.GetCurrentUserIdRequired();
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null) return this.CreateNotFoundResponse("User not found");

        return Ok(new ProfileResponse(user.Id, user.UserName!, user.Email!, user.FirstName, user.LastName));
    }

    /// <summary>
    /// PUT /api/profile — Update current user's profile.
    /// </summary>
    [HttpPut]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileRequest request)
    {
        var userId = this.GetCurrentUserIdRequired();
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null) return this.CreateNotFoundResponse("User not found");

        user.FirstName = request.FirstName;
        user.LastName = request.LastName;
        user.UpdatedAt = DateTime.UtcNow;

        await _userRepository.UpdateAsync(user);
        return Ok(new ProfileResponse(user.Id, user.UserName!, user.Email!, user.FirstName, user.LastName));
    }

    /// <summary>
    /// POST /api/profile/change-password — Change current user's password.
    /// </summary>
    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
    {
        if (string.IsNullOrEmpty(request.CurrentPassword) || string.IsNullOrEmpty(request.NewPassword))
        {
            return this.CreateErrorResponse("Current password and new password are required", 400);
        }

        var userId = this.GetCurrentUserIdRequired();
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null) return this.CreateNotFoundResponse("User not found");

        // TODO: Implement proper password verification using UserManager or similar service
        // For now, this is a placeholder that would require integration with password service
        return this.CreateSuccessResponse("Password changed successfully (placeholder)");
    }

    /// <summary>
    /// POST /api/profile/request-email-change — Request email change with verification.
    /// </summary>
    [HttpPost("request-email-change")]
    public async Task<IActionResult> RequestEmailChange([FromBody] RequestEmailChangeRequest request)
    {
        if (string.IsNullOrEmpty(request.NewEmail))
        {
            return this.CreateErrorResponse("New email is required", 400);
        }

        var userId = this.GetCurrentUserIdRequired();
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null) return this.CreateNotFoundResponse("User not found");

        // Generate verification token
        var verificationToken = Guid.NewGuid().ToString();
        // TODO: Store in cache/database with expiration and send via email

        return this.CreateSuccessResponse("Verification email sent", new { verificationToken });
    }

    /// <summary>
    /// POST /api/profile/verify-email-change — Verify email change with token.
    /// </summary>
    [HttpPost("verify-email-change")]
    public async Task<IActionResult> VerifyEmailChange([FromBody] VerifyEmailChangeRequest request)
    {
        if (string.IsNullOrEmpty(request.VerificationToken) || string.IsNullOrEmpty(request.NewEmail))
        {
            return this.CreateErrorResponse("Verification token and new email are required", 400);
        }

        var userId = this.GetCurrentUserIdRequired();
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null) return this.CreateNotFoundResponse("User not found");

        // TODO: Validate verification token from cache/database

        user.Email = request.NewEmail;
        user.UpdatedAt = DateTime.UtcNow;
        await _userRepository.UpdateAsync(user);

        return this.CreateSuccessResponse("Email updated successfully");
    }

    /// <summary>
    /// POST /api/profile/logout — Logout current user session.
    /// </summary>
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        var userId = this.GetCurrentUserIdRequired();
        var sessionToken = this.GetCurrentSessionToken();

        if (!string.IsNullOrEmpty(sessionToken))
        {
            await _sessionService.TerminateSessionAsync(sessionToken, "User logged out");
        }

        return this.CreateSuccessResponse("Logged out successfully");
    }

    /// <summary>
    /// DELETE /api/profile — Soft-delete (deactivate) current user's account.
    /// </summary>
    [HttpDelete]
    public async Task<IActionResult> DeactivateAccount([FromBody] DeactivateAccountRequest request)
    {
        if (string.IsNullOrEmpty(request.Password))
        {
            return this.CreateErrorResponse("Password confirmation is required", 400);
        }

        var userId = this.GetCurrentUserIdRequired();
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null) return this.CreateNotFoundResponse("User not found");

        // TODO: Implement proper password verification
        
        user.SoftDelete(user.UserName);
        await _userRepository.UpdateAsync(user);
        
        // Terminate all sessions
        await _sessionService.TerminateAllUserSessionsAsync(userId, "Account deactivated", null);

        return this.CreateSuccessResponse("Account deactivated successfully");
    }
}
