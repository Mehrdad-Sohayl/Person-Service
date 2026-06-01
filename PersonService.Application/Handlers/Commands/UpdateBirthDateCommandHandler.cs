using MediatR;
using PersonService.Application.Commands;
using PersonService.Application.Exceptions;
using PersonService.Domain.Entities;
using PersonService.Domain.Factories;
using PersonService.Domain.Interfaces.Repositories;

namespace PersonService.Application.Handlers.Commands;

public class UpdateBirthDateCommandHandler : IRequestHandler<UpdateBirthDateCommand, Person?>
{
    private readonly IReadRepository<Person> _readRepository;
    private readonly IWriteRepository<Person> _writeRepository;

    public UpdateBirthDateCommandHandler(IReadRepository<Person> readRepository, IWriteRepository<Person> writeRepository)
    {
        _readRepository = readRepository;
        _writeRepository = writeRepository;
    }

    public async Task<Person?> Handle(UpdateBirthDateCommand request, CancellationToken ct)
    {
        var existing = await _readRepository.GetByIdAsync(request.Id);

        if (existing != null)
        {
            var person = PersonFactory.Create(
                id: request.Id,
                firstName: existing.FirstName.Value,
                lastName: existing.LastName.Value,
                nationalCode: existing.NationalCode.Value,
                birthDate: request.BirthDate);

            return await _writeRepository.UpdateAsync(person, ct);
        }
        throw new ApplicationValidationException(new ApplicationError(ApplicationErrorCodes.NotFound, ApplicationErrorCodes.NotFound));
    }
}
