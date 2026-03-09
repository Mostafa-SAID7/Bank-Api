using Bank.Domain.Enums;

namespace Bank.Application.DTOs;

// External Biller Integration DTOs
public record BillerPaymentRequest(
    Guid PaymentId,
    Guid BillerId,
    string BillerAccountNumber,
    string BillerRoutingNumber,
    decimal Amount,
    string Currency,
    string CustomerReference,
    string PaymentDescription,
    DateTime ScheduledDate,
    PaymentMethod PaymentMethod = PaymentMethod.ACH);

public class BillerPaymentResponse
{
    public bool Success { get; set; }
    public string ExternalReference { get; set; } = string.Empty;
    public string ConfirmationNumber { get; set; } = string.Empty;
    public BillPaymentStatus Status { get; set; }
    public DateTime ProcessedDate { get; set; }
    public string Message { get; set; } = string.Empty;
    public decimal? ProcessingFee { get; set; }
    public DateTime? ExpectedDeliveryDate { get; set; }
}

public class BillerPaymentStatusResponse
{
    public string ExternalReference { get; set; } = string.Empty;
    public BillPaymentStatus Status { get; set; }
    public DateTime LastUpdated { get; set; }
    public string StatusMessage { get; set; } = string.Empty;
    public DateTime? DeliveredDate { get; set; }
    public string? FailureReason { get; set; }
}

public class BillerHealthCheckResponse
{
    public Guid BillerId { get; set; }
    public bool IsHealthy { get; set; }
    public DateTime LastChecked { get; set; }
    public TimeSpan ResponseTime { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? ErrorMessage { get; set; }
    public Dictionary<string, object> HealthMetrics { get; set; } = new();
}

public class BillerAccountValidationResponse
{
    public bool IsValid { get; set; }
    public string AccountStatus { get; set; } = string.Empty;
    public string BillerName { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public Dictionary<string, string> ValidationDetails { get; set; } = new();
}

// Bill Presentment DTOs
public class BillPresentmentDto
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public Guid BillerId { get; set; }
    public string BillerName { get; set; } = string.Empty;
    public string AccountNumber { get; set; } = string.Empty;
    public decimal AmountDue { get; set; }
    public decimal MinimumPayment { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime StatementDate { get; set; }
    public string Currency { get; set; } = "USD";
    public BillPresentmentStatus Status { get; set; }
    public string BillNumber { get; set; } = string.Empty;
    public List<BillLineItemDto> LineItems { get; set; } = new();
    public DateTime CreatedAt { get; set; }
    public DateTime? PaidDate { get; set; }
}

public class BillLineItemDto
{
    public string Description { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Category { get; set; } = string.Empty;
    public DateTime? ServiceDate { get; set; }
}

// Batch Processing DTOs
public class BatchPaymentResponse
{
    public string BatchId { get; set; } = string.Empty;
    public int TotalPayments { get; set; }
    public int SuccessfulPayments { get; set; }
    public int FailedPayments { get; set; }
    public DateTime ProcessedDate { get; set; }
    public List<BatchPaymentResult> Results { get; set; } = new();
}

public class BatchPaymentResult
{
    public Guid PaymentId { get; set; }
    public bool Success { get; set; }
    public string ExternalReference { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public BillPaymentStatus Status { get; set; }
}

// Payment Routing DTOs
public class PaymentRoutingPreferences
{
    public Guid BillerId { get; set; }
    public PaymentMethod PreferredMethod { get; set; }
    public List<PaymentMethod> SupportedMethods { get; set; } = new();
    public TimeSpan ProcessingWindow { get; set; }
    public decimal MaxDailyAmount { get; set; }
    public int MaxDailyTransactions { get; set; }
    public bool RequiresPreAuthorization { get; set; }
    public Dictionary<string, object> RoutingRules { get; set; } = new();
}

// Payment Status Synchronization DTOs
public class PaymentStatusSyncResult
{
    public Guid PaymentId { get; set; }
    public bool Success { get; set; }
    public BillPaymentStatus OldStatus { get; set; }
    public BillPaymentStatus NewStatus { get; set; }
    public DateTime SyncDate { get; set; }
    public string Message { get; set; } = string.Empty;
}

// Payment Receipt DTOs
public class PaymentReceiptDto
{
    public Guid PaymentId { get; set; }
    public string ReceiptNumber { get; set; } = string.Empty;
    public Guid CustomerId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string BillerName { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Currency { get; set; } = string.Empty;
    public DateTime PaymentDate { get; set; }
    public DateTime ProcessedDate { get; set; }
    public string ConfirmationNumber { get; set; } = string.Empty;
    public string Reference { get; set; } = string.Empty;
    public PaymentMethod PaymentMethod { get; set; }
    public decimal? ProcessingFee { get; set; }
    public BillPaymentStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
}

// Retry Policy DTOs
public class PaymentRetryRequest
{
    public Guid PaymentId { get; set; }
    public int RetryAttempt { get; set; }
    public string FailureReason { get; set; } = string.Empty;
    public DateTime NextRetryDate { get; set; }
    public TimeSpan BackoffDelay { get; set; }
}

public class PaymentRetryResult
{
    public Guid PaymentId { get; set; }
    public bool Success { get; set; }
    public int AttemptNumber { get; set; }
    public DateTime AttemptDate { get; set; }
    public string Message { get; set; } = string.Empty;
    public DateTime? NextRetryDate { get; set; }
    public bool IsMaxRetriesReached { get; set; }
}

// Enums for Integration
public enum PaymentMethod
{
    ACH = 1,
    Wire = 2,
    Check = 3,
    RealTimePayment = 4,
    CreditCard = 5,
    DebitCard = 6
}

public enum BillPresentmentStatus
{
    Pending = 1,
    Available = 2,
    Paid = 3,
    Overdue = 4,
    Cancelled = 5
}