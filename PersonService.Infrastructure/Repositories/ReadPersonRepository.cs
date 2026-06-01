using Microsoft.EntityFrameworkCore;
using PersonService.Domain.Entities;
using PersonService.Domain.Interfaces.Repositories;
using PersonService.Infrastructure.Data;

namespace PersonService.Infrastructure.Repositories;

internal class ReadPersonRepository : IReadRepository<Person>
{
    private readonly PersonDbContext _context;

    public ReadPersonRepository(PersonDbContext context)
    {
        _context = context;
    }

    public async Task<Person?> FindByNationalCodeAsync(string nationalCode, CancellationToken ct = default)
    {
        return await _context.Persons
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.NationalCode.Value == nationalCode, ct);
    }

    public async Task<IReadOnlyList<Person>> GetAllAsync(CancellationToken ct = default)
    {
        return await _context.Persons
            .AsNoTracking()
            .ToListAsync(ct);
    }

    public async Task<Person?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await _context.Persons
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id, ct);
    }

    public async Task<IReadOnlyList<Person>> GetPagedAsync(int skip, int take, CancellationToken ct = default)
    {
        return await _context.Persons
            .AsNoTracking()
            .Skip(skip)
            .Take(take)
            .ToListAsync(ct);
    }
}

