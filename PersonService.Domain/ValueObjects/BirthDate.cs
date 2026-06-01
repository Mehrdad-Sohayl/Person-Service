using PersonService.Domain.Exceptions;

namespace PersonService.Domain.ValueObjects;

public sealed class BirthDate : IEquatable<BirthDate>
{
    public DateTime Value { get; }

    private BirthDate() { }

    public BirthDate(DateTime value)
    {
        if (value > DateTime.UtcNow)
            throw new DomainValidationException(new DomainError(DomainErrorCodes.InvalidBirthDate, DomainErrorCodes.InvalidBirthDate));

        Value = value;
    }

    public bool Equals(BirthDate? other) =>
        !ReferenceEquals(other, null) && Value == other.Value;

    public override bool Equals(object? obj) => Equals(obj as BirthDate);

    public override int GetHashCode() => Value.GetHashCode();
}