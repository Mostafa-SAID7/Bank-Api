namespace Bank.Application.DTOs.Deposit.Interest;

/// <summary>
/// Request to calculate interest for a period
/// </summary>
public class InterestCalculationRequest
{
    public DateTime EndDate { get; set; }
    public decimal InterestRate { get; set; }
    public decimal Principal { get; set; }
    public DateTime StartDate { get; set; }
}
