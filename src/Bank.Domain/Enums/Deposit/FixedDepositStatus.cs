namespace Bank.Domain.Enums;

/// <summary>
/// Fixed deposit status
/// </summary>
public enum FixedDepositStatus
{
    Active = 1,
    Matured = 2,
    Renewed = 3,
    Closed = 4,
    PendingRenewal = 5,
    AutoRenewed = 6
}
