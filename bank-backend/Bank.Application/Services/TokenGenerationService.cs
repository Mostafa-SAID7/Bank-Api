using Bank.Application.Interfaces;
using Bank.Application.Utilities;

namespace Bank.Application.Services;

/// <summary>
/// Service implementation for token generation
/// </summary>
public class TokenGenerationService : ITokenGenerationService
{
    public string GenerateSecureToken(int length = 32)
        => TokenGenerationHelper.GenerateSecureToken(length);

    public string GenerateNumericToken(int length = 6)
        => TokenGenerationHelper.GenerateNumericToken(length);

    public string GenerateActivationCode(int length = 8)
        => TokenGenerationHelper.GenerateActivationCode(length);

    public string GenerateRandomPin(int length = 4)
        => TokenGenerationHelper.GenerateRandomPin(length);

    public string GenerateConfirmationNumber(string prefix = "CNF")
        => TokenGenerationHelper.GenerateConfirmationNumber(prefix);

    public List<string> GenerateBackupCodes(int count = 10, int length = 8)
        => TokenGenerationHelper.GenerateBackupCodes(count, length);

    public string GenerateSecretKey(int length = 20)
        => TokenGenerationHelper.GenerateSecretKey(length);

    public string GenerateQrCodeUrl(string issuer, string accountName, string secretKey)
        => TokenGenerationHelper.GenerateQrCodeUrl(issuer, accountName, secretKey);

    public string GeneratePassword(int length = 12, bool includeUppercase = true, 
        bool includeLowercase = true, bool includeNumbers = true, bool includeSpecialChars = true)
        => TokenGenerationHelper.GeneratePassword(length, includeUppercase, includeLowercase, includeNumbers, includeSpecialChars);

    public string GenerateRandomCode(int length = 6, bool alphanumeric = false)
        => TokenGenerationHelper.GenerateRandomCode(length, alphanumeric);
}