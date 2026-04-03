using Bank.Application.Interfaces;
using Bank.Application.Utilities;

namespace Bank.Application.Services;

/// <summary>
/// Service implementation for validation operations
/// </summary>
public class ValidationService : IValidationService
{
    public bool ValidateIbanChecksum(string iban)
        => ValidationHelper.ValidateIbanChecksum(iban);

    public bool ValidateUSRoutingNumberChecksum(string routingNumber)
        => ValidationHelper.ValidateUSRoutingNumberChecksum(routingNumber);

    public bool PerformSanctionsCheck(string name, string? country = null)
        => ValidationHelper.PerformSanctionsCheck(name, country);

    public bool ValidateEmailFormat(string email)
        => ValidationHelper.ValidateEmailFormat(email);

    public bool ValidatePhoneNumberFormat(string phoneNumber)
        => ValidationHelper.ValidatePhoneNumberFormat(phoneNumber);

    public bool ValidateAccountNumberFormat(string accountNumber)
        => ValidationHelper.ValidateAccountNumberFormat(accountNumber);

    public bool ValidateCreditCardNumber(string cardNumber)
        => ValidationHelper.ValidateCreditCardNumber(cardNumber);

    public (bool IsValid, List<string> Errors) ValidatePasswordStrength(string password, int minLength = 8, 
        bool requireUppercase = true, bool requireLowercase = true, bool requireNumbers = true, bool requireSpecialChars = true)
        => ValidationHelper.ValidatePasswordStrength(password, minLength, requireUppercase, requireLowercase, requireNumbers, requireSpecialChars);

    public bool ValidatePinFormat(string pin, int expectedLength = 4)
        => ValidationHelper.ValidatePinFormat(pin, expectedLength);

    public bool ValidateAmountRange(decimal amount, decimal minAmount = 0.01m, decimal? maxAmount = null)
        => ValidationHelper.ValidateAmountRange(amount, minAmount, maxAmount);

    public bool ValidateCurrencyCode(string currencyCode)
        => ValidationHelper.ValidateCurrencyCode(currencyCode);
}