namespace Bank.Application.Interfaces;

/// <summary>
/// Interface for validation services
/// </summary>
public interface IValidationService
{
    /// <summary>
    /// Validates IBAN checksum
    /// </summary>
    bool ValidateIbanChecksum(string iban);

    /// <summary>
    /// Validates US routing number checksum
    /// </summary>
    bool ValidateUSRoutingNumberChecksum(string routingNumber);

    /// <summary>
    /// Performs sanctions check
    /// </summary>
    bool PerformSanctionsCheck(string name, string? country = null);

    /// <summary>
    /// Validates email format
    /// </summary>
    bool ValidateEmailFormat(string email);

    /// <summary>
    /// Validates phone number format
    /// </summary>
    bool ValidatePhoneNumberFormat(string phoneNumber);

    /// <summary>
    /// Validates account number format
    /// </summary>
    bool ValidateAccountNumberFormat(string accountNumber);

    /// <summary>
    /// Validates credit card number
    /// </summary>
    bool ValidateCreditCardNumber(string cardNumber);

    /// <summary>
    /// Validates password strength
    /// </summary>
    (bool IsValid, List<string> Errors) ValidatePasswordStrength(string password, int minLength = 8, 
        bool requireUppercase = true, bool requireLowercase = true, bool requireNumbers = true, bool requireSpecialChars = true);

    /// <summary>
    /// Validates PIN format
    /// </summary>
    bool ValidatePinFormat(string pin, int expectedLength = 4);

    /// <summary>
    /// Validates amount range
    /// </summary>
    bool ValidateAmountRange(decimal amount, decimal minAmount = 0.01m, decimal? maxAmount = null);

    /// <summary>
    /// Validates currency code format
    /// </summary>
    bool ValidateCurrencyCode(string currencyCode);
}