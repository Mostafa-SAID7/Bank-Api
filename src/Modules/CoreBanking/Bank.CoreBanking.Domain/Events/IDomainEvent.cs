namespace Bank.CoreBanking.Domain.Events;

/// <summary>
/// Marker interface for domain events within Core Banking module
/// </summary>
public interface IDomainEvent
{
    Guid AggregateId { get; }
}
