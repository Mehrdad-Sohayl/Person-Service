namespace PersonService.Contracts.Contracts;

public record GetAllPersonsRequest(int PageNumber, int PageSize);
public record PersonDto(Guid Id, string FirstName, string LastName, string NationalCode, DateTime BirthDate);
public record GetAllPersonsResponse(IReadOnlyList<PersonDto> Persons, int TotalCount);