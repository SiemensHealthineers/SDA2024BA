using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

using NeuroMedia.Application.Features.Videos.Helpers;
using NeuroMedia.Application.Interfaces.Repositories;
using NeuroMedia.Domain.Entities;
using NeuroMedia.Domain.Enums;
using NeuroMedia.Seed.Configuration;
using NeuroMedia.Seed.Services.Contract;

namespace NeuroMedia.Seed.Services
{
    public class DatabaseSeedService(IServiceProvider provider,
        IHostEnvironment environment,
        IConfiguration configuration,
        IOptions<SeedConfiguration> seedConfigurationOptions) : GenericHostedService<DatabaseSeedService>(provider, environment, configuration), ISeedService
    {
        private readonly SeedConfiguration _seedConfiguration = seedConfigurationOptions.Value;
        private readonly List<Patient> _patients = [];
        private readonly List<Visit> _visits = [];
        private readonly List<Questionnaire> _questionnaires = [];
        private readonly List<Video> _videos = [];

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            await base.StartAsync(cancellationToken);

            await SeedDataAsync();
        }

        public async Task SeedDataAsync()
        {
            await CreateDefaultPatients();
            await CreateDefaultVisits();
            await CreateDefaultQuestionnaires();
            await CreateDefaultVideos();
        }

        private async Task CreateDefaultPatients()
        {
            Console.WriteLine("Seeding of default patients has started");

            using var scope = Provider.CreateScope();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

            foreach (var patient in _seedConfiguration.Patients)
            {
                var createdPatient = await unitOfWork.Repository<Patient>().AddAsync(
                    new Patient
                    {
                        Name = patient.Name,
                        Surname = patient.Surname,
                        DateOfBirth = patient.DateOfBirth.ToUniversalTime(),
                        Diagnosis = patient.Diagnosis,
                        DateOfDiagnosis = patient.DateOfDiagnosis.ToUniversalTime(),
                        Sex = patient.Sex,
                        Pseudonym = patient.Pseudonym,
                        Email = patient.Email,
                        PhoneNumber = patient.PhoneNumber,
                        HighestEducation = patient.HighestEducation,
                        EmploymentStatus = patient.EmploymentStatus,
                        RapExamination = patient.RapExamination,
                        PreviousBotulinumToxinApplication = patient.PreviousBotulinumToxinApplication,
                        IsActive = patient.IsActive,
                        TenantId = patient.TenantId,
                        UserId = patient.UserId,
                        CreatedBy = patient.UserId,
                        CreatedDate = DateTime.UtcNow,
                        UpdatedBy = patient.UserId,
                        UpdatedDate = DateTime.UtcNow
                    });

                _patients.Add(createdPatient);
            }

            await unitOfWork.SaveAsync(new CancellationToken(), _seedConfiguration.Patients.Count);

            Console.WriteLine("Seeding of default patients has finished");
        }

        private async Task CreateDefaultVisits()
        {
            Console.WriteLine("Seeding of default visits has started");

            using var scope = Provider.CreateScope();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

            foreach (var visit in _seedConfiguration.Visits)
            {
                var createdVisit = await unitOfWork.Repository<Visit>().AddAsync(
                    new Visit
                    {
                        VisitType = visit.VisitType,
                        DateOfVisit = visit.DateOfVisit.ToUniversalTime(),
                        Note = visit.Note,
                        PatientId = visit.PatientId,
                        CreatedBy = visit.CreatedBy,
                        CreatedDate = DateTime.UtcNow,
                        UpdatedBy = visit.UpdatedBy,
                        UpdatedDate = DateTime.UtcNow
                    });

                _visits.Add(createdVisit);
            }

            await unitOfWork.SaveAsync(new CancellationToken(), _seedConfiguration.Visits.Count);

            Console.WriteLine("Seeding of default visits has finished");
        }


        private async Task CreateDefaultQuestionnaires()
        {
            Console.WriteLine("Seeding of default questionnaires has started");

            using var scope = Provider.CreateScope();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

            foreach (var questionnaire in _seedConfiguration.Questionnaires)
            {
                var createdQuestionnaire = await unitOfWork.Repository<Questionnaire>().AddAsync(
                    new Questionnaire
                    {
                        VisitId = questionnaire.VisitId,
                        QuestionnaireType = questionnaire.QuestionnaireType,
                        BlobPath = questionnaire.BlobPath,
                        CreatedBy = questionnaire.CreatedBy,
                        CreatedDate = DateTime.UtcNow,
                        UpdatedBy = questionnaire.UpdatedBy,
                        UpdatedDate = DateTime.UtcNow
                    });

                _questionnaires.Add(createdQuestionnaire);
            }

            await unitOfWork.SaveAsync(new CancellationToken(), _seedConfiguration.Questionnaires.Count);

            Console.WriteLine("Seeding of default questionnaires has finished");
        }

        private async Task CreateDefaultVideos()
        {
            Console.WriteLine("Seeding of default videos has started");

            using var scope = Provider.CreateScope();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

            var fileTime = 123456789012345678;
            var i = 0;

            foreach (var video in _seedConfiguration.Videos)
            {
                foreach (VideoType videoType in Enum.GetValues(typeof(VideoType)))
                {
                    var createdVideo = await unitOfWork.Repository<Video>().AddAsync(
                        new Video
                        {
                            VisitId = video.VisitId,
                            VideoType = videoType,
                            BlobPath = VideoHelper.GetVideoFilePath(1, 1, i + 1, videoType, $"{fileTime + i}.mp4"),
                            CreatedBy = video.CreatedBy,
                            CreatedDate = DateTime.UtcNow,
                            UpdatedBy = video.UpdatedBy,
                            UpdatedDate = DateTime.UtcNow,
                        });

                    _videos.Add(createdVideo);
                    i++;
                }
            }

            await unitOfWork.SaveAsync(new CancellationToken(), _seedConfiguration.Videos.Count * Enum.GetValues(typeof(VideoType)).Length);

            Console.WriteLine("Seeding of default videos has finished");
        }
    }
}
