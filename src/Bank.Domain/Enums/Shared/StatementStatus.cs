namespace Bank.Domain.Enums;

/// <summary>
/// Statement status
/// </summary>
public enum StatementStatus
{
    Requested = 1,
    Generating = 2,
    Generated = 3,
    Delivered = 4,
    Failed = 5,
    Cancelled = 6
,
    Draft,
    Viewed,
    Archived
}

