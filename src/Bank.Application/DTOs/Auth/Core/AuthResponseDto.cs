namespace Bank.Application.DTOs.Auth.Core;

public record AuthResponse(
    string? Token, 
    string? RefreshToken = null, 
    bool RequiresTwoFactor = false, 
    string? ChallengeToken = null);

