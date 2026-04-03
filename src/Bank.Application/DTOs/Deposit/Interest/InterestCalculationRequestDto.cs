namespace Bank.Application.DTOs.Deposit.Interest;

/// <summary>
/// Request to calculate interest for an account
/// </summary>
public class InterestCalculationRequest
{
    public Guid AccountId { get; set; }
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    public int? CompoundingFrequency { get; set; } // Optional override
}

