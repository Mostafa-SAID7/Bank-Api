using Bank.Application.Interfaces.Security;
using Bank.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System.Linq;

namespace Bank.Infrastructure.Services.Security;

/// <summary>
/// Infrastructure implementation of IIdentityService using ASP.NET Core Identity.
/// Isolates UserManager/SignInManager dependencies from the Application layer.
/// </summary>
public class IdentityService : IIdentityService
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;

    public IdentityService(UserManager<User> userManager, SignInManager<User> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public async Task<User?> FindByIdAsync(Guid userId)
    {
        return await _userManager.FindByIdAsync(userId.ToString());
    }

    public async Task<User?> FindByEmailAsync(string email)
    {
        return await _userManager.FindByEmailAsync(email);
    }

    public async Task<IList<string>> GetRolesAsync(User user)
    {
        return await _userManager.GetRolesAsync(user);
    }

    public async Task<IdentityLoginResult> ValidateCredentialsAsync(User user, string password)
    {
        var result = await _signInManager.CheckPasswordSignInAsync(user, password, lockoutOnFailure: true);
        
        if (result.Succeeded)
        {
            return new IdentityLoginResult { Success = true };
        }

        if (result.IsLockedOut)
        {
            return new IdentityLoginResult 
            { 
                Success = false, 
                IsLockedOut = true, 
                Message = "Account is temporarily locked due to too many failed attempts. Try again later." 
            };
        }

        return new IdentityLoginResult 
        { 
            Success = false, 
            Message = "Invalid credentials." 
        };
    }

    public async Task<IdentityCreationResult> CreateUserAsync(string username, string email, string password, string role = "User")
    {
        var user = new User
        {
            UserName = username,
            Email = email,
            FirstName = username,
            LastName = string.Empty
        };

        var result = await _userManager.CreateAsync(user, password);
        if (!result.Succeeded)
        {
            return new IdentityCreationResult
            {
                Success = false,
                Message = "User creation failed.",
                Errors = result.Errors.Select(e => e.Description).ToList()
            };
        }

        var roleResult = await _userManager.AddToRoleAsync(user, role);
        if (!roleResult.Succeeded)
        {
            // Note: In a real system, we might want to clean up the user or handle this more gracefully,
            // but for this refactor we return the error
            return new IdentityCreationResult
            {
                Success = false,
                Message = "Failed to assign default role.",
                Errors = roleResult.Errors.Select(e => e.Description).ToList()
            };
        }

        return new IdentityCreationResult
        {
            Success = true,
            User = user
        };
    }

    public string HashPassword(User user, string password)
    {
        return _userManager.PasswordHasher.HashPassword(user, password);
    }

    public bool VerifyPassword(User user, string hashedPassword, string providedPassword)
    {
        var result = _userManager.PasswordHasher.VerifyHashedPassword(user, hashedPassword, providedPassword);
        return result == PasswordVerificationResult.Success || result == PasswordVerificationResult.SuccessRehashNeeded;
    }
}
