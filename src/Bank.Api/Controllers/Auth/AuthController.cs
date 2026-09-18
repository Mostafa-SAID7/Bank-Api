using Bank.Application.Commands.Auth;
using Bank.Application.DTOs.Auth.Core;
using Bank.Api.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bank.Api.Controllers.Auth;

[ApiController]
[Route("api/[controller]")]
public class AuthController : BaseApiController
{
    private string GetIpAddress()
    {
        if (Request.Headers.ContainsKey("X-Forwarded-For"))
            return Request.Headers["X-Forwarded-For"].ToString();
        return HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString() ?? "unknown";
    }

    private string GetUserAgent()
    {
        return Request.Headers["User-Agent"].ToString();
    }

    /// <summary>
    /// Login with email and password
    /// </summary>
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var command = new LoginCommand(request.Email, request.Password, GetIpAddress(), GetUserAgent());
        var result = await Mediator.Send(command);

        if (result.IsSuccess)
        {
            if (result.Value.RequiresTwoFactor)
            {
                return Ok(new { 
                    requiresTwoFactor = true, 
                    challengeToken = result.Value.ChallengeToken,
                    message = "MFA verification required." 
                });
            }
            return this.CreateSuccessResponse("Login successful", result.Value);
        }

        return Unauthorized(new { message = result.ErrorMessage });
    }

    /// <summary>
    /// Register a new user
    /// </summary>
    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var command = new RegisterCommand(request.Username, request.Email, request.Password, GetIpAddress());
        var result = await Mediator.Send(command);

        if (result.IsSuccess)
        {
            return this.CreateSuccessResponse("Registration successful", result.Value);
        }

        return BadRequest(new { message = result.ErrorMessage });
    }

    /// <summary>
    /// Verify MFA token
    /// </summary>
    [HttpPost("verify-mfa")]
    [AllowAnonymous]
    public async Task<IActionResult> VerifyMfa([FromBody] VerifyMfaRequest request)
    {
        var command = new VerifyMfaCommand(request.ChallengeToken, request.Code, request.Email, GetIpAddress(), GetUserAgent());
        var result = await Mediator.Send(command);

        if (result.IsSuccess)
        {
            return this.CreateSuccessResponse("Login successful", result.Value);
        }

        return Unauthorized(new { message = result.ErrorMessage });
    }
}
