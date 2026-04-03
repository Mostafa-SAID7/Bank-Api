namespace Bank.Application.DTOs.Deposit.Interest;

/// <summary>
/// Request to apply interest to an account
/// </summary>
public class ApplyInterestRequest
{
    public Guid AccountId { get; set; }
    public Guid UserId { get; set; }
}

