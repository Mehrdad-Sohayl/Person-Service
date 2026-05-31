using MediatR;
using PersonService.Domain.Entities;

namespace PersonService.Application.Commands;

public class UpdatePersonCommand : IRequest<Person?>
{

    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime BirthDate { get; set; }

    public UpdatePersonCommand(
        Guid id, string firstName, string lastName, DateTime birthDate)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        BirthDate = birthDate;
    }
}
