using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using NeuroMedia.Application.Interfaces.Blobstorages;
using NeuroMedia.Seed.Services.Contract;

namespace NeuroMedia.Seed.Services
{
    public class BlobDropService(IServiceProvider provider,
        IHostEnvironment environment,
        IConfiguration configuration) : GenericHostedService<BlobDropService>(provider, environment, configuration), IDropService
    {
        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            await base.StartAsync(cancellationToken);

            await DropAsync();
        }

        public async Task DropAsync()
        {
            using var scope = Provider.CreateScope();

            var blobStorage = scope.ServiceProvider.GetRequiredService<IBlobStorage>();

            await DropContainer(blobStorage);
        }

        private static async Task DropContainer(IBlobStorage blobStorage)
        {
            Console.WriteLine($"Dropping of the blob storage has started");

            try
            {
                await blobStorage.DropContainer(blobStorage.ContainerClient);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                throw;
            }

            Console.WriteLine($"Dropping of the blob storage has finished");
        }
    }
}
