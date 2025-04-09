using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

using NeuroMedia.Application.Interfaces.Repositories;
using NeuroMedia.Domain.Common;
using NeuroMedia.Domain.Common.Interfaces;
using NeuroMedia.Persistence.Contexts;

using System.Collections;

namespace NeuroMedia.Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext;
        private Hashtable _repositories;
        private bool disposed;

        public UnitOfWork(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public IGenericRepository<T> Repository<T>() where T : BaseAuditableEntity
        {
            if (_repositories == null)
                _repositories = new Hashtable();

            var type = typeof(T).Name;

            if (!_repositories.ContainsKey(type))
            {
                var repositoryType = typeof(GenericRepository<>);

                var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(T)), _dbContext);

                _repositories.Add(type, repositoryInstance);
            }

            return (IGenericRepository<T>) _repositories[type];
        }

        public Task RollbackAsync()
        {
            _dbContext.ChangeTracker.Entries().ToList().ForEach(x => x.Reload());
            return Task.CompletedTask;
        }

        public async Task<int> SaveAsync(CancellationToken cancellationToken, int expected = 1, string userId = "system")
        {
            UpdateAuditableEntities(_dbContext, userId);

            int count;

            if ((count = await _dbContext.SaveChangesAsync(cancellationToken)) < expected)
            {
                throw new DbUpdateException("Save to database failed");
            }

            return count;
        }

        public Task<int> SaveAndRemoveCacheAsync(CancellationToken cancellationToken, params string[] cacheKeys)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
            {
                if (disposing)
                {
                    //dispose managed resources
                    _dbContext.Dispose();
                }
            }
            //dispose unmanaged resources
            disposed = true;
        }

        private static void UpdateAuditableEntities(ApplicationDbContext dbContext, string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return;
            }

            foreach (var entry in dbContext.ChangeTracker.Entries<BaseAuditableEntity>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedBy = userId;
                    entry.Entity.CreatedDate = DateTime.UtcNow;
                    entry.Entity.UpdatedBy = userId;
                    entry.Entity.UpdatedDate = DateTime.UtcNow;
                }

                if (entry.State == EntityState.Modified)
                {
                    entry.Entity.UpdatedBy = userId;
                    entry.Entity.UpdatedDate = DateTime.UtcNow;
                }
            }
        }
    }
}
