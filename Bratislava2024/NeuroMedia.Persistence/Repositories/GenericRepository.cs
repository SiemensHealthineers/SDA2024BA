using NeuroMedia.Application.Interfaces.Repositories;
using NeuroMedia.Domain.Common;
using NeuroMedia.Persistence.Contexts;

using Microsoft.EntityFrameworkCore;

namespace NeuroMedia.Persistence.Repositories
{
    public class GenericRepository<T>(ApplicationDbContext dbContext) : IGenericRepository<T> where T : BaseAuditableEntity
    {
        protected readonly ApplicationDbContext _dbContext = dbContext;

        public IQueryable<T> Entities => _dbContext.Set<T>();

        public async Task<T> AddAsync(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
            return entity;
        }

        public Task UpdateAsync(T entity, bool ignoreQueryFilters = false)
        {
            T exist;
            if (ignoreQueryFilters)
            {
                exist = _dbContext.Set<T>().IgnoreQueryFilters().FirstOrDefault(e => e.Id == entity.Id);
            }
            else
            {
                exist = _dbContext.Set<T>().Find(entity.Id);
            }
            _dbContext.Entry(exist).CurrentValues.SetValues(entity);
            return Task.CompletedTask;
        }

        public bool ExistsAsync(T entity)
        {
            T exist = _dbContext.Set<T>().Find(entity.Id);
            return exist != null;
        }

        public async Task DeleteAsync(T entity, bool ignoreQueryFilters = false)
        {
            if (ignoreQueryFilters)
            {
                var existingEntity = await _dbContext.Set<T>().IgnoreQueryFilters().FirstOrDefaultAsync(e => e.Id == entity.Id);
                if (existingEntity != null)
                {
                    _dbContext.Set<T>().Remove(existingEntity);
                }
            }
            else
            {
                _dbContext.Set<T>().Remove(entity);
            }
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _dbContext
                .Set<T>()
                .ToListAsync();
        }

        public async Task<T?> GetByIdAsync(int id, bool ignoreQueryFilters = false)
        {
            if (ignoreQueryFilters)
            {
                return await _dbContext.Set<T>().IgnoreQueryFilters().FirstOrDefaultAsync(e => e.Id == id);
            }
            else
            {
                return await _dbContext.Set<T>().FindAsync(id);
            }
        }
    }
}
