namespace Domain.Events;

public record PersonUpdatedEvent(Guid PersonId) : DomainEventBase;