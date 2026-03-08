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

public enum AuditEventType
{
    UserAction = 1,
    SystemEvent = 2,
    SecurityEvent = 3,
    ComplianceEvent = 4
}

public enum FraudRiskLevel
{
    Low = 1,
    Medium = 2,
    High = 3,
    Critical = 4
}

public enum FraudRuleType
{
    TransactionAmount,
    TransactionFrequency,
    LocationAnomaly,
    TimeAnomaly,
    DeviceAnomaly,
    BehaviorAnomaly
}
