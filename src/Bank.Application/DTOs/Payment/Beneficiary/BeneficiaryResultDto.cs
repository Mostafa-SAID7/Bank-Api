namespace Bank.Application.DTOs.Payment.Beneficiary;

/// <summary>
/// Result of beneficiary operations
/// </summary>
public class BeneficiaryResult
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public BeneficiaryDto? Beneficiary { get; set; }
    public List<string> Errors { get; set; } = new();
}

