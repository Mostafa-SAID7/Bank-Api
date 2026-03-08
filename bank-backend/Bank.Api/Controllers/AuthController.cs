using Bank.Application.Interfaces;
using Bank.Application.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Bank.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        try
        {
            var token = await _authService.LoginAsync(request.Email, request.Password);
            return Ok(new AuthResponse(token));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        try
        {
            var user = await _authService.RegisterAsync(request.Username, request.Email, request.Password);
            return Ok(new { user.Id, user.UserName, user.Email });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
