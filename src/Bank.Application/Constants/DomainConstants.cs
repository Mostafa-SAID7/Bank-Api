namespace Bank.Application.Constants;

/// <summary>
/// Domain-wide constants for error messages, role names, and common strings
/// </summary>
public static class DomainConstants
{
    // Authentication & Authorization
    public const string ADMIN_ROLE = "Admin";
    public const string USER_ROLE = "User";
    public const string MANAGER_ROLE = "Manager";
    public const string AUDITOR_ROLE = "Auditor";

    // Access & Permissions
    public const string ACCESS_DENIED = "You don't have access to this account";
    public const string USER_NOT_AUTHENTICATED = "User not authenticated";
    public const string USER_NOT_FOUND = "User not found";
    public const string YOU_DONT_HAVE_ACCESS = "You can only access your own resources";
    public const string ADMIN_ACCESS_REQUIRED = "Administrative access required";
    public const string INSUFFICIENT_PERMISSIONS = "You do not have sufficient permissions for this operation";

    // Account & Deposit
    public const string ACCOUNT_PREFIX = "Account";
    public const string FIXED_DEPOSIT_PREFIX = "FixedDeposit";
    public const string ACCESS_OWN_DEPOSITS = "You can only access your own deposits";
    public const string DEPOSIT_NOT_FOUND = "Deposit not found";
    public const string ACCOUNT_NOT_FOUND = "Account not found";
    public const string ACCOUNT_ALREADY_EXISTS = "Account already exists";
    public const string INVALID_ACCOUNT_NUMBER = "Invalid account number";
    public const string INVALID_IBAN = "Invalid IBAN format";
    public const string INVALID_ROUTING_NUMBER = "Invalid routing number";
    public const string INVALID_SWIFT_CODE = "Invalid SWIFT code";

    // Deposit Products & Operations
    public const string DEPOSIT_PRODUCT_NOT_FOUND = "Deposit product not found";
    public const string INTEREST_TIER_NOT_FOUND = "Interest tier not found";
    public const string INVALID_DEPOSIT_AMOUNT = "Invalid deposit amount";
    public const string INSUFFICIENT_BALANCE = "Insufficient balance for this operation";
    public const string UNABLE_TO_PROCESS_MATURITY = "Unable to process maturity action";
    public const string UNABLE_TO_PROCESS_EARLY_WITHDRAWAL = "Unable to process early withdrawal";
    public const string UNABLE_TO_PROCESS_INTEREST = "Unable to process interest calculation";
    public const string EARLY_WITHDRAWAL_NOT_PERMITTED = "Early withdrawal is not permitted";
    public const string DEPOSIT_ALREADY_MATURED = "Deposit has already matured";
    public const string INVALID_WITHDRAWAL_PERIOD = "The deposit withdrawal period is not yet mature";

    // Card Operations
    public const string CARD_NOT_FOUND = "Card not found";
    public const string INVALID_CARD_ID = "Invalid card ID";
    public const string CARD_NOT_FOUND_OR_DENIED = "Card not found or access denied";
    public const string CARD_ALREADY_EXISTS = "Card already exists";
    public const string CARD_IS_INACTIVE = "Card is not active";
    public const string CARD_HAS_EXPIRED = "Card has expired";
    public const string CARD_IS_BLOCKED = "Card is blocked";
    public const string CARD_ACTIVATION_FAILED = "Card activation failed";
    public const string CARD_BLOCK_FAILED = "Card blocking operation failed";
    public const string INVALID_PIN = "Invalid PIN";
    public const string INVALID_CARD_LIMITS = "Invalid card limits";
    public const string TRANSACTION_EXCEEDS_LIMIT = "Transaction amount exceeds card limits";
    public const string MERCHANT_CATEGORY_BLOCKED = "Merchant category is blocked";
    public const string ONLINE_TRANSACTIONS_DISABLED = "Online transactions are disabled for this card";
    public const string INTERNATIONAL_TRANSACTIONS_DISABLED = "International transactions are disabled for this card";

    // Loan Operations
    public const string LOAN_NOT_FOUND = "Loan not found";
    public const string LOAN_APPLICATION_NOT_FOUND = "Loan application not found";
    public const string INVALID_LOAN_AMOUNT = "Invalid loan amount";
    public const string LOAN_APPLICATION_FAILED = "Loan application submission failed";
    public const string LOAN_APPROVAL_FAILED = "Loan approval operation failed";
    public const string LOAN_DISBURSEMENT_FAILED = "Loan disbursement failed";
    public const string NO_PENDING_LOANS = "No pending loans found";
    public const string LOAN_STATUS_INVALID = "Loan status is invalid for this operation";
    public const string INSUFFICIENT_CREDIT_SCORE = "Insufficient credit score for loan approval";
    public const string LOAN_ALREADY_APPROVED = "Loan has already been approved";

    // Payment & Bills
    public const string RECEIPT_NOT_FOUND = "Receipt not found";
    public const string BILL_PRESENTMENT_NOT_FOUND = "Bill presentment not found";
    public const string BILLER_NOT_FOUND = "Biller not found";
    public const string PAYMENT_NOT_FOUND = "Payment not found";
    public const string BENEFICIARY_NOT_FOUND = "Beneficiary not found";
    public const string PAYMENT_FAILED = "Payment processing failed";
    public const string PAYMENT_ALREADY_PROCESSED = "Payment has already been processed";
    public const string INVALID_PAYMENT_AMOUNT = "Invalid payment amount";
    public const string TRANSFER_LIMIT_EXCEEDED = "Transfer limit exceeded";
    public const string NO_UPCOMING_PAYMENTS = "No upcoming payments found";
    public const string DUPLICATE_PAYMENT = "This payment appears to be a duplicate";
    public const string PAYMENT_REFERENCE_INVALID = "Invalid payment reference";

    // Beneficiary Operations
    public const string BENEFICIARY_ALREADY_EXISTS = "Beneficiary already exists";
    public const string BENEFICIARY_INACTIVE = "Beneficiary is inactive";
    public const string BENEFICIARY_NOT_VERIFIED = "Beneficiary has not been verified";
    public const string INVALID_BENEFICIARY_ACCOUNT = "Invalid beneficiary account details";
    public const string YOU_DONT_HAVE_ACCESS_TO_THIS_ACCOUNT = "You don't have access to this account";

    // Session & Authentication
    public const string SESSION_NOT_FOUND = "Session not found";
    public const string SESSION_EXPIRED = "Session has expired";
    public const string INVALID_SESSION_TOKEN = "Invalid session token";
    public const string REFRESH_TOKEN_REQUIRED = "Refresh token is required";
    public const string REFRESH_TOKEN_INVALID = "Refresh token is invalid or expired";
    public const string REFRESH_TOKEN_REVOKED = "Refresh token has been revoked";
    public const string INVALID_CREDENTIALS = "Invalid username or password";
    public const string ACCOUNT_LOCKED = "Account is locked due to too many failed login attempts";
    public const string PASSWORD_RESET_REQUIRED = "Password reset is required before login";

    // Password & Security
    public const string PASSWORD_VALIDATION_FAILED = "Password validation failed";
    public const string PASSWORD_TOO_SHORT = "Password must be at least 8 characters long";
    public const string PASSWORD_TOO_LONG = "Password must not exceed 128 characters";
    public const string PASSWORD_MISSING_UPPERCASE = "Password must contain at least one uppercase letter";
    public const string PASSWORD_MISSING_LOWERCASE = "Password must contain at least one lowercase letter";
    public const string PASSWORD_MISSING_DIGIT = "Password must contain at least one digit";
    public const string PASSWORD_MISSING_SPECIAL_CHAR = "Password must contain at least one special character";
    public const string PASSWORD_NOT_UNIQUE = "Password must contain sufficient unique characters";
    public const string PASSWORD_COMMON = "Password is too common and easily guessable";
    public const string PASSWORD_CONTAINS_PERSONAL_INFO = "Password must not contain personal information";
    public const string PASSWORD_RECENTLY_USED = "Password has been used recently. Please choose a different password";
    public const string PASSWORD_RESET_FAILED = "Password reset operation failed";
    public const string PASSWORD_CHANGE_FAILED = "Password change operation failed";

    // Statement & Audit
    public const string STATEMENT_NOT_FOUND = "Statement not found";
    public const string STATEMENT_GENERATION_FAILED = "Statement generation failed";
    public const string INVALID_DATE_RANGE = "Invalid date range specified";
    public const string AUDIT_ERROR_MESSAGE = "An error occurred while retrieving audit logs";
    public const string AUDIT_LOG_NOT_FOUND = "Audit log not found";

    // IP Whitelist
    public const string IP_WHITELIST_PREFIX = "IpWhitelist";
    public const string IP_ADDRESS_NOT_WHITELISTED = "Your IP address is not whitelisted";
    public const string INVALID_IP_ADDRESS = "Invalid IP address format";
    public const string IP_WHITELIST_ENTRY_EXISTS = "This IP address is already whitelisted";

    // Two-Factor Authentication
    public const string TWO_FACTOR_REQUIRED = "Two-factor authentication is required";
    public const string INVALID_OTP = "Invalid OTP provided";
    public const string OTP_EXPIRED = "OTP has expired";
    public const string OTP_DELIVERY_FAILED = "Failed to send OTP";
    public const string TWO_FACTOR_SETUP_FAILED = "Two-factor authentication setup failed";
    public const string TWO_FACTOR_DISABLE_FAILED = "Two-factor authentication disable operation failed";

    // Email & Notification
    public const string EMAIL_VERIFICATION_FAILED = "Email verification failed";
    public const string EMAIL_ALREADY_VERIFIED = "Email has already been verified";
    public const string EMAIL_CHANGE_FAILED = "Email change operation failed";
    public const string INVALID_EMAIL = "Invalid email address format";
    public const string EMAIL_ALREADY_IN_USE = "Email address is already in use";
    public const string NOTIFICATION_FAILED = "Failed to send notification";

    // Generic/Utility
    public const string UNKNOWN = "Unknown";
    public const string OPERATION_FAILED = "Operation failed";
    public const string UNEXPECTED_ERROR = "An unexpected error occurred";
    public const string INVALID_REQUEST = "Invalid request";
    public const string VALIDATION_FAILED = "Validation failed";
    public const string NOT_IMPLEMENTED = "This feature is not yet implemented";
    public const string SERVICE_UNAVAILABLE = "Service is temporarily unavailable";
    public const string DUPLICATE_ENTRY = "A duplicate entry already exists";
    public const string CONFLICT_DETECTED = "A conflict was detected with existing data";
    public const string RESOURCE_ALREADY_IN_USE = "Resource is already in use";
}
