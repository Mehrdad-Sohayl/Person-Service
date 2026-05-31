using PersonService.Client.Api.Models;
using PersonService.Contracts;

namespace PersonService.Client.Api.Services
{
    public class DeletePersonService
    {
        private readonly IPersonGrpcClientService _personGrpcClientService;

        public DeletePersonService(IPersonGrpcClientService personGrpcClientService)
        {
            _personGrpcClientService = personGrpcClientService;
        }

        public async Task DeleteAsync(string id)
        {
            var grpcRequest = new DeletePersonRequest
            {
                Id = id
            };

            await _personGrpcClientService.DeletePersonAsync(grpcRequest);
        }
    }
}
