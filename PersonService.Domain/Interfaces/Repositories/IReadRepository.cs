
using PersonService.Domain.Common;

namespace PersonService.Domain.Interfaces.Repositories
{
    public interface IReadRepository<T> where T : BaseEntity
    {
        Task<T?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task<IReadOnlyList<T>> GetAllAsync(CancellationToken ct = default);
        Task<T?> FindByNationalCodeAsync(string nationalCode, CancellationToken ct = default);
        Task<IReadOnlyList<T>> GetPagedAsync(int skip, int take, CancellationToken ct = default);
    }
}

