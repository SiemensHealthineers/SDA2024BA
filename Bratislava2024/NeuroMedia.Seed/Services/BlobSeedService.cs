using System.Text;
using System.Text.Json;

using Azure;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

using NeuroMedia.Application.Features.Questionnaires.Helpers;
using NeuroMedia.Application.Features.Videos.Helpers;
using NeuroMedia.Application.Interfaces.Blobstorages;
using NeuroMedia.Domain.Enums;
using NeuroMedia.Seed.Configuration;
using NeuroMedia.Seed.Helpers;
using NeuroMedia.Seed.Services.Contract;

namespace NeuroMedia.Seed.Services
{
    public class BlobSeedService(IServiceProvider provider,
        IHostEnvironment environment,
        IConfiguration configuration,
        IOptions<SeedConfiguration> ccSeedConfigurationOptions) : GenericHostedService<BlobSeedService>(provider, environment, configuration), ISeedService
    {
        private const int MaxTryoutLoops = 10;
        private const int MaxSleepLoops = 60;
        private const int SleepTime = 1000;

        private const string StorageDataPath = "StorageData";

        private readonly SeedConfiguration _ccSeedConfiguration = ccSeedConfigurationOptions.Value;

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            await base.StartAsync(cancellationToken);

            // added, dummy test
            var test = _ccSeedConfiguration.Patients.Count == 1;
            await SeedDataAsync();
        }

        public async Task SeedDataAsync()
        {
            for (var i = 0; i < MaxTryoutLoops; i++)
            {
                try
                {
                    await UploadDefaultQuestionnaires();
                    await UploadDefaultVideos();

                    break;
                }
                catch (RequestFailedException e) when ("ContainerBeingDeleted".Equals(e.ErrorCode, StringComparison.Ordinal))
                {
                    for (var j = 0; j < MaxSleepLoops; j++)
                    {
                        Console.Write('*');
                        await Task.Delay(SleepTime);
                    }

                    Console.WriteLine();
                }
            }
        }

        private async Task UploadDefaultQuestionnaires()
        {
            Console.WriteLine($"Seeding of default questionnaires started");

            using var scope = Provider.CreateScope();
            var blobStorage = scope.ServiceProvider.GetRequiredService<IBlobStorage>();

            foreach (QuestionnaireType questionnaireType in Enum.GetValues(typeof(QuestionnaireType)))
            {
                var blobPath = QuestionnaireHelper.GetQuestionnairePath(questionnaireType);

                var questionnaireDto = QuestionnaireGenerationHelper.Generate(questionnaireType);
                var questionnaireJson = JsonSerializer.Serialize(questionnaireDto);
                using var questionnaireStream = new MemoryStream(Encoding.UTF8.GetBytes(questionnaireJson));

                await blobStorage.UploadAsync(blobPath, questionnaireStream);
            }

            Console.WriteLine("Seeding of default questionnaires finished");
        }

        private async Task UploadDefaultVideos()
        {
            Console.WriteLine($"Seeding of default videos started");

            using var scope = Provider.CreateScope();
            var blobStorage = scope.ServiceProvider.GetRequiredService<IBlobStorage>();

            const string videoFolder = "Videos";
            var videosFilesPath = Path.Combine(StorageDataPath, videoFolder);
            var i = 0;

            foreach (var videoFileName in Directory.GetFiles(videosFilesPath))
            {
                if (videoFileName.Contains("SampleVideo_1280x720_2mb"))
                {
                    continue;
                }

                using var videoFileStream = File.OpenRead(videoFileName);
                var blobPath = VideoHelper.GetVideoFilePath(1, 1, i + 1, (VideoType) (i % Enum.GetValues(typeof(VideoType)).Length), Path.GetFileName(videoFileName));

                await blobStorage.UploadAsync(blobPath, videoFileStream);
                i++;
            }

            Console.WriteLine("Seeding of default videos finished");
        }
    }
}
