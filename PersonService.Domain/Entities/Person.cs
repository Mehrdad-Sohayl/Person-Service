using Domain.Events;
using PersonService.Domain.Common;
using PersonService.Domain.ValueObjects;

namespace PersonService.Domain.Entities;

public class Person : BaseEntity
{
    public Name FirstName { get; private set; }
    public Name LastName { get; private set; }
    public NationalCode NationalCode { get; private set; }
    public BirthDate BirthDate { get; private set; }

    private Person() { }

    internal Person(
        Guid? id,
        Name firstName,
        Name lastName,
        NationalCode nationalCode,
        BirthDate birthDate)
        : base()
    {
        Id = id == null ? base.Id : id.Value;
        FirstName = firstName;
        LastName = lastName;
        NationalCode = nationalCode;
        BirthDate = birthDate;
    }

    public void UpdateFirstName(Name firstName)
    {
        FirstName = firstName;
        UpdatedAt = DateTime.UtcNow;

        AddDomainEvent(new PersonUpdatedEvent(Id));
    }

    public void UpdateLastName(Name lastName)
    {
        LastName = lastName;
        UpdatedAt = DateTime.UtcNow;
        AddDomainEvent(new PersonUpdatedEvent(Id));
    }

    public void UpdateBirthDate(BirthDate birthDate)
    {
        BirthDate = birthDate;
        UpdatedAt = DateTime.UtcNow;
        AddDomainEvent(new PersonUpdatedEvent(Id));
    }

    public void MarkDeleted()
    {
        IsDeleted = true;
        UpdatedAt = DateTime.UtcNow;
        AddDomainEvent(new PersonUpdatedEvent(Id));
    }
}

