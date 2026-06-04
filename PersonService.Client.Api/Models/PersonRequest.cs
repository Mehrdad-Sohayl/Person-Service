namespace PersonService.Client.Api.Models;

public record CreatePersonApiRequest(
    string FirstName,
    string LastName,
    string NationalCode,
    DateTime BirthDate);

public record UpdatePersonApiRequest(
    string Id,
    string? FirstName,
    string? LastName,
    DateTime? BirthDate);

public record GetAllPersonsApiRequest(int PageNumber, int PageSize);
