using Bank.Api.Helpers;
using Bank.Application.DTOs;
using Bank.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bank.Api.Controllers.Auth;

/// <summary>
/// Controller for password policy management and validation
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PasswordPolicyController : ControllerBase
{
    private readonly IPasswordPolicyService _passwordPolicyService;

    public PasswordPolicyController(IPasswordPolicyService passwordPolicyService)
    {
        _passwordPolicyService = passwordPolicyService;
    }

    /// <summary>
    /// Gets all active password policies (admin only)
    /// </summary>
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<List<PasswordPolicyInfo>>> GetPasswordPolicies()
    {
        var policies = await _passwordPolicyService.GetActivePasswordPoliciesAsync();
        
        var policyInfos = policies.Select(p => new PasswordPolicyInfo
        {
            Id = p.Id,
            Name = p.Name,
            ComplexityLevel = p.ComplexityLevel,
            MinimumLength = p.MinimumLength,
            MaximumLength = p.MaximumLength,
            RequireUppercase = p.RequireUppercase,
            RequireLowercase = p.RequireLowercase,
            RequireDigits = p.RequireDigits,
            RequireSpecialCharacters = p.RequireSpecialCharacters,
            MinimumUniqueCharacters = p.MinimumUniqueCharacters,
            PasswordHistoryCount = p.PasswordHistoryCount,
            MaxPasswordAge = p.MaxPasswordAge,
            MaxFailedAttempts = p.MaxFailedAttempts,
            LockoutDuration = p.LockoutDuration,
            IsDefault = p.IsDefault,
            IsActive = p.IsActive,
            Description = p.Description
        }).ToList();

        return Ok(policyInfos);
    }

    /// <summary>
    /// Gets the default password policy (public)
    /// </summary>
    [HttpGet("default")]
    [AllowAnonymous]
    public async Task<ActionResult<PasswordPolicyInfo>> GetDefaultPasswordPolicy()
    {
        var policy = await _passwordPolicyService.GetDefaultPasswordPolicyAsync();
        
        if (policy == null)
        {
            return this.CreateNotFoundResponse("No default password policy found");
        }

        var policyInfo = new PasswordPolicyInfo
        {
            Id = policy.Id,
            Name = policy.Name,
            ComplexityLevel = policy.ComplexityLevel,
            MinimumLength = policy.MinimumLength,
            MaximumLength = policy.MaximumLength,
            RequireUppercase = policy.RequireUppercase,
            RequireLowercase = policy.RequireLowercase,
            RequireDigits = policy.RequireDigits,
            RequireSpecialCharacters = policy.RequireSpecialCharacters,
            MinimumUniqueCharacters = policy.MinimumUniqueCharacters,
            PasswordHistoryCount = policy.PasswordHistoryCount,
            MaxPasswordAge = policy.MaxPasswordAge,
            MaxFailedAttempts = policy.MaxFailedAttempts,
            LockoutDuration = policy.LockoutDuration,
            IsDefault = policy.IsDefault,
            IsActive = policy.IsActive,
            Description = policy.Description
        };

        return Ok(policyInfo);
    }

    /// <summary>
    /// Validates a password against the current policy (does NOT expose validation details publicly)
    /// </summary>
    [HttpPost("validate")]
    [AllowAnonymous]
    public async Task<ActionResult<PasswordValidationResult>> ValidatePassword([FromBody] ValidatePasswordRequest request)
    {
        if (string.IsNullOrEmpty(request.Password))
        {
            return BadRequest(new
            {
                IsValid = false,
                Errors = new[] { "Password is required" },
                RequiredComplexityLevel = "High",
                PasswordStrengthScore = 0
            });
        }

        // For anonymous requests, don't validate against specific user (only policy)
        // Use Guid.Empty as placeholder since policy service expects a userId parameter
        var result = await _passwordPolicyService.ValidatePasswordAsync(request.Password, Guid.Empty, request.ComplexityLevel);
        
        // For security, only return minimal information when unauthenticated
        return Ok(new
        {
            result.IsValid,
            result.Errors,
            result.RequiredComplexityLevel,
            result.PasswordStrengthScore
            // Don't return: IsPasswordRecentlyUsed, ContainsUserInfo (security risk)
        });
    }

    /// <summary>
    /// Generates a secure password (admin only)
    /// </summary>
    [HttpPost("generate")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<string>> GenerateSecurePassword([FromBody] GeneratePasswordRequest request)
    {
        var password = await _passwordPolicyService.GenerateSecurePasswordAsync(request.ComplexityLevel);
        return Ok(new { password });
    }
}
