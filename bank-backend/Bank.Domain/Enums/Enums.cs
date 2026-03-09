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

/// <summary>
/// Loan types supported by the system
/// </summary>
public enum LoanType
{
    Personal = 1,    // Personal loan
    Auto = 2,        // Auto loan
    Mortgage = 3,    // Mortgage loan
    Business = 4,    // Business loan
    Education = 5,   // Education loan
    HomeEquity = 6   // Home equity loan
}

/// <summary>
/// Loan application and processing status
/// </summary>
public enum LoanStatus
{
    UnderReview = 1,     // Application submitted, under review
    Approved = 2,        // Loan approved
    Rejected = 3,        // Loan application rejected
    Disbursed = 4,       // Loan amount disbursed
    Active = 5,          // Loan is active with payments
    PaidOff = 6,         // Loan fully paid off
    Delinquent = 7,      // Loan payments overdue
    DefaultStatus = 8,   // Loan in default
    Cancelled = 9        // Loan application cancelled
}

/// <summary>
/// Loan payment status
/// </summary>
public enum LoanPaymentStatus
{
    Scheduled = 1,   // Payment scheduled
    Paid = 2,        // Payment completed
    Overdue = 3,     // Payment overdue
    Partial = 4,     // Partial payment made
    Failed = 5,      // Payment failed
    Waived = 6       // Payment waived
}

/// <summary>
/// Interest calculation methods
/// </summary>
public enum InterestCalculationMethod
{
    Simple = 1,              // Simple interest
    CompoundMonthly = 2,     // Compound monthly
    CompoundDaily = 3,       // Compound daily
    ReducingBalance = 4,     // Reducing balance method
    FlatRate = 5            // Flat rate method
}

/// <summary>
/// Credit score ranges
/// </summary>
public enum CreditScoreRange
{
    Poor = 1,        // 300-579
    Fair = 2,        // 580-669
    Good = 3,        // 670-739
    VeryGood = 4,    // 740-799
    Excellent = 5    // 800-850
}

/// <summary>
/// Loan document types
/// </summary>
public enum LoanDocumentType
{
    Application = 1,         // Loan application
    Agreement = 2,           // Loan agreement
    IncomeProof = 3,        // Income verification
    IdentityProof = 4,      // Identity verification
    AddressProof = 5,       // Address verification
    Collateral = 6,         // Collateral documents
    Insurance = 7,          // Insurance documents
    Other = 8               // Other supporting documents
}

/// <summary>
/// Card types supported by the system
/// </summary>
public enum CardType
{
    Debit = 1,           // Debit card
    Credit = 2,          // Credit card
    Prepaid = 3,         // Prepaid card
    Business = 4,        // Business card
    Premium = 5          // Premium card
}

/// <summary>
/// Card status
/// </summary>
public enum CardStatus
{
    Inactive = 1,        // Card issued but not activated
    Active = 2,          // Card is active and can be used
    Blocked = 3,         // Card is temporarily blocked
    Expired = 4,         // Card has expired
    Lost = 5,            // Card reported as lost
    Stolen = 6,          // Card reported as stolen
    Damaged = 7,         // Card is damaged
    Cancelled = 8        // Card is permanently cancelled
}

/// <summary>
/// Card activation channels
/// </summary>
public enum CardActivationChannel
{
    Phone = 1,           // Phone activation
    Online = 2,          // Online banking activation
    Mobile = 3,          // Mobile app activation
    ATM = 4,             // ATM activation
    Branch = 5           // Branch activation
}

/// <summary>
/// Card transaction types
/// </summary>
public enum CardTransactionType
{
    Purchase = 1,        // Purchase transaction
    Withdrawal = 2,      // ATM withdrawal
    Refund = 3,          // Refund transaction
    Fee = 4,             // Card fee
    Interest = 5,        // Interest charge
    Payment = 6,         // Payment to card
    Transfer = 7,        // Transfer transaction
    Adjustment = 8       // Manual adjustment
}

/// <summary>
/// Card transaction status
/// </summary>
public enum CardTransactionStatus
{
    Pending = 1,         // Transaction pending
    Authorized = 2,      // Transaction authorized
    Settled = 3,         // Transaction settled
    Declined = 4,        // Transaction declined
    Reversed = 5,        // Transaction reversed
    Disputed = 6         // Transaction disputed
}

/// <summary>
/// Merchant category codes (simplified)
/// </summary>
public enum MerchantCategory
{
    General = 1,         // General merchandise
    Grocery = 2,         // Grocery stores
    Gas = 3,             // Gas stations
    Restaurant = 4,      // Restaurants
    Hotel = 5,           // Hotels
    Travel = 6,          // Travel services
    Entertainment = 7,   // Entertainment
    Healthcare = 8,      // Healthcare
    Education = 9,       // Education
    Government = 10,     // Government services
    Online = 11,         // Online purchases
    ATM = 12,            // ATM transactions
    CashAdvance = 13,    // Cash advance
    Gambling = 14,       // Gambling
    Adult = 15           // Adult entertainment
}

/// <summary>
/// Card block reasons
/// </summary>
public enum CardBlockReason
{
    CustomerRequest = 1,     // Customer requested block
    LostCard = 2,           // Card reported lost
    StolenCard = 3,         // Card reported stolen
    SuspiciousActivity = 4, // Suspicious activity detected
    ExcessiveDeclines = 5,  // Too many declined transactions
    OverLimit = 6,          // Over credit limit
    Expired = 7,            // Card expired
    DamagedCard = 8,        // Card damaged
    ComplianceHold = 9,     // Compliance hold
    SystemSecurity = 10     // System security measure
}
/// <summary>
/// Card networks supported by the system
/// </summary>
public enum CardNetwork
{
    Visa = 1,            // Visa network
    Mastercard = 2,      // Mastercard network
    AmericanExpress = 3, // American Express network
    Discover = 4,        // Discover network
    UnionPay = 5,        // UnionPay network
    JCB = 6,             // JCB network
    DinersClub = 7       // Diners Club network
}

/// <summary>
/// Notification types
/// </summary>
public enum NotificationType
{
    TransactionAlert = 1,    // Transaction notification
    SecurityAlert = 2,       // Security-related alert
    LowBalance = 3,          // Low balance alert
    PaymentReminder = 4,     // Payment due reminder
    AccountUpdate = 5,       // Account status update
    CardAlert = 6,           // Card-related alert
    LoanAlert = 7,           // Loan-related alert
    SystemMaintenance = 8,   // System maintenance notice
    Marketing = 9,           // Marketing notification
    Compliance = 10,         // Compliance notification
    Welcome = 11,            // Welcome message
    PasswordExpiry = 12,     // Password expiry warning
    LoginAlert = 13,         // Login notification
    Other = 99               // Other notifications
}

/// <summary>
/// Notification channels
/// </summary>
public enum NotificationChannel
{
    InApp = 1,               // In-app notification
    Email = 2,               // Email notification
    SMS = 3,                 // SMS notification
    Push = 4,                // Push notification
    WhatsApp = 5,            // WhatsApp notification
    Voice = 6                // Voice call notification
}

/// <summary>
/// Notification status
/// </summary>
public enum NotificationStatus
{
    Pending = 1,             // Notification pending
    Sent = 2,                // Notification sent
    Delivered = 3,           // Notification delivered
    Read = 4,                // Notification read
    Failed = 5,              // Notification failed
    Cancelled = 6,           // Notification cancelled
    Expired = 7              // Notification expired
}

/// <summary>
/// Notification priority levels
/// </summary>
public enum NotificationPriority
{
    Low = 1,                 // Low priority
    Normal = 2,              // Normal priority
    High = 3,                // High priority
    Critical = 4             // Critical priority
}

/// <summary>
/// Security alert types
/// </summary>
public enum SecurityAlertType
{
    LoginFromNewDevice = 1,      // Login from new device
    LoginFromNewLocation = 2,    // Login from new location
    PasswordChanged = 3,         // Password changed
    TwoFactorDisabled = 4,       // 2FA disabled
    AccountLocked = 5,           // Account locked
    SuspiciousActivity = 6,      // Suspicious activity detected
    UnauthorizedAccess = 7,      // Unauthorized access attempt
    DataBreach = 8,              // Data breach notification
    PhishingAttempt = 9,         // Phishing attempt detected
    MalwareDetected = 10,        // Malware detected
    CompromisedCredentials = 11, // Compromised credentials
    Other = 99                   // Other security alerts
}
/// <summary>
/// Biller categories for bill payment services
/// </summary>
public enum BillerCategory
{
    Utilities = 1,       // Electric, Gas, Water, Sewer
    Telecommunications = 2, // Phone, Internet, Cable TV
    Insurance = 3,       // Auto, Home, Life, Health Insurance
    Credit = 4,          // Credit Cards, Loans
    Government = 5,      // Taxes, Fees, Fines
    Healthcare = 6,      // Medical, Dental, Vision
    Education = 7,       // Tuition, Student Loans
    Retail = 8,          // Store Cards, Subscriptions
    Transportation = 9,  // Auto Loans, Transit Passes
    Other = 10          // Miscellaneous billers
}
/// <summary>
/// Bill payment status
/// </summary>
public enum BillPaymentStatus
{
    Pending = 1,         // Payment scheduled but not processed
    Processing = 2,      // Payment is being processed
    Processed = 3,       // Payment processed successfully
    Delivered = 4,       // Payment delivered to biller
    Failed = 5,          // Payment failed
    Cancelled = 6,       // Payment cancelled by customer
    Returned = 7         // Payment returned by biller
}

/// <summary>
/// Bill presentment status
/// </summary>
public enum BillPresentmentStatus
{
    Pending = 1,         // Bill presentment pending
    Presented = 2,       // Bill presented to customer
    Viewed = 3,          // Bill viewed by customer
    Paid = 4,            // Bill paid
    Overdue = 5,         // Bill overdue
    Cancelled = 6        // Bill cancelled
}

/// <summary>
/// Payment methods supported by the system
/// </summary>
public enum PaymentMethod
{
    BankTransfer = 1,    // Bank transfer
    DebitCard = 2,       // Debit card
    CreditCard = 3,      // Credit card
    DigitalWallet = 4,   // Digital wallet
    Check = 5,           // Check payment
    Cash = 6,            // Cash payment
    ACH = 7,             // ACH transfer
    WireTransfer = 8     // Wire transfer
}

/// <summary>
/// Deposit product types
/// </summary>
public enum DepositProductType
{
    SavingsAccount = 1,      // Regular savings account
    FixedDeposit = 2,        // Fixed deposit/Certificate of deposit
    RecurringDeposit = 3,    // Recurring deposit
    HighYieldSavings = 4,    // High yield savings account
    MoneyMarket = 5,         // Money market account
    BusinessSavings = 6      // Business savings account
}

/// <summary>
/// Fixed deposit status
/// </summary>
public enum FixedDepositStatus
{
    Active = 1,              // Deposit is active
    Matured = 2,             // Deposit has matured
    Renewed = 3,             // Deposit has been renewed
    Closed = 4,              // Deposit closed before maturity
    PendingRenewal = 5,      // Awaiting renewal decision
    AutoRenewed = 6          // Automatically renewed
}

/// <summary>
/// Deposit certificate status
/// </summary>
public enum DepositCertificateStatus
{
    Generated = 1,           // Certificate generated
    Issued = 2,              // Certificate issued to customer
    Delivered = 3,           // Certificate delivered
    Cancelled = 4,           // Certificate cancelled
    Replaced = 5             // Certificate replaced
}

/// <summary>
/// Interest tier calculation basis
/// </summary>
public enum InterestTierBasis
{
    Balance = 1,             // Based on account balance
    Term = 2,                // Based on deposit term
    Relationship = 3,        // Based on customer relationship
    Promotional = 4          // Promotional rates
}

/// <summary>
/// Deposit withdrawal penalty type
/// </summary>
public enum WithdrawalPenaltyType
{
    None = 1,                // No penalty
    FixedAmount = 2,         // Fixed penalty amount
    Percentage = 3,          // Percentage of withdrawal
    InterestForfeiture = 4,  // Forfeit earned interest
    Combined = 5             // Combination of penalties
}

/// <summary>
/// Deposit maturity action
/// </summary>
public enum MaturityAction
{
    AutoRenew = 1,           // Automatically renew deposit
    TransferToPrimary = 2,   // Transfer to primary account
    HoldForInstructions = 3, // Hold pending customer instructions
    PartialRenew = 4         // Renew part, transfer remainder
}

/// <summary>
/// Types of deposit transactions
/// </summary>
public enum DepositTransactionType
{
    InterestCredit = 1,      // Interest credited to deposit
    PenaltyCharge = 2,       // Early withdrawal penalty
    MaturityPayout = 3,      // Maturity amount payout
    PartialWithdrawal = 4,   // Partial withdrawal from deposit
    EarlyWithdrawal = 5,     // Early withdrawal (full)
    RenewalCredit = 6,       // Credit for renewal
    FeeCharge = 7,           // Fee charged on deposit
    Adjustment = 8,          // Manual adjustment
    Reversal = 9             // Transaction reversal
}

/// <summary>
/// Types of maturity notices
/// </summary>
public enum MaturityNoticeType
{
    Initial = 1,         // Initial maturity notice
    Reminder = 2,        // Reminder notice
    Final = 3,           // Final notice before auto-action
    AutoRenewal = 4,     // Auto-renewal confirmation
    MaturityConfirmation = 5 // Maturity processing confirmation
}