using MediatR;
using PersonService.Domain.Entities;

namespace PersonService.Application.Commands;

public class UpdateBirthDateCommand : IRequest<Person?>
{
    public UpdateBirthDateCommand(Guid id, DateTime birthDate)
    {
        Id = id;
        BirthDate = birthDate;
    }

    public Guid Id { get; set; }
    public DateTime BirthDate { get; set; }
}
