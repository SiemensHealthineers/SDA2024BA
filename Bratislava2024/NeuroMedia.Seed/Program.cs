using System.CommandLine;
using System.CommandLine.Invocation;
using System.Diagnostics.CodeAnalysis;

using Azure;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Npgsql;

using NeuroMedia.Seed.Services;
using NeuroMedia.Persistence.Extensions;
using NeuroMedia.Seed.Configuration;

[assembly: ExcludeFromCodeCoverage]
namespace NeuroMedia.Seed
{
    public static class Program
    {
        [SuppressMessage("Performance", "CA1861:Avoid constant arrays as arguments", Justification = "<Pending>")]
        public static async Task<int> Main(string[] args)
        {
            var hostBuilder = CreateHostBuilder(args);

            var dbCommand = new Command("database", "Manage database")
            {
                new Option<bool>(["-d", "--drop"],
                    "Drops the database"),
                new Option<bool>(["-m", "--migrate"],
                    "Applies any pending migrations from NeuroMedia.Persistence/Migrations folder. If the database doesn't exist, it will be created"),
                new Option<bool>(["-s", "--seed"],
                    "Inserts sample data into the database. Any existing data are untouched. The database MUST be created"),
                new Option<bool>(["-l", "--list-migrations"],
                    "Prints out which migrations are currently applied")
            };

            dbCommand.Handler = CreateDbCommandHandler(hostBuilder);

            var blobCommand = new Command("blob", "Manage blob storage data")
            {
                new Option<bool>(["-d", "--drop"],
                    "Drops the whole blob storage container"),
                new Option<bool>(["-s", "--seed"],
                    "Inserts sample data into the blob storage. If the container doesn't exist, it will be created. Any existing data are untouched. The database SHOULD contain related data"),
                new Option<string[]>(["-m", "--migrate"], getDefaultValue: () => [""],
                    "Migrates specified existing generic steps' documents to real implementation ones. Multi-word step names must be enclosed in double-quotes (\")")
            };

            blobCommand.Handler = CreateBlobCommandHandler(hostBuilder);

            var allCommand = new Command("all", "Applies all operations")
            {
                new Option<bool>(["-s", "--seed"],
                    "Inserts sample data into the database and blob storage")
            };

            allCommand.Handler = CreateAllCommandHandler(hostBuilder);

            var rootCommand = new RootCommand("Database and Blob Storage Seed tool")
            {
                blobCommand,
                dbCommand,
                allCommand
            };

            return await rootCommand.InvokeAsync(args);
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            var hostBuilder = Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(configure => configure.AddJsonFile("patients_data.json"))
                .ConfigureAppConfiguration(configure => configure.AddJsonFile("visits_data.json"))
                .ConfigureAppConfiguration(configure => configure.AddJsonFile("questionnaires_data.json"))
                .ConfigureAppConfiguration(configure => configure.AddJsonFile("videos_data.json"))
                .ConfigureServices((hostContext, services) => services
                    .Configure<SeedConfiguration>(hostContext.Configuration)
                    .AddPersistenceLayer(hostContext.Configuration));

            //hostBuilder.ConfigureServices(services =>
            //{
            //    var serviceDescriptor = services.FirstOrDefault(descriptor => descriptor.ServiceType == typeof(IBackgroundWorkerService));

            //    if (serviceDescriptor != null)
            //    {
            //        services.Remove(serviceDescriptor);
            //    }
            //});

            return hostBuilder;
        }

        private static ICommandHandler CreateDbCommandHandler(IHostBuilder hostBuilder)
        {
            return CommandHandler.Create<bool, bool, bool, bool>(async (d, m, s, l) =>
            {
                hostBuilder.ConfigureServices(AddDatabaseHostedServices(d, m, s, l));

                using var host = hostBuilder.Build();

                try
                {
                    await host.StartAsync();

                    return 0;
                }
                catch (NpgsqlException ex)
                {
                    Console.WriteLine("Could not connect to the SQL Server.");
                    Console.WriteLine("Make sure the server is running and the connection string is correct.");
                    Console.WriteLine(ex.Message);

                    return 1;
                }
            });
        }

        private static ICommandHandler CreateBlobCommandHandler(IHostBuilder hostBuilder)
        {
            return CommandHandler.Create<bool, bool>(async (d, s) =>
            {
                hostBuilder.ConfigureServices(AddBlobHostedServices(d, s));

                using var host = hostBuilder.Build();

                try
                {
                    await host.StartAsync();

                    return 0;
                }
                catch (AggregateException e) when (e.InnerException is RequestFailedException)
                {
                    Console.WriteLine("Could not connect to the Blob Storage.");
                    Console.WriteLine("Make sure the storage is running and the connection string is correct.");

                    return 1;
                }
            });
        }

        private static Action<IServiceCollection> AddDatabaseHostedServices(bool drop = false, bool migrate = false, bool seed = false,
            bool list = false)
        {
            return services =>
            {
                if (drop)
                {
                    services.AddHostedService<DatabaseDropService>();
                }

                if (migrate)
                {
                    services.AddHostedService<DatabaseMigrationService>();
                }

                if (seed)
                {
                    services.AddHostedService<DatabaseSeedService>();
                }

                if (list)
                {
                    services.AddHostedService<ListService>();
                }
            };
        }

        private static Action<IServiceCollection> AddBlobHostedServices(bool drop, bool seed)
        {
            return services =>
            {
                if (drop)
                {
                    services.AddHostedService<BlobDropService>();
                }

                if (seed)
                {
                    services.AddHostedService<BlobSeedService>();
                }
            };
        }

        private static ICommandHandler CreateAllCommandHandler(IHostBuilder hostBuilder)
        {
            return CommandHandler.Create<bool>(async s =>
            {
                hostBuilder.ConfigureServices(services =>
                {
                    services.AddHostedService<BlobDropService>();
                    //services.AddHostedService<DatabaseDropService>();

                    services.AddHostedService<DatabaseMigrationService>();

                    if (s)
                    {
                        services.AddHostedService<DatabaseSeedService>();
                        services.AddHostedService<BlobSeedService>();
                    }

                    services.AddHostedService<ListService>();
                });

                using var host = hostBuilder.Build();

                try
                {
                    await host.StartAsync();

                    return 0;
                }
                catch (AggregateException e) when (e.InnerException is RequestFailedException)
                {
                    Console.WriteLine("Could not connect to the Blob Storage.");
                    Console.WriteLine("Make sure the storage is running and the connection string is correct.");

                    return 1;
                }
                catch (NpgsqlException ex)
                {
                    Console.WriteLine("Could not connect to the SQL Server.");
                    Console.WriteLine("Make sure the server is running and the connection string is correct.");
                    Console.WriteLine(ex.Message);

                    return 1;
                }
            });
        }
    }
}
