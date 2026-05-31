using MediatR;
using PersonService.Application.Commands;
using PersonService.Domain.Entities;
using PersonService.Domain.Factories;
using PersonService.Domain.Interfaces.Repositories;

namespace PersonService.Application.Handlers.Commands;

public class CreatePersonCommandHandler : IRequestHandler<CreatePersonCommand, Person>
{
    private readonly IWriteRepository<Person> _writeRepository;

    public CreatePersonCommandHandler(IWriteRepository<Person> writeRepository)
    {
        _writeRepository = writeRepository;
    }

    public async Task<Person> Handle(CreatePersonCommand request, CancellationToken ct)
    {
        var entity = PersonFactory.Create(
            id: null,
            firstName: request.FirstName,
            lastName: request.LastName,
            nationalCode: request.NationalCode,
            birthDate: request.BirthDate);

        return await _writeRepository.AddAsync(entity, ct);
    }
}
