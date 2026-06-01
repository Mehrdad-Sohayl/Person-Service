using PersonService.Domain.Exceptions;

namespace PersonService.Domain.ValueObjects;

public sealed class NationalCode : IEquatable<NationalCode>
{
    public string Value { get; }

    private NationalCode() { }

    public NationalCode(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainValidationException(new DomainError(DomainErrorCodes.EmptyNationalCode, DomainErrorCodes.EmptyNationalCode));
        if (!System.Text.RegularExpressions.Regex.IsMatch(value, @"^\d{10}$"))
            throw new DomainValidationException(new DomainError(DomainErrorCodes.InvalidNationalCode, DomainErrorCodes.InvalidNationalCode));

        Value = value;
    }

    public bool Equals(NationalCode? other) =>
        !ReferenceEquals(other, null) && Value == other.Value;

    public override bool Equals(object? obj) => Equals(obj as NationalCode);

    public override int GetHashCode() => Value.GetHashCode();
}
