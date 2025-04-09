using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using NeuroMedia.Persistence.Contexts;
using NeuroMedia.Seed.Services.Contract;

namespace NeuroMedia.Seed.Services
{
    public class DatabaseDropService(IServiceProvider provider,
        IHostEnvironment environment,
        IConfiguration configuration) : GenericHostedService<DatabaseDropService>(provider, environment, configuration), IDropService
    {
        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            await base.StartAsync(cancellationToken);

            await DropAsync();
        }

        public async Task DropAsync()
        {
            using var scope = Provider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var db = typeof(ApplicationDbContext).FullName;

            Console.WriteLine("Drop of has started");

            await context.Database.EnsureDeletedAsync();

            Console.WriteLine("Drop finished");
        }
    }
}
