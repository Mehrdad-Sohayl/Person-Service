using MediatR;

namespace PersonService.Application.Commands;

public class DeletePersonCommand : IRequest
{
    public Guid Id { get; private set; }

    public DeletePersonCommand(Guid id)
    {
        Id = id;
    }
}
