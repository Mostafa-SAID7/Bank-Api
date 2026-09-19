namespace Bank.Contracts;

/// <summary>
/// Marker for messages exchanged between modules inside the monolith.
/// These contracts must not contain domain entities or persistence types.
/// </summary>
public interface IIntegrationEvent
{
    Guid EventId { get; }
    DateTimeOffset OccurredAt { get; }
}