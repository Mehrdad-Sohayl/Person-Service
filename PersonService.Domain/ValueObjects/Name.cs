using PersonService.Domain.Exceptions;

namespace PersonService.Domain.ValueObjects;

public sealed class Name : IEquatable<Name>
{
    public string Value { get; }

    private Name() { }


    public Name(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainValidationException(new DomainError(DomainErrorCodes.EmptyName, DomainErrorCodes.EmptyName));

            if(value.Length> 20)
            throw new DomainValidationException(new DomainError(DomainErrorCodes.NameLenght, DomainErrorCodes.NameLenght));

        Value = value;
    }

    public bool Equals(Name? other)
    {
        if (other == null) return false;
        return Value == other.Value;
    }

    public override bool Equals(object? obj) => Equals(obj as Name);

    public override int GetHashCode() => Value.GetHashCode();
}
