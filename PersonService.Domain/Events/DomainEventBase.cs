namespace Domain.Events;

public record class DomainEventBase : IDomainEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
    public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
    public long AggregateVersion { get; internal set; }
}