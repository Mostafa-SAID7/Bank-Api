namespace Bank.Contracts.Payments;

public sealed record PaymentCompleted(
    Guid PaymentId,
    Guid AccountId,
    decimal Amount,
    string Currency,
    string PostingReference,
    Guid EventId,
    DateTimeOffset OccurredAt) : IIntegrationEvent;

public sealed record PaymentFailed(
    Guid PaymentId,
    Guid AccountId,
    decimal Amount,
    string Currency,
    string FailureReason,
    Guid EventId,
    DateTimeOffset OccurredAt) : IIntegrationEvent;