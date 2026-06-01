namespace PersonService.Application.Exceptions;

public sealed class ApplicationValidationException : Exception
{
    public IReadOnlyCollection<ApplicationError> Errors { get; }

    public ApplicationValidationException()
    {
    }

    public ApplicationValidationException(ApplicationError domainError)
    {
        Errors = new List<ApplicationError>()
        {
            domainError
        };
    }

    public ApplicationValidationException(List<ApplicationError> domainErrors)
    {
        Errors = domainErrors;
    }

    public ApplicationValidationException(IEnumerable<ApplicationError> errors)
        : base(string.Join("; ", errors))
    {
        if (errors == null || !errors.Any())
            throw new ArgumentException("No error message provided", nameof(errors));
        Errors = errors .ToList().AsReadOnly();
    }

}

public record ApplicationError(string Code, string Message);

public static class ApplicationErrorCodes
{
    public const string NotFound = "Person not found.";
}