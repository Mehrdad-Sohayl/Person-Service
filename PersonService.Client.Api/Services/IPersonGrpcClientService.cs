using PersonService.Contracts;

namespace PersonService.Client.Api.Services
{
    public interface IPersonGrpcClientService
    {
        Task<PersonResponse> CreatePersonAsync(CreatePersonRequest request);
        Task<PersonResponse> GetPersonByIdAsync(GetPersonByIdRequest request);
        Task<PersonResponse> UpdatePersonAsync(UpdatePersonRequest request);
        Task DeletePersonAsync(DeletePersonRequest request);
    }
}