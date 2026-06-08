namespace Bank.Application.DTOs.Payment.Beneficiary;

/// <summary>
/// Request DTO for creating a beneficiary
/// </summary>
public class CreateBeneficiaryRequest
{
    /// <summary>
    /// Beneficiary name
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Beneficiary account number
    /// </summary>
    public string AccountNumber { get; set; } = string.Empty;

    /// <summary>
    /// Bank name where beneficiary account is held
    /// </summary>
    public string BankName { get; set; } = string.Empty;

    /// <summary>
    /// Bank code or routing number
    /// </summary>
    public string BankCode { get; set; } = string.Empty;

    /// <summary>
    /// Optional notes about the beneficiary
    /// </summary>
    public string? Notes { get; set; }
}
