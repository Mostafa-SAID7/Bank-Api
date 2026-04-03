namespace Bank.Domain.Enums;

/// <summary>
/// Card status
/// </summary>
public enum CardStatus
{
    Inactive = 1,
    Active = 2,
    Blocked = 3,
    Expired = 4,
    Lost = 5,
    Stolen = 6,
    Damaged = 7,
    Cancelled = 8
}
