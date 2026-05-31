namespace PersonService.Domain.ValueObjects;

public sealed class BirthDate : IEquatable<BirthDate>
{
    public DateTime Value { get; }

    private BirthDate() { }

    public BirthDate(DateTime value)
    {
        if (value > DateTime.UtcNow)
            throw new ArgumentException("Birth date cannot be in the future.", nameof(value));

        Value = value;
    }

    public bool Equals(BirthDate? other) =>
        !ReferenceEquals(other, null) && Value == other.Value;

    public override bool Equals(object? obj) => Equals(obj as BirthDate);

    public override int GetHashCode() => Value.GetHashCode();
}