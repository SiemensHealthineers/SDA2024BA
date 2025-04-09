using NeuroMedia.Application.Interfaces.Repositories;
using NeuroMedia.Persistence.Contexts;
using NeuroMedia.Persistence.Repositories;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using NeuroMedia.Application.Interfaces.Blobstorages;
using NeuroMedia.Infrastructure.Blobstorages;

namespace NeuroMedia.Persistence.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddPersistenceLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMappings();
            services.AddDbContext(configuration);
            services.AddBlobStorage();
            services.AddRepositories();

            return services;
        }

        private static void AddMappings(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
        }

        public static void AddDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("ApplicationDatabaseConnection");
            var migrationsAssemblyName = typeof(ApplicationDbContext).Assembly.FullName;

            services.AddDbContext<ApplicationDbContext>(options => options
                .UseNpgsql(connectionString!,
                    b => b.MigrationsAssembly(migrationsAssemblyName))
                .EnableSensitiveDataLogging());

        }

        public static void AddBlobStorage(this IServiceCollection services)
        {
            services
                .AddScoped<IBlobServiceFactory, BlobServiceFactory>()
                .AddScoped<IBlobStorage, BlobStorage>();
        }

        private static void AddRepositories(this IServiceCollection services)
        {
            services
                .AddTransient(typeof(IUnitOfWork), typeof(UnitOfWork))
                .AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>))
                .AddTransient(typeof(IPatientRepository), typeof(PatientRepository))
                .AddTransient(typeof(IQuestionnaireRepository), typeof(QuestionnaireRepository))
                .AddTransient(typeof(IVideoRepository), typeof(VideoRepository));
        }
    }
}
