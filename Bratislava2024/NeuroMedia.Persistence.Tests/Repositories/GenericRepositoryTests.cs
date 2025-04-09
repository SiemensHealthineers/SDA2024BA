using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

using NeuroMedia.Domain.Common;
using NeuroMedia.Persistence.Contexts;
using NeuroMedia.Persistence.Repositories;

namespace NeuroMedia.Persistence.Tests.Repositories
{
    [ExcludeFromCodeCoverage]
    public class GenericRepositoryTests : IDisposable
    {
        private readonly TestDbContext _context;
        private readonly TestRepository _repository;
        private static readonly int s_id = 1;

        public GenericRepositoryTests()
        {
            var dbName = $"TestDb_{DateTime.Now.ToFileTimeUtc()}";
            var dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(dbName)
                .Options;

            _context = new TestDbContext(dbContextOptions);
            _repository = new TestRepository(_context);
        }

        [Fact]
        public async Task QueryReturnsAllData()
        {
            await PopulateDataAsync(_context);

            var entities = await _repository
                .Entities
                .ToListAsync();

            Assert.Single(entities);
        }

        [Fact]
        public async Task QueryReturnsDataByCustomLogic()
        {
            await PopulateDataAsync(_context);

            var entity = await _repository
                .Entities
                .FirstOrDefaultAsync();

            Assert.NotNull(entity);
        }

        [Fact]
        public async Task InsertAsyncCreatesNewEntity()
        {
            await PopulateDataAsync(_context);

            await _repository.AddAsync(new TestEntity { CreatedDate = DateTime.UtcNow, CreatedBy = "xyz", UpdatedDate = DateTime.UtcNow, UpdatedBy = "xyz" });
            var saved = await _context.SaveChangesAsync();

            Assert.Equal(1, saved);

            var entities = await _repository
                .Entities
                .ToListAsync();

            Assert.Equal(2, entities.Count);
        }

        [Fact]
        public async Task UpdateSavesChangedData()
        {
            await PopulateDataAsync(_context);
            var entity = await _repository
                .Entities
                .AsTracking()
                .FirstAsync();
            entity.Changed = true;

            await _repository.UpdateAsync(entity);
            var saved = await _context.SaveChangesAsync();

            Assert.Equal(1, saved);

            var changedEntity = await _repository
                .Entities
                .FirstAsync();

            Assert.True(changedEntity.Changed);
        }

        [Fact]
        public async Task DeleteRemovesEntity()
        {
            await PopulateDataAsync(_context);
            var entity = await _repository
                .Entities
                .AsTracking()
                .FirstAsync();

            await _repository.DeleteAsync(entity);
            var saved = await _context.SaveChangesAsync();

            Assert.Equal(1, saved);

            var entities = await _repository
                .Entities
                .ToListAsync();

            Assert.Empty(entities);
        }

        private static async Task PopulateDataAsync(TestDbContext context)
        {
            await context.Entities.AddAsync(new TestEntity { Id = s_id, CreatedDate = DateTime.UtcNow, CreatedBy = "abc", UpdatedDate = DateTime.UtcNow, UpdatedBy = "abc" });
            await context.SaveChangesAsync();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            _context.Dispose();
        }

        private class TestEntity : BaseAuditableEntity
        {
            public bool Changed { get; set; }
        }

        private class TestDbContext(DbContextOptions<ApplicationDbContext> options) : ApplicationDbContext(options)
        {
            public DbSet<TestEntity> Entities => Set<TestEntity>();
        }

        private class TestRepository(ApplicationDbContext context) : GenericRepository<TestEntity>(context)
        {
        }
    }
}
