using NeuroMedia.Domain.Entities;

using Microsoft.EntityFrameworkCore;

using System.Reflection;

namespace NeuroMedia.Persistence.Contexts
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        public DbSet<Patient> Patients => Set<Patient>();
        public DbSet<UserInfo> UserInfos => Set<UserInfo>();
        public DbSet<Visit> Visits => Set<Visit>();
        public DbSet<Questionnaire> Questionnaires => Set<Questionnaire>();
        public DbSet<Video> Videos => Set<Video>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            modelBuilder.Entity<Patient>().HasQueryFilter(p => p.IsActive);

            modelBuilder.Entity<Patient>()
                .HasMany(p => p.Visits)
                .WithOne(v => v.Patient)
                .HasForeignKey(v => v.PatientId);

            modelBuilder.Entity<Visit>()
                .HasMany(v => v.Questionnaires)
                .WithOne(q => q.Visit)
                .HasForeignKey(q => q.VisitId);

            modelBuilder.Entity<Visit>()
                .HasMany(v => v.Videos)
                .WithOne(q => q.Visit)
                .HasForeignKey(q => q.VisitId);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            return await base.SaveChangesAsync(cancellationToken);
        }

        public override int SaveChanges()
        {
            return SaveChangesAsync().GetAwaiter().GetResult();
        }
    }
}
