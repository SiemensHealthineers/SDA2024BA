using System.Security.Claims;

using AutoMapper;

using Microsoft.Extensions.Logging;

using Moq;

using NeuroMedia.Application.Exceptions;
using NeuroMedia.Application.Features.Questionnaires.Commands.CreateQuestionnaireRecordsCommand;
using NeuroMedia.Application.Interfaces.Repositories;
using NeuroMedia.Application.Logging;
using NeuroMedia.Domain.Entities;
using NeuroMedia.Domain.Enums;

namespace NeuroMedia.Application.Tests.Features.Questionnaires.Commands.CreateResultsForVisit
{
    public class CreateQuestionnaireRecordsCommandTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IQuestionnaireRepository> _questionnaireRepositoryMock;
        private readonly Mock<ILogger<CreateResultsForVisitCommandHandler>> _loggerMock;
        private readonly CreateResultsForVisitCommandHandler _handler;

        public CreateQuestionnaireRecordsCommandTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _questionnaireRepositoryMock = new Mock<IQuestionnaireRepository>();
            _loggerMock = new Mock<ILogger<CreateResultsForVisitCommandHandler>>();
            _handler = new CreateResultsForVisitCommandHandler(_unitOfWorkMock.Object, _mapperMock.Object, _questionnaireRepositoryMock.Object, _loggerMock.Object);

        }


        [Fact]
        public async Task Records_Created_WhenDataValid()
        {
            var visitId = 1;
            var type = QuestionnaireType.CGIDoctor;
            var fileTime = DateTime.Now.ToFileTime();
            var blobPath = $"Results/1/1/1/{type.ToString()}/{fileTime}.json";

            var questionnaire = new Questionnaire
            {
                VisitId = 1,
                QuestionnaireType = 0,
                BlobPath = blobPath
            };

            var createDto = new CreateQuestionnaireRecordsDto
            {
                VisitId = 1,
                QuestionnaireType = 0
            };

            var visit = new Visit
            {
                VisitType = 0,
                DateOfVisit = DateTime.UtcNow,
                PatientId = 1,
            };

            var request = new CreateQuestionnaireRecordsCommand(visitId, type, new ClaimsPrincipal(), new LogRow());

            _unitOfWorkMock.Setup(u => u.Repository<Visit>().GetByIdAsync(It.IsAny<int>(), false)).ReturnsAsync(visit);
            _questionnaireRepositoryMock.Setup(q => q.GetByVisitIdAndTypeAsync(It.IsAny<int>(), It.IsAny<QuestionnaireType>())).ReturnsAsync((Questionnaire?) null);
            _questionnaireRepositoryMock.Setup(q => q.AddAsync(It.IsAny<Questionnaire>())).ReturnsAsync(questionnaire);
            _unitOfWorkMock.Setup(uow => uow.SaveAsync(It.IsAny<CancellationToken>(), It.IsAny<int>(), It.IsAny<string>()));

            _mapperMock.Setup(m => m.Map<Questionnaire>(It.IsAny<CreateQuestionnaireRecordsDto>())).Returns(questionnaire);
            _mapperMock.Setup(m => m.Map<CreateQuestionnaireRecordsDto>(It.IsAny<Questionnaire>())).Returns(createDto);

            var result = await _handler.Handle(request, CancellationToken.None);

            Assert.Equal(createDto, result);
            _questionnaireRepositoryMock.Verify(q => q.AddAsync(It.IsAny<Questionnaire>()), Times.Once());
            _unitOfWorkMock.Verify(uow => uow.SaveAsync(It.IsAny<CancellationToken>(), It.IsAny<int>(), It.IsAny<string>()), Times.Once);
        }


        [Fact]
        public async Task ThrowException_WhenUnexpectedError()
        {
            var visitId = 1;
            var type = QuestionnaireType.CGIDoctor;
            var fileTime = DateTime.Now.ToFileTime();
            var blobPath = $"Results/1/1/1/{type.ToString()}/{fileTime}.json";

            var questionnaire = new Questionnaire
            {
                VisitId = 1,
                QuestionnaireType = 0,
                BlobPath = blobPath
            };

            var createDto = new CreateQuestionnaireRecordsDto
            {
                VisitId = 1,
                QuestionnaireType = 0
            };

            var visit = new Visit
            {
                VisitType = 0,
                DateOfVisit = DateTime.UtcNow,
                PatientId = 1,
            };

            var request = new CreateQuestionnaireRecordsCommand(visitId, type, new ClaimsPrincipal(), new LogRow());
            _unitOfWorkMock.Setup(u => u.Repository<Visit>().GetByIdAsync(It.IsAny<int>(), false)).ReturnsAsync(visit);
            _questionnaireRepositoryMock.Setup(q => q.GetByVisitIdAndTypeAsync(It.IsAny<int>(), It.IsAny<QuestionnaireType>())).ReturnsAsync((Questionnaire?) null);
            _questionnaireRepositoryMock.Setup(q => q.AddAsync(It.IsAny<Questionnaire>()))
                        .ThrowsAsync(new Exception("Unexpected exception"));

            _mapperMock.Setup(m => m.Map<Questionnaire>(It.IsAny<CreateQuestionnaireRecordsDto>())).Returns(new Questionnaire());

            var exception = await Assert.ThrowsAsync<ClientException>(() => _handler.Handle(request, CancellationToken.None));

            Assert.Equal("Results creation failed", exception.Message);
        }


        [Fact]
        public async Task ThrowException_WhenVisitNotFound()
        {
            var visitId = 1;
            var type = QuestionnaireType.CGIDoctor;
            var fileTime = DateTime.Now.ToFileTime();
            var blobPath = $"Results/1/1/1/{type.ToString()}/{fileTime}.json";
            var questionnaire = new Questionnaire
            {
                VisitId = 1,
                QuestionnaireType = 0,
                BlobPath = blobPath
            };

            var createDto = new CreateQuestionnaireRecordsDto
            {
                VisitId = 1,
                QuestionnaireType = 0
            };

            var visit = new Visit
            {
                VisitType = 0,
                DateOfVisit = DateTime.UtcNow,
                PatientId = 1,
            };
            var request = new CreateQuestionnaireRecordsCommand(visitId, type, new ClaimsPrincipal(), new LogRow());

            _unitOfWorkMock.Setup(u => u.Repository<Visit>().GetByIdAsync(It.IsAny<int>(), false)).ReturnsAsync((Visit?)null);
            var exception = await Assert.ThrowsAsync<NotFoundClientException>(() => _handler.Handle(request, CancellationToken.None));
            Assert.Equal("Visit not found", exception.Message);
        }
        [Fact]
        public async Task ThrowException_WhenResultAlreadyExist()
        {
            var visitId = 1;
            var type = QuestionnaireType.CGIDoctor;
            var fileTime = DateTime.Now.ToFileTime();
            var blobPath = $"Results/1/1/1/{QuestionnaireType.CGIDoctor.ToString()}/{fileTime}.json";

            var questionnaire = new Questionnaire
            {
                VisitId = 1,
                QuestionnaireType = 0,
                BlobPath = blobPath
            };

            var visit = new Visit
            {
                VisitType = 0,
                DateOfVisit = DateTime.UtcNow,
                PatientId = 1,
            };
            var request = new CreateQuestionnaireRecordsCommand(visitId, type, new ClaimsPrincipal(), new LogRow());
            _unitOfWorkMock.Setup(u => u.Repository<Visit>().GetByIdAsync(It.IsAny<int>(), false)).ReturnsAsync(visit);
            _questionnaireRepositoryMock.Setup(q => q.GetByVisitIdAndTypeAsync(It.IsAny<int>(), It.IsAny<QuestionnaireType>())).ReturnsAsync(questionnaire);
            var exception = await Assert.ThrowsAsync<BadRequestClientException>(() =>
                _handler.Handle(request, CancellationToken.None));
            Assert.Equal("Questionnaire records already exist", exception.Message);
        }
    }
}
