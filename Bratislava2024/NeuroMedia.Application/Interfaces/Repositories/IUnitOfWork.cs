using NeuroMedia.Domain.Common;

namespace NeuroMedia.Application.Interfaces.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<T> Repository<T>() where T : BaseAuditableEntity;

        Task<int> SaveAsync(CancellationToken cancellationToken, int expected = 1, string userId = "system");

        Task<int> SaveAndRemoveCacheAsync(CancellationToken cancellationToken, params string[] cacheKeys);

        Task RollbackAsync();
    }
}
