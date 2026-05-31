using PersonService.Client.Api.Models;
using PersonService.Contracts;

namespace PersonService.Client.Api.Services
{
    public class GetPersonService
    {
        private readonly IPersonGrpcClientService _personGrpcClientService;

        public GetPersonService(IPersonGrpcClientService personGrpcClientService)
        {
            _personGrpcClientService = personGrpcClientService;
        }

        public async Task<Person> GetAsync(string id)
        {
            var grpcRequest = new GetPersonByIdRequest
            {
                Id = id
            };

            var personResponse = await _personGrpcClientService.GetPersonByIdAsync(grpcRequest);

            var person = new Person
            {
                Id = personResponse.Id,
                FirstName = personResponse.FirstName,
                LastName = personResponse.LastName,
                NationalCode = personResponse.NationalCode,
                BirthDate = personResponse.BirthDate
            };

            return person;
        }
    }
}
