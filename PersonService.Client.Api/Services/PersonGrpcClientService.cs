using PersonService.Contracts;

namespace PersonService.Client.Api.Services;

public class PersonGrpcClientService : IPersonGrpcClientService
{
    private readonly PersonCrudService.PersonCrudServiceClient _client;

    public PersonGrpcClientService(PersonCrudService.PersonCrudServiceClient client)
    {
        _client = client;
    }

    public async Task<PersonResponse> CreatePersonAsync(CreatePersonRequest request)
    {
        return _client.Create(request);
    }

    public async Task DeletePersonAsync(DeletePersonRequest request)
    {
        _client.Delete(request);
    }

    public async Task<PersonResponse> GetPersonByIdAsync(GetPersonByIdRequest request)
    {
        return _client.GetById(request);
    }

    public async Task<GetAllPersonsResponse> GetAllPersonsAsync(GetAllPersonsRequest request)
    {
        return _client.GetAll(request);
    }

    public async Task<PersonResponse> UpdateBirthDateAsync(UpdateBirthDateRequest request)
    {
        return _client.UpdateBirthDate(request);
    }

    public async Task<PersonResponse> UpdateFirstNameAsync(UpdateFirstNameRequest request)
    {
        return _client.UpdateFirstName(request);
    }

    public async Task<PersonResponse> UpdateLastNameAsync(UpdateLastNameRequest request)
    {
        return _client.UpdateLastName(request);
    }
}