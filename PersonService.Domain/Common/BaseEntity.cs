using System.ComponentModel.DataAnnotations;
using Domain.Events;

namespace PersonService.Domain.Common;

public abstract class BaseEntity
{
    [Key]
    public Guid Id { get; protected set; }

    public DateTime CreatedAt { get; protected set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; protected set; }
    public bool IsDeleted { get; protected set; } = false;

    private readonly List<IDomainEvent> _domainEvents = new();
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();
    public long Version { get; private set; } = 0;

    [Timestamp]
    public byte[] RowVersion { get; private set; }

    protected BaseEntity()
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.UtcNow;
    }

    protected void AddDomainEvent(IDomainEvent domainEvent)
    {
        Version++;
        if (domainEvent is DomainEventBase eventBase)
        {
            typeof(DomainEventBase)
            .GetProperty(nameof(IDomainEvent.AggregateVersion))?
            .SetValue(eventBase, Version);
        }

        _domainEvents.Add(domainEvent);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}