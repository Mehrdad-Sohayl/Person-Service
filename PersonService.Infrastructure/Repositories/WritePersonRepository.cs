using Microsoft.EntityFrameworkCore;
using PersonService.Domain.Entities;
using PersonService.Domain.Interfaces.Repositories;
using PersonService.Infrastructure.Data;

namespace PersonService.Infrastructure.Repositories;

internal class WritePersonRepository : IWriteRepository<Person>
{
    private readonly PersonDbContext _context;

    public WritePersonRepository(PersonDbContext context)
    {
        _context = context;
    }

    public async Task<Person> AddAsync(Person entity, CancellationToken ct = default)
    {
        if (entity == null) throw new ArgumentNullException(nameof(entity));
        await _context.Persons.AddAsync(entity, ct);
        await _context.SaveChangesAsync(ct);
        return entity;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var existing = await _context.Persons.FindAsync(new object[] { id }, ct);
        if (existing == null) return false;

        existing.MarkDeleted();

        try
        {
            _context.Persons.Update(existing);
            await _context.SaveChangesAsync(ct);
            return true;
        }
        catch (DbUpdateConcurrencyException ex)
        {
            throw new DbUpdateConcurrencyException($"Could not delete Person(Id={id}) because it was modified by another process.", ex);
        }
    }

    public async Task<Person?> UpdateAsync(Person entity, CancellationToken ct = default)
    {
        if (entity == null) throw new ArgumentNullException(nameof(entity));
        var existing = await _context.Persons.FindAsync(new object[] { entity.Id }, ct);
        if (existing == null) return null;

        var result = _context.Persons.Update(entity);

        try
        {
            await _context.SaveChangesAsync(ct);
            return result.Entity;
        }
        catch (DbUpdateConcurrencyException ex)
        {
            throw new DbUpdateConcurrencyException($"Could not update Person(Id={existing.Id}) because it was modified by another process.", ex);
        }
    }
}
