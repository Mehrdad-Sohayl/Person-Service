using MediatR;
using PersonService.Domain.Entities;

namespace PersonService.Application.Commands;

public class UpdateFirstNameCommand : IRequest<Person?>
{
    public UpdateFirstNameCommand(Guid id, string firstName)
    {
        Id = id;
        FirstName = firstName;
    }

    public Guid Id { get; set; }
    public string FirstName { get; set; }
}
