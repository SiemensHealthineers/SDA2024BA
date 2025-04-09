using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using NeuroMedia.Persistence.Contexts;
using NeuroMedia.Seed.Services.Contract;

namespace NeuroMedia.Seed.Services
{
    public class ListService(IServiceProvider provider,
        IHostEnvironment environment,
        IConfiguration configuration) : GenericHostedService<ListService>(provider, environment, configuration), IListService
    {
        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            await base.StartAsync(cancellationToken).ConfigureAwait(false);

            await ListAsync().ConfigureAwait(false);
        }

        public async Task ListAsync()
        {
            using var scope = Provider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var db = typeof(ApplicationDbContext).FullName;

            Console.WriteLine($"Database version of {db}");

            var appliedMigrations = (await context.Database.GetAppliedMigrationsAsync())
                .ToList();

            if (appliedMigrations.Count > 0)
            {
                Console.WriteLine($"Applied migrations: {appliedMigrations.Count}");

                foreach (var migration in appliedMigrations)
                {
                    Console.WriteLine($"{migration}");
                }
            }
            else
            {
                Console.WriteLine($"No applied migrations:  {appliedMigrations.Count}");
            }
        }
    }
}
