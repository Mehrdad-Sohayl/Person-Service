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

        public async Task<Person> UpdateAsync(UpdatePersonApiRequest request)
        {
            var person = new Person()
            {
                Id = request.id,
                FirstName = request.FirstName,
                LastName = request.LastName,
                BirthDate = request.BirthDate.ToTimestamp(),
            };

            var grpcRequest = new UpdatePersonRequest
            {
                Person = person,
            };

            var updatePersonResponse = await _personGrpcClientService.UpdatePersonAsync(grpcRequest);

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
