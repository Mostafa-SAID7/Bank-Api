namespace Bank.Application.DTOs.Payment.Beneficiary;

/// <summary>
/// Result of beneficiary verification
/// </summary>
public class BeneficiaryVerificationResult
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public bool IsAccountValid { get; set; }
    public string? AccountHolderName { get; set; }
    public string? BankName { get; set; }
    public List<string> ValidationErrors { get; set; } = new();
}

