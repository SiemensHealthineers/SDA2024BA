using AutoMapper;
using NeuroMedia.Application.Interfaces.Repositories;

using Moq;
using NeuroMedia.Application.Features.Questionnaires.Queries.GetQuestionnairesByVisitId;
using NeuroMedia.Domain.Entities;
using NeuroMedia.Application.Features.Questionnaires.Dtos;

using NeuroMedia.Domain.Enums;

using NeuroMedia.Persistence.Contexts;
using System.Security.Claims;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using NeuroMedia.Application.Logging;
using NeuroMedia.Application.Exceptions;
using NeuroMedia.Application.Interfaces.Repositories;

namespace NeuroMedia.Application.Tests.Features.Questionnaires.Queries.GetQuestionnairesByVisitId
{
    public class GetAllQuestionnairesResultsByVisitIdQueryTests : IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly IMapper _mapper;
        private readonly Mock<ILogger<GetAllQuestionnairesResultsByVisitIdHandler>> _loggerMock;
        private readonly GetAllQuestionnairesResultsByVisitIdHandler _handler;

        public GetAllQuestionnairesResultsByVisitIdQueryTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("TestDatabase")
                .Options;

            _context = new ApplicationDbContext(options);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Questionnaire, QuestionnaireRecordDto>();
            });

            _mapper = config.CreateMapper();
            _loggerMock = new Mock<ILogger<GetAllQuestionnairesResultsByVisitIdHandler>>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _unitOfWorkMock.Setup(u => u.Repository<Questionnaire>().Entities)
                .Returns(_context.Questionnaires.AsQueryable());
            _handler = new GetAllQuestionnairesResultsByVisitIdHandler(_unitOfWorkMock.Object, _mapper, _loggerMock.Object);
        }

        [Fact]
        public async Task Handle_InvalidVisitId_ThrowBadRequestException()
        {
            var visitId = 0;
            var query = new GetAllQuestionnairesResultsByVisitIdQuery(visitId, new ClaimsPrincipal(), new LogRow());

            var exception = await Assert.ThrowsAsync<BadRequestClientException>(() => _handler.Handle(query, CancellationToken.None));
            Assert.Equal("Invalid visit ID", exception.Message);
        }

        [Fact]
        public async Task Handle_VisitNotFound_ThrowNotFoundException()
        {
            var visitId = 1;
            var query = new GetAllQuestionnairesResultsByVisitIdQuery(visitId, new ClaimsPrincipal(), new LogRow());

            _unitOfWorkMock.Setup(uow => uow.Repository<Visit>().GetByIdAsync(It.IsAny<int>(), false))
                .ReturnsAsync((Visit?) null);

            var exception = await Assert.ThrowsAsync<NotFoundClientException>(() => _handler.Handle(query, CancellationToken.None));
            Assert.Equal("Visit not found", exception.Message);
        }

        [Fact]
        public async Task Handle_ReturnsEmptyList_WhenNoQuestionnairesExist()
        {
            ClearDatabase();

            var visit = new Visit();

            _unitOfWorkMock.Setup(uow => uow.Repository<Visit>().GetByIdAsync(It.IsAny<int>(), false))
                .ReturnsAsync(visit);

            _unitOfWorkMock.Setup(uow => uow.Repository<Questionnaire>().GetByIdAsync(It.IsAny<int>(), false))
                .ReturnsAsync((Questionnaire?) null);

            var query = new GetAllQuestionnairesResultsByVisitIdQuery(1, new ClaimsPrincipal(), new LogRow());
            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.Equal([], result);
        }


        [Fact]
        public async Task Handle_ReturnsListOfResultsDto_WhenQuestionnairesExist()
        {
            ClearDatabase();

            var questionnaireType = QuestionnaireType.CGIDoctor;
            var dateTime = DateTime.Now.ToFileTime();
            var blobPath = $"Results/1/1/1/CGIDoctor/{dateTime}.json";
            var questionnaire = new Questionnaire
            {
                VisitId = 1,
                QuestionnaireType = questionnaireType,
                BlobPath = blobPath,
                UpdatedDate = DateTime.Now,
                CreatedDate = DateTime.Now,
                CreatedBy = "TestUser",
                UpdatedBy = "TestUser"
            };

            _context.Questionnaires.Add(questionnaire);
            await _context.SaveChangesAsync();

            var visit = new Visit
            {
                Id = 1
            };

            _unitOfWorkMock.Setup(uow => uow.Repository<Visit>().GetByIdAsync(It.IsAny<int>(), false))
                .ReturnsAsync(visit);

            var query = new GetAllQuestionnairesResultsByVisitIdQuery(1, new ClaimsPrincipal(), new LogRow());
            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Single(result);

            var returnedDto = result.First();

            Assert.Equal(questionnaire.QuestionnaireType, returnedDto.QuestionnaireType);
            Assert.Equal(questionnaire.BlobPath, returnedDto.BlobPath);
        }


        private void ClearDatabase()
        {
            _context.Questionnaires.RemoveRange(_context.Questionnaires);
            _context.SaveChanges();
        }


        void IDisposable.Dispose()
        {
            _context.Dispose();
        }
    }
}
