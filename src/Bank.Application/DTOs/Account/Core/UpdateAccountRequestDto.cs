namespace Bank.Application.DTOs.Account.Core;

public class UpdateAccountRequest
{
    public Guid? AccountId { get; set; }
    public string? AccountNickname { get; set; }
    public decimal? DailyTransferLimit { get; set; }
    public decimal? MonthlyTransferLimit { get; set; }
    public Bank.Domain.Enums.AccountStatus? Status { get; set; }
    public decimal? OverdraftLimit { get; set; }
    public bool? OverdraftProtectionEnabled { get; set; }
    public string? NotificationPreferences { get; set; }
    public decimal? InterestRate { get; set; }
    public string? AccountHolderName { get; set; }
}
