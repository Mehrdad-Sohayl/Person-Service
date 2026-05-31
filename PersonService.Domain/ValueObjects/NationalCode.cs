namespace PersonService.Domain.ValueObjects;

public sealed class NationalCode : IEquatable<NationalCode>
{
    public string Value { get; }

    private NationalCode() { }

    public NationalCode(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("National code cannot be empty.", nameof(value));
        if (!System.Text.RegularExpressions.Regex.IsMatch(value, @"^\d{10}$"))
            throw new ArgumentException("National code must be exactly 10 digits.", nameof(value));

        Value = value;
    }

    public bool Equals(NationalCode? other) =>
        !ReferenceEquals(other, null) && Value == other.Value;

    public override bool Equals(object? obj) => Equals(obj as NationalCode);

    public override int GetHashCode() => Value.GetHashCode();
}
