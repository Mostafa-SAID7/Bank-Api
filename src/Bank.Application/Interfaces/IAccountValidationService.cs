using Bank.Application.DTOs;

namespace Bank.Application.Interfaces;

/// <summary>
/// Service interface for validating external bank accounts
/// </summary>
public interface IAccountValidationService
{
    /// <summary>
    /// Validate external bank account details
    /// </summary>
    Task<AccountValidationResult> ValidateExternalAccountAsync(ExternalAccountValidationRequest request);
    
    /// <summary>
    /// Validate SWIFT code format and existence
    /// </summary>
    Task<SwiftValidationResult> ValidateSwiftCodeAsync(string swiftCode);
    
    /// <summary>
    /// Validate IBAN format and checksum
    /// </summary>
    Task<IbanValidationResult> ValidateIbanAsync(string iban);
    
    /// <summary>
    /// Validate domestic routing number
    /// </summary>
    Task<RoutingNumberValidationResult> ValidateRoutingNumberAsync(string routingNumber, string countryCode = "US");
    
    /// <summary>
    /// Get bank information from SWIFT code
    /// </summary>
    Task<BankInformationResult> GetBankInformationAsync(string swiftCode);
    
    /// <summary>
    /// Validate account number format for specific bank
    /// </summary>
    Task<AccountNumberValidationResult> ValidateAccountNumberFormatAsync(string accountNumber, string bankCode, string countryCode);
    
    /// <summary>
    /// Perform comprehensive beneficiary account validation
    /// </summary>
    Task<ComprehensiveValidationResult> ValidateBeneficiaryAccountAsync(BeneficiaryAccountValidationRequest request);
}