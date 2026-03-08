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

public enum SessionStatus
{
    Active = 1,
    Expired = 2,
    Terminated = 3,
    Locked = 4
}

public enum AccountLockoutReason
{
    FailedLoginAttempts = 1,
    SuspiciousActivity = 2,
    AdminAction = 3,
    ComplianceHold = 4
}

public enum PasswordComplexityLevel
{
    Basic = 1,
    Standard = 2,
    Strong = 3,
    Enterprise = 4
}

public enum IpWhitelistType
{
    AdminAccess = 1,
    ApiAccess = 2,
    HighValueTransactions = 3
}

public enum AccountStatus
{
    Active = 1,
    Inactive = 2,
    Dormant = 3,
    Closed = 4,
    Frozen = 5,
    Suspended = 6
}

public enum AccountType
{
    Savings = 1,
    Checking = 2,
    Business = 3,
    Premium = 4
}

public enum FeeType
{
    MonthlyMaintenance = 1,
    DormancyFee = 2,
    OverdraftFee = 3,
    TransactionFee = 4,
    MinimumBalanceFee = 5,
    AccountClosureFee = 6
}

public enum FeeFrequency
{
    OneTime = 1,
    Daily = 2,
    Weekly = 3,
    Monthly = 4,
    Quarterly = 5,
    Annually = 6
}

public enum AccountHoldType
{
    Legal = 1,
    Compliance = 2,
    Fraud = 3,
    Administrative = 4,
    Regulatory = 5
}

public enum AccountRestrictionType
{
    NoDebits = 1,
    NoCredits = 2,
    NoTransfers = 3,
    LimitedAccess = 4,
    ReadOnly = 5
}

public enum JointAccountRole
{
    PrimaryHolder = 1,
    SecondaryHolder = 2,
    Signatory = 3,
    ViewOnly = 4
}

public enum InterestCompoundingFrequency
{
    Daily = 1,
    Monthly = 2,
    Quarterly = 3,
    SemiAnnually = 4,
    Annually = 5
}
