using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace NeuroMedia.Seed.Services.Contract
{
    public abstract class GenericHostedService<TType> : IHostedService
    {
        protected Type Type { get; }

        protected IServiceProvider Provider { get; }

        protected IHostEnvironment Environment { get; }

        protected IConfiguration Configuration { get; }

        protected GenericHostedService(
            IServiceProvider provider,
            IHostEnvironment environment,
            IConfiguration configuration)
        {
            Type = typeof(TType);
            Provider = provider;
            Environment = environment;
            Configuration = configuration;
        }

        public virtual Task StartAsync(CancellationToken cancellationToken)
        {
            var svc = Type.FullName;
            var env = Environment.EnvironmentName;

            Console.WriteLine($"Hosted service '{svc}' started.");
            Console.WriteLine($"Environment: '{env}'");

            return Task.CompletedTask;
        }

        public virtual Task StopAsync(CancellationToken cancellationToken)
        {
            var svc = Type.FullName;

            Console.WriteLine($"Hosted service '{svc}' stopped.");

            return Task.CompletedTask;
        }
    }
}
