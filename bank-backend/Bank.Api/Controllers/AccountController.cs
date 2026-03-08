using Bank.Application.Interfaces;
using Bank.Api.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Bank.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly IAccountService _accountService;

    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    /// <summary>
    /// GET /api/account — List all accounts for the logged-in user.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetMyAccounts()
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var accounts = await _accountService.GetUserAccountsAsync(userId);
        return Ok(accounts);
    }

    /// <summary>
    /// GET /api/account/{id} — Get a specific account by ID.
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetAccountById(Guid id)
    {
        var account = await _accountService.GetAccountByIdAsync(id);
        if (account == null) return NotFound();
        return Ok(account);
    }

    /// <summary>
    /// POST /api/account — Create a new account for the logged-in user.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> CreateAccount([FromBody] CreateAccountRequest request)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var account = await _accountService.CreateAccountAsync(userId, request.AccountHolderName);
        return CreatedAtAction(nameof(GetAccountById), new { id = account.Id }, account);
    }

    /// <summary>
    /// PUT /api/account/{id} — Update an account's holder name.
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAccount(Guid id, [FromBody] UpdateAccountRequest request)
    {
        var account = await _accountService.GetAccountByIdAsync(id);
        if (account == null) return NotFound();

        // Verify ownership
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        if (account.UserId != userId) return Forbid();

        account.AccountHolderName = request.AccountHolderName;
        // In a real app, persist via service  
        return Ok(account);
    }

    /// <summary>
    /// DELETE /api/account/{id} — Delete an account.
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAccount(Guid id)
    {
        var account = await _accountService.GetAccountByIdAsync(id);
        if (account == null) return NotFound();

        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        if (account.UserId != userId) return Forbid();

        // In a real app, delete via service
        return NoContent();
    }
}
