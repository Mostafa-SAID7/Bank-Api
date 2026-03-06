namespace Bank.Domain.Enums;

public enum TransactionStatus
{
    Pending,
    Processing,
    Completed,
    Failed
}

public enum TransactionType
{
    ACH,
    WPS,
    RTGS
}

public enum BatchJobStatus
{
    Pending,
    Processing,
    Completed,
    Failed
}
