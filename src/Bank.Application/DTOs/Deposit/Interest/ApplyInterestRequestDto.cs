namespace Bank.Application.DTOs.Deposit.Interest;

/// <summary>
/// Request to apply interest to an account
/// </summary>
public class ApplyInterestRequest
{
    public Bank.Domain.Enums.InterestCalculationMethod CalculationMethod { get; set; }
    public Bank.Domain.Enums.CompoundingFrequency CompoundingFrequency { get; set; }
    public Guid DepositId { get; set; }
    public DateTime EffectiveDate { get; set; }
    public decimal InterestRate { get; set; }
    public Guid AccountId { get; set; }
    public Guid UserId { get; set; }
}


