using NeuroMedia.Domain.Common.Interfaces;

namespace NeuroMedia.Application.Interfaces.Repositories
{
    public interface IGenericRepository<T> where T : class, IEntity
    {
        IQueryable<T> Entities { get; }

        Task<T?> GetByIdAsync(int id, bool ignoreQueryFilters = false);
        Task<List<T>> GetAllAsync();
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity, bool ignoreQueryFilters = false);
        bool ExistsAsync(T entity);
        Task DeleteAsync(T entity, bool ignoreQueryFilters = false);
    }
}
