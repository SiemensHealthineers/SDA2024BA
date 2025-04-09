using AutoMapper;

using Moq;
using Microsoft.Extensions.Logging;

using NeuroMedia.Application.Interfaces.Repositories;
using NeuroMedia.Application.Logging;
using NeuroMedia.Application.Features.Questionnaires.Commands.UploadAnswers;
using NeuroMedia.Application.Interfaces.Blobstorages;
using NeuroMedia.Application.Exceptions;
using NeuroMedia.Domain.Enums;
using NeuroMedia.Domain.Entities;
using NeuroMedia.Application.Features.Questionnaires.Dtos;
using System.Security.Claims;
using NeuroMedia.Application.Features.Questionnaires.Queries.GetQuestionnaireResultsByBlobPath;

namespace NeuroMedia.Application.Tests.Features.Questionnaires.Commands.UploadAnswers
{
    public class UploadAnswersCommandTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IBlobStorage> _blobStorageMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<UploadAnswersCommandHandler>> _loggerMock;
        private readonly UploadAnswersCommandHandler _handler;

        public UploadAnswersCommandTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _blobStorageMock = new Mock<IBlobStorage>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<UploadAnswersCommandHandler>>();
            _handler = new UploadAnswersCommandHandler(_unitOfWorkMock.Object, _blobStorageMock.Object, _mapperMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task Answers_Uploaded_WhenDataValid()
        {
            var questionnaire = new Questionnaire { QuestionnaireType = QuestionnaireType.Botulotoxin };
            var resultsDto = new QuestionnaireRecordDto();
            var visit = new Visit();

            _unitOfWorkMock.Setup(uow => uow.Repository<Questionnaire>().GetByIdAsync(It.IsAny<int>(), false))
                .ReturnsAsync(questionnaire);

            _blobStorageMock.Setup(bs => bs.UploadAsync(
                It.IsAny<string>(),
                It.IsAny<MemoryStream>(),
                It.IsAny<IDictionary<string, string>>(),
                It.IsAny<bool>(),
                It.IsAny<bool>(),
                It.IsAny<LogRow>()
                ))
                .ReturnsAsync(true);

            _unitOfWorkMock.Setup(uow => uow.Repository<Questionnaire>().UpdateAsync(It.IsAny<Questionnaire>(), false))
                .Returns(Task.CompletedTask);

            _unitOfWorkMock.Setup(uow => uow.Repository<Visit>().GetByIdAsync(It.IsAny<int>(), false))
            .ReturnsAsync(visit);

            _unitOfWorkMock.Setup(uow => uow.SaveAsync(It.IsAny<CancellationToken>(), It.IsAny<int>(), It.IsAny<string>()))
                .ReturnsAsync(1);


            _unitOfWorkMock.Setup(uow => uow.Repository<Questionnaire>().GetByIdAsync(It.IsAny<int>(), false))
                .ReturnsAsync(questionnaire);

            _mapperMock.Setup(m => m.Map<QuestionnaireRecordDto>(It.IsAny<Questionnaire>()))
                .Returns(resultsDto);

            var fileTime = DateTime.Now.ToFileTime();
            var blobPath = $"Results/1/1/1/{QuestionnaireType.Botulotoxin.ToString()}/{fileTime}.json";
            var command = new UploadAnswersCommand(blobPath, new AnswersDto(), new ClaimsPrincipal(), new LogRow());

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(resultsDto, result);
        }

        [Fact]
        public async Task Handle_ThrowsBadRequestException_WhenBlobPathIsInvalid()
        {
            var dateTime = DateTime.Now.ToFileTime();
            var questionnaireType = QuestionnaireType.CGIDoctor;
            var blobPath = $"Result/1/1/1/{questionnaireType}/{dateTime}.json";

            var claimsPrincipal = new ClaimsPrincipal();
            var logRow = new LogRow();
            var command = new UploadAnswersCommand(blobPath, new AnswersDto(), new ClaimsPrincipal(), new LogRow());

            var exception = await Assert.ThrowsAsync<BadRequestClientException>(() => _handler.Handle(command, CancellationToken.None));
            Assert.Equal("Invalid blob path", exception.Message);
        }

        [Fact]
        public async Task ThrowException_WhenQuestionnaireNotFound()
        {
            _unitOfWorkMock.Setup(uow => uow.Repository<Questionnaire>().GetByIdAsync(It.IsAny<int>(), false))
                .ReturnsAsync((Questionnaire?) null);

            var fileTime = DateTime.Now.ToFileTime();
            var blobPath = $"Results/1/1/1/{QuestionnaireType.CGIDoctor.ToString()}/{fileTime}.json";


            var command = new UploadAnswersCommand(blobPath, new AnswersDto(), new ClaimsPrincipal(), new LogRow());

            await Assert.ThrowsAsync<NotFoundClientException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task ThrowException_WhenResultsAlreadyExist()
        {
            var fileTime = DateTime.Now.ToFileTime();
            var questionnaire = new Questionnaire { BlobPath = $"Results/1/1/1/{QuestionnaireType.CGIDoctor.ToString()}/{fileTime}.json" };
            _unitOfWorkMock.Setup(uow => uow.Repository<Questionnaire>().GetByIdAsync(It.IsAny<int>(), false))
                .ReturnsAsync(questionnaire);

            fileTime = DateTime.Now.ToFileTime();
            var blobPath = $"Results/1/1/1/{QuestionnaireType.CGIDoctor.ToString()}/{fileTime}.json";
            var command = new UploadAnswersCommand(blobPath, new AnswersDto(), new ClaimsPrincipal(), new LogRow());

            await Assert.ThrowsAsync<BadRequestClientException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task ThrowException_WhenWrongQuestionnaireType()
        {
            var questionnaire = new Questionnaire { QuestionnaireType = QuestionnaireType.CGIDoctor };
            _unitOfWorkMock.Setup(uow => uow.Repository<Questionnaire>().GetByIdAsync(It.IsAny<int>(), false))
                .ReturnsAsync(questionnaire);

            var fileTime = DateTime.Now.ToFileTime();
            var blobPath = $"Results/1/1/1/{QuestionnaireType.CGIPatient.ToString()}/{fileTime}.json";
            var command = new UploadAnswersCommand(blobPath, new AnswersDto(), new ClaimsPrincipal(), new LogRow());

            await Assert.ThrowsAsync<BadRequestClientException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task ThrowException_WhenBlobStorageUploadFails()
        {
            var questionnaire = new Questionnaire { QuestionnaireType = QuestionnaireType.CGIDoctor };
            _unitOfWorkMock.Setup(uow => uow.Repository<Questionnaire>().GetByIdAsync(It.IsAny<int>(), false))
                .ReturnsAsync(questionnaire);

            _blobStorageMock.Setup(bs => bs.UploadAsync(
                It.IsAny<string>(),
                It.IsAny<MemoryStream>(),
                It.IsAny<IDictionary<string, string>>(),
                It.IsAny<bool>(),
                It.IsAny<bool>(),
                It.IsAny<LogRow>()
                ))
                .ReturnsAsync(false);

            var fileTime = DateTime.Now.ToFileTime();
            var blobPath = $"Results/1/1/1/{QuestionnaireType.CGIDoctor.ToString()}/{fileTime}.json";
            var command = new UploadAnswersCommand(blobPath, new AnswersDto(), new ClaimsPrincipal(), new LogRow());

            await Assert.ThrowsAsync<ClientException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task ThrowException_WhenDatabaseFails()
        {
            var questionnaire = new Questionnaire { QuestionnaireType = QuestionnaireType.CGIDoctor };

            _unitOfWorkMock.Setup(uow => uow.Repository<Questionnaire>().GetByIdAsync(It.IsAny<int>(), false))
                .ReturnsAsync(questionnaire);

            _blobStorageMock.Setup(bs => bs.UploadAsync(
                It.IsAny<string>(),
                It.IsAny<MemoryStream>(),
                It.IsAny<IDictionary<string, string>>(),
                It.IsAny<bool>(),
                It.IsAny<bool>(),
                It.IsAny<LogRow>()
                ))
                .ReturnsAsync(true);

            _unitOfWorkMock.Setup(uow => uow.SaveAsync(It.IsAny<CancellationToken>(), It.IsAny<int>(), It.IsAny<string>()))
                .ThrowsAsync(new Exception("Database failure"));


            var fileTime = DateTime.Now.ToFileTime();
            var blobPath = $"Results/1/1/1/{QuestionnaireType.CGIDoctor.ToString()}/{fileTime}.json";
            var command = new UploadAnswersCommand(blobPath, new AnswersDto(), new ClaimsPrincipal(), new LogRow());

            await Assert.ThrowsAsync<ClientException>(() => _handler.Handle(command, CancellationToken.None));

            _unitOfWorkMock.Verify(uow => uow.RollbackAsync(), Times.Once);
        }

        [Fact]
        public async Task ThrowException_WhenUnexpectedError()
        {
            var questionnaire = new Questionnaire { QuestionnaireType = QuestionnaireType.CGIDoctor };

            _unitOfWorkMock.Setup(uow => uow.Repository<Questionnaire>().GetByIdAsync(It.IsAny<int>(), false))
                .ReturnsAsync(questionnaire);

            _blobStorageMock.Setup(bs => bs.UploadAsync(
                It.IsAny<string>(),
                It.IsAny<MemoryStream>(),
                It.IsAny<IDictionary<string, string>>(),
                It.IsAny<bool>(),
                It.IsAny<bool>(),
                It.IsAny<LogRow>()
                ))
                .ReturnsAsync(true);

            _unitOfWorkMock.Setup(uow => uow.Repository<Questionnaire>().UpdateAsync(It.IsAny<Questionnaire>(), false))
                .ThrowsAsync(new Exception("Unexpected error"));

            var fileTime = DateTime.Now.ToFileTime();
            var blobPath = $"Results/1/1/1/{QuestionnaireType.CGIDoctor.ToString()}/{fileTime}.json";
            var command = new UploadAnswersCommand(blobPath, new AnswersDto(), new ClaimsPrincipal(), new LogRow());

            await Assert.ThrowsAsync<ClientException>(() => _handler.Handle(command, CancellationToken.None));

            _unitOfWorkMock.Verify(uow => uow.RollbackAsync(), Times.Once);
        }
    }
}
