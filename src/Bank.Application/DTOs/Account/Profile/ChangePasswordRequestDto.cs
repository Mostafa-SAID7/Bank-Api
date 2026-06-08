namespace Bank.Application.DTOs.Account.Profile;

public record ChangePasswordRequest(string CurrentPassword, string NewPassword);
