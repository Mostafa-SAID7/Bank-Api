using Bank.Domain.Entities;

namespace Bank.Application.Interfaces.Payment;

/// <summary>
/// Interface for PaymentRetry notification service
/// Defines contract for sending payment retry notifications
/// </summary>
public interface IPaymentRetryNotificationService
{
    /// <summary>
    /// Notifies when a payment has reached maximum retry attempts
    /// </summary>
    /// <param name="payment">The BillPayment entity that reached max retries</param>
    Task NotifyPaymentMaxRetriesReached(BillPayment payment);

    /// <summary>
    /// Notifies when a payment has permanently failed after all retry attempts
    /// </summary>
    /// <param name="payment">The BillPayment entity that permanently failed</param>
    Task NotifyPaymentPermanentFailure(BillPayment payment);
}
