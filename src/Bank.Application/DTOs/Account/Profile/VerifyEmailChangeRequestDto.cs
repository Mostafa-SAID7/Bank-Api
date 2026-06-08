namespace Bank.Application.DTOs.Account.Profile;

public record VerifyEmailChangeRequest(string VerificationToken, string NewEmail);
