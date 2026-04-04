namespace Bank.Application.DTOs.Account.Core;

public class CreateAccountRequest
{
    public string AccountNickname { get; set; }
    public Bank.Domain.Enums.AccountType AccountType { get; set; }
    public string Currency { get; set; }
    public decimal InitialDeposit { get; set; }
    public decimal? InterestRate { get; set; }
    public bool OverdraftProtectionEnabled { get; set; }
    public Guid CustomerId { get; set; }
}
