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

public enum RecurringPaymentFrequency
{
    Daily = 1,
    Weekly = 2,
    BiWeekly = 3,
    Monthly = 4,
    Quarterly = 5,
    SemiAnnually = 6,
    Annually = 7
}

public enum RecurringPaymentStatus
{
    Active = 1,
    Paused = 2,
    Cancelled = 3,
    Completed = 4,
    Failed = 5
}

public enum RecurringPaymentExecutionStatus
{
    Scheduled = 1,
    Processing = 2,
    Completed = 3,
    Failed = 4,
    Skipped = 5
}

public enum PaymentTemplateCategory
{
    General = 1,
    Utilities = 2,
    Rent = 3,
    Insurance = 4,
    Loan = 5,
    Investment = 6,
    Salary = 7,
    Vendor = 8,
    Personal = 9
}

public enum BulkTransferStatus
{
    Pending = 1,
    Processing = 2,
    Completed = 3,
    Failed = 4,
    PartiallyCompleted = 5
}

/// <summary>
/// Two-factor authentication methods supported by the system
/// </summary>
public enum TwoFactorMethod
{
    SMS = 1,
    Email = 2,
    AuthenticatorApp = 3,
    BackupCode = 4
}

/// <summary>
/// Two-factor authentication setup status
/// </summary>
public enum TwoFactorStatus
{
    NotSetup = 0,
    Pending = 1,
    Active = 2,
    Disabled = 3
}

/// <summary>
/// Beneficiary type classification
/// </summary>
public enum BeneficiaryType
{
    Internal = 1,    // Same bank account
    External = 2,    // Different bank account
    International = 3 // International bank account
}

/// <summary>
/// Beneficiary category for organization
/// </summary>
public enum BeneficiaryCategory
{
    Personal = 1,
    Family = 2,
    Business = 3,
    Utility = 4,
    Government = 5,
    Investment = 6,
    Loan = 7,
    Insurance = 8,
    Other = 9
}

/// <summary>
/// Beneficiary status
/// </summary>
public enum BeneficiaryStatus
{
    Pending = 1,     // Awaiting verification
    Active = 2,      // Verified and active
    Suspended = 3,   // Temporarily suspended
    Archived = 4,    // Archived but preserving history
    Blocked = 5      // Blocked due to security concerns
}

/// <summary>
/// Statement status
/// </summary>
public enum StatementStatus
{
    Requested = 1,   // Statement generation requested
    Generating = 2,  // Statement is being generated
    Generated = 3,   // Statement generated successfully
    Delivered = 4,   // Statement delivered to customer
    Failed = 5,      // Statement generation failed
    Cancelled = 6    // Statement request cancelled
}

/// <summary>
/// Statement format types
/// </summary>
public enum StatementFormat
{
    PDF = 1,         // PDF format
    CSV = 2,         // CSV format
    Excel = 3,       // Excel format
    HTML = 4,        // HTML format
    JSON = 5         // JSON format
}

/// <summary>
/// Statement delivery methods
/// </summary>
public enum StatementDeliveryMethod
{
    Email = 1,       // Email delivery
    Download = 2,    // Download from portal
    Mail = 3,        // Physical mail
    SMS = 4,         // SMS with download link
    API = 5          // API delivery
}
