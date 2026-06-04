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

        public async Task<PagedResult<List<Person>>> GetAllAsync(GetAllPersonsApiRequest request)
        {
            var grpcRequest = new GetAllPersonsRequest
            {
                PageNumber = request.PageNumber,
                PageSize = request.PageSize
            };

            var allPersonResponse = await _personGrpcClientService.GetAllPersonsAsync(grpcRequest);

            var person = new PagedResult<List<Person>>
            {
                Value = allPersonResponse.Persons.Select(p => new Person
                {
                    Id = p.Id,
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    NationalCode = p.NationalCode,
                    BirthDate = p.BirthDate
                }).ToList(),
                TotalCount = allPersonResponse.TotalCount
            };

            return person;
        }
    }
}
