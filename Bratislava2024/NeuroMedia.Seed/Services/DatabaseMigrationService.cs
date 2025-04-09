using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using NeuroMedia.Persistence.Contexts;
using NeuroMedia.Seed.Services.Contract;

namespace NeuroMedia.Seed.Services
{
    public class DatabaseMigrationService(IServiceProvider provider,
        IHostEnvironment environment,
        IConfiguration configuration) : GenericHostedService<DatabaseMigrationService>(provider, environment, configuration), IMigrationService
    {
        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            await base.StartAsync(cancellationToken);

            await MigrateAsync();
        }

        public async Task MigrateAsync()
        {
            using var scope = Provider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var db = typeof(ApplicationDbContext).FullName;

            Console.WriteLine("Migration has started");

            await context.Database.MigrateAsync();

            Console.WriteLine("Migration finished");
        }
    }
}
