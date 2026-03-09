namespace Bank.Application.Interfaces;

/// <summary>
/// Interface for token generation services
/// </summary>
public interface ITokenGenerationService
{
    /// <summary>
    /// Generates a secure random token
    /// </summary>
    string GenerateSecureToken(int length = 32);

    /// <summary>
    /// Generates a numeric token
    /// </summary>
    string GenerateNumericToken(int length = 6);

    /// <summary>
    /// Generates an activation code
    /// </summary>
    string GenerateActivationCode(int length = 8);

    /// <summary>
    /// Generates a random PIN
    /// </summary>
    string GenerateRandomPin(int length = 4);

    /// <summary>
    /// Generates a confirmation number
    /// </summary>
    string GenerateConfirmationNumber(string prefix = "CNF");

    /// <summary>
    /// Generates backup codes for two-factor authentication
    /// </summary>
    List<string> GenerateBackupCodes(int count = 10, int length = 8);

    /// <summary>
    /// Generates a secret key for TOTP
    /// </summary>
    string GenerateSecretKey(int length = 20);

    /// <summary>
    /// Generates a QR code URL for TOTP setup
    /// </summary>
    string GenerateQrCodeUrl(string issuer, string accountName, string secretKey);

    /// <summary>
    /// Generates a random password
    /// </summary>
    string GeneratePassword(int length = 12, bool includeUppercase = true, 
        bool includeLowercase = true, bool includeNumbers = true, bool includeSpecialChars = true);

    /// <summary>
    /// Generates a random code
    /// </summary>
    string GenerateRandomCode(int length = 6, bool alphanumeric = false);
}