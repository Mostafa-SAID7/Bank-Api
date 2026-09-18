namespace Bank.Application.DTOs.Auth.Core;

public record VerifyMfaRequest(string ChallengeToken, string Code, string Email);
