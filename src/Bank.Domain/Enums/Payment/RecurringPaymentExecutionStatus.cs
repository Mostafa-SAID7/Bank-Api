namespace Bank.Domain.Enums;

public enum RecurringPaymentExecutionStatus
{
    Scheduled = 1,
    Processing = 2,
    Completed = 3,
    Failed = 4,
    Skipped = 5
}
