using Google.Protobuf.WellKnownTypes;
using PersonService.Client.Api.Models;
using PersonService.Contracts;

namespace PersonService.Client.Api.Services
{
    public class UpdatePersonService
    {
        private readonly IPersonGrpcClientService _personGrpcClientService;

        public UpdatePersonService(IPersonGrpcClientService personGrpcClientService)
        {
            _personGrpcClientService = personGrpcClientService;
        }

        public async Task<Person> UpdateFirstNameAsync(UpdatePersonApiRequest request)
        {
            var grpcRequest = new UpdateFirstNameRequest
            {
                Id = request.id,
                FirstName = request.FirstName
            };

            var updatePersonResponse = await _personGrpcClientService.UpdateFirstNameAsync(grpcRequest);

            var updatedPerson = new Person
            {
                Id = updatePersonResponse.Id,
                FirstName = updatePersonResponse.FirstName,
                LastName = updatePersonResponse.LastName,
                NationalCode = updatePersonResponse.NationalCode,
                BirthDate = updatePersonResponse.BirthDate
            };

            return updatedPerson;
        }

        public async Task<Person> UpdateLastNameAsync(UpdatePersonApiRequest request)
        {
            var grpcRequest = new UpdateLastNameRequest
            {
                Id = request.id,
                LastName = request.LastName
            };

            var updatePersonResponse = await _personGrpcClientService.UpdateLastNameAsync(grpcRequest);

            var updatedPerson = new Person
            {
                Id = updatePersonResponse.Id,
                FirstName = updatePersonResponse.FirstName,
                LastName = updatePersonResponse.LastName,
                NationalCode = updatePersonResponse.NationalCode,
                BirthDate = updatePersonResponse.BirthDate
            };

            return updatedPerson;
        }

        public async Task<Person> UpdateBirthDateAsync(UpdatePersonApiRequest request)
        {
            var grpcRequest = new UpdateBirthDateRequest
            {
                Id = request.id,
                BirthDate = request.BirthDate!.Value.ToTimestamp()
            };

            var updatePersonResponse = await _personGrpcClientService.UpdateBirthDateAsync(grpcRequest);

            var updatedPerson = new Person
            {
                Id = updatePersonResponse.Id,
                FirstName = updatePersonResponse.FirstName,
                LastName = updatePersonResponse.LastName,
                NationalCode = updatePersonResponse.NationalCode,
                BirthDate = updatePersonResponse.BirthDate
            };

            return updatedPerson;
        }
    }
}
