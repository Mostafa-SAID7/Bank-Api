using Bank.Application.Interfaces;
using Bank.Application.Helpers.Auth;
using Bank.Application.Helpers.Card;
using Bank.Application.Helpers.Payment;
using Bank.Application.Helpers.Shared;

namespace Bank.Application.Services;

/// <summary>
/// Service implementation for token generation
/// </summary>
public class TokenGenerationService : ITokenGenerationService
{
    public string GenerateSecureToken(int length = 32)
        => SecureTokenGenerator.GenerateSecureToken(length);

    public string GenerateNumericToken(int length = 6)
        => SecureTokenGenerator.GenerateNumericToken(length);

    public string GenerateActivationCode(int length = 8)
        => SecureTokenGenerator.GenerateActivationCode(length);

    public string GenerateRandomPin(int length = 4)
        => PinGenerator.GenerateRandomPin(length);

    public string GenerateConfirmationNumber(string prefix = "CNF")
        => ConfirmationNumberGenerator.GenerateConfirmationNumber(prefix);

    public List<string> GenerateBackupCodes(int count = 10, int length = 8)
        => BackupCodeGenerator.GenerateBackupCodes(count, length);

    public string GenerateSecretKey(int length = 20)
        => TotpSecretGenerator.GenerateSecretKey(length);

    public string GenerateQrCodeUrl(string issuer, string accountName, string secretKey)
        => TotpSecretGenerator.GenerateQrCodeUrl(issuer, accountName, secretKey);

    public string GeneratePassword(int length = 12, bool includeUppercase = true, 
        bool includeLowercase = true, bool includeNumbers = true, bool includeSpecialChars = true)
        => PasswordGenerator.GeneratePassword(length, includeUppercase, includeLowercase, includeNumbers, includeSpecialChars);

    public string GenerateRandomCode(int length = 6, bool alphanumeric = false)
        => RandomCodeGenerator.GenerateRandomCode(length, alphanumeric);
}