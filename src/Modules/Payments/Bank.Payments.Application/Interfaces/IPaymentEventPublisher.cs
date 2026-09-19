namespace Bank.Payments.Application;

/// <summary>
/// Publishes payment domain events to the Outbox for async consumption
/// Enables Notifications and Audit modules to consume events without direct coupling
/// </summary>
public interface IPaymentEventPublisher
{
    /// <summary>
    /// Publish a PaymentCompleted event
    /// </summary>
    Task PublishPaymentCompletedAsync(
        Guid paymentId,
        Guid customerId,
        Guid fromAccountId,
        Guid beneficiaryId,
        decimal amount,
        string currency,
        string transactionReference,
        string postingReference,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Publish a PaymentFailed event
    /// </summary>
    Task PublishPaymentFailedAsync(
        Guid paymentId,
        Guid customerId,
        Guid fromAccountId,
        Guid beneficiaryId,
        decimal amount,
        string currency,
        string failureReason,
        int retryCount,
        bool canRetry,
        CancellationToken cancellationToken = default);
}
