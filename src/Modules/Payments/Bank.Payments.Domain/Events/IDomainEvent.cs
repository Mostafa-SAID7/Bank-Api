namespace Bank.Payments.Domain.Events;

/// <summary>
/// Marker interface for domain events within the Payments module
/// </summary>
public interface IDomainEvent
{
    Guid AggregateId { get; }
}
