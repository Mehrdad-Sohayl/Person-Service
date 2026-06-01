using MediatR;
using PersonService.Application.Commands;
using PersonService.Application.Exceptions;
using PersonService.Domain.Entities;
using PersonService.Domain.Factories;
using PersonService.Domain.Interfaces.Repositories;

namespace PersonService.Application.Handlers.Commands;

public class UpdateLastNameCommandHandler : IRequestHandler<UpdateLastNameCommand, Person?>
{
        private readonly IReadRepository<Person> _readRepository;

    private readonly IWriteRepository<Person> _writeRepository;

    public UpdateLastNameCommandHandler(IReadRepository<Person> readRepository, IWriteRepository<Person> writeRepository)
    {
        _readRepository = readRepository;
        _writeRepository = writeRepository;
    }

    public async Task<Person?> Handle(UpdateLastNameCommand request, CancellationToken ct)
    {
        var existing = await _readRepository.GetByIdAsync(request.Id);

        if (existing != null)
        {
            var person = PersonFactory.Create(
                id: request.Id,
                firstName: existing.FirstName.Value,
                lastName: request.LastName,
                nationalCode: existing.NationalCode.Value,
                birthDate: existing.BirthDate.Value);

            return await _writeRepository.UpdateAsync(person, ct);
        }
        throw new ApplicationValidationException(new ApplicationError(ApplicationErrorCodes.NotFound, ApplicationErrorCodes.NotFound));
    }
}
