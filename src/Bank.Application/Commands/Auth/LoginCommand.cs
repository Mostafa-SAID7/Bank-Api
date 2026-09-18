using Bank.Application.Common.Models;
using Bank.Application.DTOs.Auth.Core;
using Bank.Application.Interfaces.Security;
using Bank.Application.Interfaces;
using MediatR;
using System.Security.Cryptography;

namespace Bank.Application.Commands.Auth;

public record LoginCommand(string Email, string Password, string IpAddress, string UserAgent) : IRequest<Result<AuthResponse>>;

public sealed class LoginCommandHandler : IRequestHandler<LoginCommand, Result<AuthResponse>>
{
    private readonly IIdentityService _identityService;
    private readonly ITokenService _tokenService;
    private readonly ISessionService _sessionService;
    private readonly ITwoFactorAuthService _twoFactorService;
    private readonly IAuditLogService _auditLogService;

    public LoginCommandHandler(
        IIdentityService identityService, 
        ITokenService tokenService,
        ISessionService sessionService,
        ITwoFactorAuthService twoFactorService,
        IAuditLogService auditLogService)
    {
        _identityService = identityService;
        _tokenService = tokenService;
        _sessionService = sessionService;
        _twoFactorService = twoFactorService;
        _auditLogService = auditLogService;
    }

    public async Task<Result<AuthResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _identityService.FindByEmailAsync(request.Email);
        if (user == null)
        {
            return Result<AuthResponse>.Failure("Invalid credentials.");
        }

        var validationResult = await _identityService.ValidateCredentialsAsync(user, request.Password);
        if (validationResult.IsLockedOut)
        {
            await _auditLogService.LogSecurityEventAsync(
                user.Id,
                "AccountLocked",
                "User",
                user.Id.ToString(),
                request.IpAddress);
            return Result<AuthResponse>.Failure("Account is temporarily locked due to too many failed attempts.");
        }

        if (!validationResult.Success)
        {
            await _auditLogService.LogSecurityEventAsync(
                user.Id,
                "FailedLogin",
                "User",
                user.Id.ToString(),
                request.IpAddress);
            return Result<AuthResponse>.Failure("Invalid credentials.");
        }

        // Check if MFA is enabled
        var isTwoFactorEnabled = await _twoFactorService.IsTwoFactorEnabledAsync(user.Id);
        if (isTwoFactorEnabled)
        {
            // Issue a temporary challenge token
            var challengeToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
            // In a real system, you'd store this in a temporary cache (like Redis) linked to the UserId.
            // For now, we return it to complete the MFA flow.
            
            await _auditLogService.LogSecurityEventAsync(
                user.Id,
                "MfaChallengeIssued",
                "User",
                user.Id.ToString(),
                request.IpAddress);
            return Result<AuthResponse>.Success(new AuthResponse(
                null, null, true, challengeToken));
        }

        var roles = await _identityService.GetRolesAsync(user);

        // Generate tokens
        var accessToken = await _tokenService.GenerateAccessTokenAsync(user, roles);
        
        // Create session and get refresh token
        var sessionResult = await _sessionService.CreateSessionAsync(user.Id, request.IpAddress, request.UserAgent);
        
        await _auditLogService.LogSecurityEventAsync(
                user.Id,
                "SuccessfulLogin",
                "User",
                user.Id.ToString(),
                request.IpAddress);

        return Result<AuthResponse>.Success(new AuthResponse(
            accessToken,
            sessionResult.RefreshToken,
            false,
            null));
    }
}
