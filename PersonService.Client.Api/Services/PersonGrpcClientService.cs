//using PersonService.Client.Api.Models;
//using Grpc.Net.Client;
//using System.Threading.Tasks;
//using Google.Protobuf;
//using System;
//using System.Globalization;

using PersonService.Client.Api.Models;
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

    public async Task<PersonResponse> UpdatePersonAsync(UpdatePersonRequest request)
    {
        return _client.Update(request);
    }
}