using Bank.Domain.Enums;

namespace Bank.Application.DTOs.Payment.Beneficiary;

/// <summary>
/// Request to add a new beneficiary
/// </summary>
public class AddBeneficiaryRequest
{
    public string Name { get; set; } = string.Empty;
    public string AccountNumber { get; set; } = string.Empty;
    public string? Nickname { get; set; }
    public string? AccountName { get; set; }
    public string BankName { get; set; } = string.Empty;
    public string BankCode { get; set; } = string.Empty;
    public string? SwiftCode { get; set; }
    public string? IbanNumber { get; set; }
    public string? RoutingNumber { get; set; }
    public BeneficiaryType Type { get; set; }
    public BeneficiaryCategory Category { get; set; }
    public decimal? DailyTransferLimit { get; set; }
    public decimal? MonthlyTransferLimit { get; set; }
    public decimal? SingleTransferLimit { get; set; }
    public string? Notes { get; set; }
    public string? Reference { get; set; }
}

