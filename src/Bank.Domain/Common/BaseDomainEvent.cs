namespace Bank.Domain.Common;

/// <summary>
/// Base implementation for domain events
/// </summary>
public abstract class BaseDomainEvent : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}