using Google.Protobuf.WellKnownTypes;
using PersonService.Client.Api.Models;
using PersonService.Contracts;

namespace PersonService.Client.Api.Services
{
    public class CreatePersonService
    {
        private readonly IPersonGrpcClientService _personGrpcClientService;

        public CreatePersonService(IPersonGrpcClientService personGrpcClientService)
        {
            _personGrpcClientService = personGrpcClientService;
        }

        public async Task<Person> CreateAsync(CreatePersonApiRequest request)
        {
            var grpcRequest = new CreatePersonRequest
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                NationalCode = request.NationalCode,
                BirthDate = request.BirthDate.ToTimestamp(),
            };

            var createPersonResponse = await _personGrpcClientService.CreatePersonAsync(grpcRequest);

            var person = new Person
            {
                Id = createPersonResponse.Id,
                FirstName = createPersonResponse.FirstName,
                LastName = createPersonResponse.LastName,
                NationalCode = createPersonResponse.NationalCode,
                BirthDate = createPersonResponse.BirthDate
            };

            return person;
        }
    }
}
