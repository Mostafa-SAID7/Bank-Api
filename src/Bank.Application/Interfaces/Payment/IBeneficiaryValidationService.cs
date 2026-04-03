using Bank.Domain.Entities;

namespace Bank.Application.Interfaces.Payment;

/// <summary>
/// Interface for Beneficiary validation service
/// Defines contract for validating Beneficiary accounts with external services
/// </summary>
public interface IBeneficiaryValidationService
{
    /// <summary>
    /// Validates a Beneficiary account with external bank validation services
    /// </summary>
    /// <param name="beneficiary">The Beneficiary entity to validate</param>
    /// <returns>True if validation succeeds, false otherwise</returns>
    Task<bool> ValidateExternalBeneficiaryAccountAsync(Beneficiary beneficiary);
}
