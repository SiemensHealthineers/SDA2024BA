using Xunit;
using System.Threading;
using System.Threading.Tasks;
using System;
using AutoMapper;
using Microsoft.EntityFrameworkCore.InMemory;
using Moq;
using NeuroMedia.Application.Interfaces.Repositories;
using NeuroMedia.Persistence.Contexts;
using NeuroMedia.Application.Features.Visits.Queries.GetActualVisit;
using NeuroMedia.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using NeuroMedia.Persistence.Repositories;
using NeuroMedia.Domain.Enums;
using NeuroMedia.Application.Features.Visits.Queries;
using NeuroMedia.Application.Features.Questionnaires.Dtos;
using NeuroMedia.Application.Features.Videos.Dtos;


namespace NeuroMedia.Application.Tests.Features.Visits.Queries
{
    public class GetActualVisitQueryTests : IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;


        public GetActualVisitQueryTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("TestDatabase")
                .Options;

            _context = new ApplicationDbContext(options);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Visit, GetActualVisitDto>();
                cfg.CreateMap<Questionnaire, QuestionnaireRecordDto>();
                cfg.CreateMap<Video, VideoInfoDto>();
            });
            _mapper = config.CreateMapper();

            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockUnitOfWork.Setup(u => u.Repository<Visit>().Entities)
            .Returns(_context.Visits.AsQueryable());
        }

        [Fact]
        public async Task GetActualVisitQueryHandler_ShouldReturnActualVisitDto_WhenVisitExists()
        {
            // Arrange
            ClearDatabase();
            var fileTime = DateTime.Now.ToFileTime();
            var patientId = 1;
            var visitDate = DateTime.UtcNow.Date;
            var visitType = VisitType.Personal;
            var questionaireType = QuestionnaireType.CGIPatient;

            _context.Visits.Add(new Visit
            {
                PatientId = patientId,
                DateOfVisit = visitDate,
                VisitType = visitType,
                Questionnaires = [
                        new() {
                            VisitId = patientId,
                            QuestionnaireType = questionaireType,
                            BlobPath = $"Results/1/1/1/{QuestionnaireType.CGIPatient.ToString()}/{fileTime}.json",
                            CreatedBy = "",
                            UpdatedBy = ""
                        }
                    ],
                Videos= [],
                CreatedBy = $"TestUser",
                UpdatedBy = $"TestUser"
            });

            await _context.SaveChangesAsync();

            var handler = new GetActualVisitQueryHandler(_mockUnitOfWork.Object, _mapper);

            // Act
            var result = await handler.Handle(new GetActualVisitQuery(patientId), CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(visitDate, result.DateOfVisit);
            Assert.Equal(visitType, result.VisitType);
            Assert.Equal(questionaireType, result.Questionnaires.First().QuestionnaireType);
        }

        [Fact]
        public async Task GetActualVisitQueryHandler_ShouldReturnNull_WhenNoVisitExists()
        {
            // Arrange
            ClearDatabase();

            var patientId = 1;

            var handler = new GetActualVisitQueryHandler(_mockUnitOfWork.Object, _mapper);

            // Act
            var result = await handler.Handle(new GetActualVisitQuery(patientId), CancellationToken.None);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetActualVisitQueryHandler_ShouldReturnActualActualVisitDto_WhenMultipleVisitsExist()
        {
            // Arrange
            ClearDatabase();

            var numberOfVisits = 6;
            var patientId = 1;
            var visitDate = DateTime.UtcNow.Date;

            for (var i = 1; i <= numberOfVisits; i++)
            {
                _context.Visits.Add(new Visit
                {
                    PatientId = patientId,
                    DateOfVisit = visitDate.AddDays(i - 1),
                    CreatedBy = $"TestUser{i}",
                    UpdatedBy = $"TestUser{i}"
                });

                await _context.SaveChangesAsync();
            }

            var handler = new GetActualVisitQueryHandler(_mockUnitOfWork.Object, _mapper);

            // Act
            var result = await handler.Handle(new GetActualVisitQuery(patientId), CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(visitDate, result.DateOfVisit);
        }

        private void ClearDatabase()
        {
            _context.Visits.RemoveRange(_context.Visits);
            _context.SaveChanges();
        }

        void IDisposable.Dispose()
        {
            _context.Dispose();
        }
    }
}
