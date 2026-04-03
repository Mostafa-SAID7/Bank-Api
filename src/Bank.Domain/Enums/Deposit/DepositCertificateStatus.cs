namespace Bank.Domain.Enums;

/// <summary>
/// Deposit certificate status
/// </summary>
public enum DepositCertificateStatus
{
    Generated = 1,
    Issued = 2,
    Delivered = 3,
    Cancelled = 4,
    Replaced = 5
}
