using Bank.Domain.Common;
using Bank.Domain.Enums;

namespace Bank.Domain.Entities;

/// <summary>
/// Represents a payment retry attempt with exponential backoff
/// </summary>
public class PaymentRetry : BaseEntity
{
    public Guid PaymentId { get; set; }
    public int AttemptNumber { get; set; }
    public DateTime AttemptDate { get; set; }
    public DateTime NextRetryDate { get; set; }
    public TimeSpan BackoffDelay { get; set; }
    public string FailureReason { get; set; } = string.Empty;
    public BillPaymentStatus Status { get; set; }
    public bool IsMaxRetriesReached { get; set; }
    public string RetryMetadataJson { get; set; } = string.Empty; // Additional retry metadata as JSON

    // Navigation properties
    public virtual BillPayment Payment { get; set; } = null!;

    /// <summary>
    /// Calculate next retry date with exponential backoff
    /// </summary>
    public static DateTime CalculateNextRetryDate(int attemptNumber, TimeSpan baseDelay)
    {
        // Exponential backoff: baseDelay * 2^(attemptNumber - 1)
        var multiplier = Math.Pow(2, attemptNumber - 1);
        var delay = TimeSpan.FromMilliseconds(baseDelay.TotalMilliseconds * multiplier);
        
        // Cap the maximum delay at 24 hours
        if (delay > TimeSpan.FromHours(24))
        {
            delay = TimeSpan.FromHours(24);
        }

        return DateTime.UtcNow.Add(delay);
    }

    /// <summary>
    /// Mark retry as completed
    /// </summary>
    public void MarkAsCompleted(BillPaymentStatus status)
    {
        Status = status;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Mark as max retries reached
    /// </summary>
    public void MarkAsMaxRetriesReached()
    {
        IsMaxRetriesReached = true;
        Status = BillPaymentStatus.Failed;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Check if retry is due
    /// </summary>
    public bool IsRetryDue()
    {
        return DateTime.UtcNow >= NextRetryDate && !IsMaxRetriesReached;
    }
}