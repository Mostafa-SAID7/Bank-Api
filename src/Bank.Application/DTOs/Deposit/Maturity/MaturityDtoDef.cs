namespace Bank.Application.DTOs.Deposit.Maturity;
using Bank.Domain.Enums;
using System;

public class MaturityDto
{
    public Guid DepositId { get; set; }
    public DateTime MaturityDate { get; set; }
    public decimal MaturityAmount { get; set; }
    public MaturityAction MaturityAction { get; set; }
    public int? RenewalTerm { get; set; }
    public decimal? RenewalInterestRate { get; set; }
    public Guid? WithdrawalAccountId { get; set; }
    public InterestReinvestmentType? InterestReinvestmentType { get; set; }
    public NotificationPreference? NotificationPreference { get; set; }
}
