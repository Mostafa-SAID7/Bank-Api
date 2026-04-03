using Bank.Domain.Enums;

namespace Bank.Application.DTOs.Deposit.Interest;

/// <summary>
/// Result of interest calculation
/// </summary>
public class InterestCalculationResult
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public decimal InterestAmount { get; set; }
    public decimal PrincipalAmount { get; set; }
    public decimal InterestRate { get; set; }
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    public int DaysCalculated { get; set; }
    public InterestCompoundingFrequency CompoundingFrequency { get; set; }
}

