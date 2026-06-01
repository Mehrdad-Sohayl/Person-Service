using MediatR;
using PersonService.Domain.Entities;

namespace PersonService.Application.Queries;

public class GetAllPersonsQuery : IRequest<IReadOnlyList<Person>>
{
    public int PageNumber { get; }
    public int PageSize { get; }

    public GetAllPersonsQuery(int pageNumber = 1, int pageSize = 50)
    {
        PageNumber = pageNumber > 0 ? pageNumber : 1;
        PageSize = pageSize > 0 ? pageSize : 50;
    }
}

