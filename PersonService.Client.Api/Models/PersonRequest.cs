namespace PersonService.Client.Api.Models;

public record CreatePersonApiRequest(
    string FirstName,
    string LastName,
    string NationalCode,
    DateTime BirthDate);

public record UpdatePersonApiRequest(
    string id,
    string? FirstName,
    string? LastName,
    DateTime? BirthDate);
