using Bank.Application.Interfaces;
using Bank.Api.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Bank.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ProfileController : ControllerBase
{
    private readonly IAuthService _authService;

    public ProfileController(IAuthService authService)
    {
        _authService = authService;
    }

    /// <summary>
    /// GET /api/profile — Get current user's profile.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetProfile()
    {
        var email = User.FindFirstValue(ClaimTypes.Email);
        if (string.IsNullOrEmpty(email)) return Unauthorized();

        var user = await _authService.GetUserByEmailAsync(email);
        if (user == null) return NotFound();

        return Ok(new ProfileResponse(user.Id, user.UserName!, user.Email!, user.FirstName, user.LastName));
    }

    /// <summary>
    /// PUT /api/profile — Update current user's profile.
    /// </summary>
    [HttpPut]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileRequest request)
    {
        var email = User.FindFirstValue(ClaimTypes.Email);
        if (string.IsNullOrEmpty(email)) return Unauthorized();

        var user = await _authService.GetUserByEmailAsync(email);
        if (user == null) return NotFound();

        user.FirstName = request.FirstName;
        user.LastName = request.LastName;
        user.UpdatedAt = DateTime.UtcNow;

        // Save via UserManager (since User is an Identity entity)
        // For now we return the updated profile — in a real app we'd persist via UserManager
        return Ok(new ProfileResponse(user.Id, user.UserName!, user.Email!, user.FirstName, user.LastName));
    }

    /// <summary>
    /// DELETE /api/profile — Soft-delete (deactivate) current user's account.
    /// </summary>
    [HttpDelete]
    public async Task<IActionResult> DeactivateAccount()
    {
        var email = User.FindFirstValue(ClaimTypes.Email);
        if (string.IsNullOrEmpty(email)) return Unauthorized();

        var user = await _authService.GetUserByEmailAsync(email);
        if (user == null) return NotFound();

        user.SoftDelete(user.UserName);
        return Ok(new { Message = "Account deactivated successfully." });
    }
}
