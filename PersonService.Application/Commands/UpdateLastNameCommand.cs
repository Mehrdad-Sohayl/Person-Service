using MediatR;
using PersonService.Domain.Entities;

namespace PersonService.Application.Commands;

public class UpdateLastNameCommand : IRequest<Person?>
{
    public UpdateLastNameCommand(Guid id, string lastName)
    {
        Id = id;
        LastName = lastName;
    }

    public Guid Id { get; set; }
    public string LastName { get; set; }
}
