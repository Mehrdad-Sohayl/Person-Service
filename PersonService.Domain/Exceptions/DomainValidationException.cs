namespace PersonService.Domain.Exceptions;

public sealed class DomainValidationException : Exception
{
    public IReadOnlyCollection<DomainError> Errors { get; }

    public DomainValidationException()
    {
    }

    public DomainValidationException(DomainError domainError)
    {
        Errors = new List<DomainError>()
        {
            domainError
        };
    }

    public DomainValidationException(List<DomainError> domainErrors)
    {
        Errors = domainErrors;
    }

    public DomainValidationException(IEnumerable<DomainError> errors)
        : base(string.Join("; ", errors))
    {
        if (errors == null || !errors.Any())
            throw new ArgumentException("No error message provided", nameof(errors));
        Errors = errors .ToList().AsReadOnly();
    }

}

public record DomainError(string Code, string Message);

public static class DomainErrorCodes
{
    public const string EmptyName = "Name can not be empty";
    public const string NameLenght = "Name lenght must be less than or equal to 20 charachters";
    public const string EmptyNationalCode = "National code cannot be empty.";
    public const string InvalidNationalCode = "National code must be exactly 10 digits.";
    public const string InvalidBirthDate = "Birth date cannot be in the future.";
}