namespace Bank.Application.DTOs.Deposit.Interest;

/// <summary>
/// Request to calculate interest for an account
/// </summary>
public class InterestCalculationRequest
{
    public DateTime EndDate { get; set; }
    public decimal InterestRate { get; set; }
    public decimal Principal { get; set; }
    public DateTime StartDate { get; set; }
    public Guid AccountId { get; set; }
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    public int? CompoundingFrequency { get; set; } // Optional override
}


