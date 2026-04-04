using Bank.Domain.Enums;

namespace Bank.Application.DTOs.Payment.Beneficiary;

/// <summary>
/// Request to update beneficiary information
/// </summary>
public class UpdateBeneficiaryRequest
{
    public string? Name { get; set; }
    public string? Nickname { get; set; }
    public string? AccountName { get; set; }
    public string? BankName { get; set; }
    public BeneficiaryCategory? Category { get; set; }
    public decimal? DailyTransferLimit { get; set; }
    public decimal? MonthlyTransferLimit { get; set; }
    public decimal? SingleTransferLimit { get; set; }
    public string? Notes { get; set; }
    public string? Reference { get; set; }
}

