using PersonService.Contracts;

namespace PersonService.Client.Api.Services
{
    public interface IPersonGrpcClientService
    {
        Task<PersonResponse> CreatePersonAsync(CreatePersonRequest request);
        Task<PersonResponse> GetPersonByIdAsync(GetPersonByIdRequest request);
        Task<PersonResponse> UpdateFirstNameAsync(UpdateFirstNameRequest request);
        Task<PersonResponse> UpdateLastNameAsync(UpdateLastNameRequest request);
        Task<PersonResponse> UpdateBirthDateAsync(UpdateBirthDateRequest request);

        Task DeletePersonAsync(DeletePersonRequest request);
    }
}