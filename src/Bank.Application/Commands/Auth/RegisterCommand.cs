using Bank.Application.Common.Models;
using Bank.Application.DTOs.Auth.Core;
using Bank.Application.Interfaces.Security;
using Bank.Application.Interfaces;
using MediatR;

namespace Bank.Application.Commands.Auth;

public record RegisterCommand(string Username, string Email, string Password, string IpAddress) : IRequest<Result<RegisterResponse>>;

public record RegisterResponse(Guid Id, string UserName, string Email);

public sealed class RegisterCommandHandler : IRequestHandler<RegisterCommand, Result<RegisterResponse>>
{
    private readonly IIdentityService _identityService;
    private readonly IAuditLogService _auditLogService;

    public RegisterCommandHandler(IIdentityService identityService, IAuditLogService auditLogService)
    {
        _identityService = identityService;
        _auditLogService = auditLogService;
    }

    public async Task<Result<RegisterResponse>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await _identityService.FindByEmailAsync(request.Email);
        if (existingUser != null)
        {
            // Do not leak that the email exists in production, but for API standard response we usually return a generic message
            return Result<RegisterResponse>.Failure("Registration failed.");
        }

        var result = await _identityService.CreateUserAsync(request.Username, request.Email, request.Password);
        
        if (!result.Success || result.User == null)
        {
            return Result<RegisterResponse>.Failure(result.Message ?? "Registration failed.");
        }

        await _auditLogService.LogSecurityEventAsync(
                result.User.Id,
                "UserRegistered",
                "User",
                result.User.Id.ToString(),
                request.IpAddress);

        return Result<RegisterResponse>.Success(new RegisterResponse(result.User.Id, result.User.UserName!, result.User.Email!));
    }
}
