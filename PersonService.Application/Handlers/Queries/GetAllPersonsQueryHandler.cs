using MediatR;
using PersonService.Application.Queries;
using PersonService.Domain.Entities;
using PersonService.Domain.Interfaces.Repositories;

namespace PersonService.Application.Handlers.Queries;

public class GetAllPersonsQueryHandler : IRequestHandler<GetAllPersonsQuery, IReadOnlyList<Person>>
{
    private readonly IReadRepository<Person> _readRepository;

    public GetAllPersonsQueryHandler(IReadRepository<Person> readRepository)
    {
        _readRepository = readRepository;
    }

    public async Task<IReadOnlyList<Person>> Handle(GetAllPersonsQuery request, CancellationToken ct)
    {
        // Apply pagination
        var skip = (request.PageNumber - 1) * request.PageSize;
        return await _readRepository.GetPagedAsync(skip, request.PageSize, ct);
    }
}

