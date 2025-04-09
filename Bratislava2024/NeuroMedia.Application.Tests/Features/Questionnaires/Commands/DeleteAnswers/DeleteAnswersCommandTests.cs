using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;

using NeuroMedia.Application.Features.Questionnaires.Commands.DeleteAnswers;
using NeuroMedia.Application.Features.Questionnaires.Commands.UploadAnswers;
using NeuroMedia.Application.Interfaces.Blobstorages;
using NeuroMedia.Application.Interfaces.Repositories;
using NeuroMedia.Application.Logging;
using NeuroMedia.Domain.Entities;
using NeuroMedia.Domain.Enums;
using System.Security.Claims;


using System.Reflection.Metadata;
using NeuroMedia.Application.Exceptions;
using NeuroMedia.Application.Features.Questionnaires.Dtos;

namespace NeuroMedia.Application.Tests.Features.Questionnaires.Commands.DeleteAnswers
{
    public class DeleteAnswersCommandTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IQuestionnaireRepository> _questionnaireRepositoryMock;
        private readonly Mock<IBlobStorage> _blobStorageMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<DeleteAnswersCommandHandler>> _loggerMock;
        private readonly DeleteAnswersCommandHandler _handler;

        public DeleteAnswersCommandTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _questionnaireRepositoryMock = new Mock<IQuestionnaireRepository>();
            _blobStorageMock = new Mock<IBlobStorage>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<DeleteAnswersCommandHandler>>();
            _handler = new DeleteAnswersCommandHandler(_unitOfWorkMock.Object, _questionnaireRepositoryMock.Object, _blobStorageMock.Object, _mapperMock.Object, _loggerMock.Object);
        }
        [Fact]
        public async Task Answers_Deleted_WhenDataValid()
        {
            var fileTime = DateTime.Now.ToFileTime();
            var blobPath = $"Results/1/1/1/{QuestionnaireType.Botulotoxin.ToString()}/{fileTime}.json";
            var visit = new Visit();
            var questionnaire = new Questionnaire { BlobPath = blobPath };
            var resultsDto = new QuestionnaireRecordDto();

            _unitOfWorkMock.Setup(uow => uow.Repository<Questionnaire>().GetByIdAsync(It.IsAny<int>(), false))
            .ReturnsAsync(questionnaire);

            _questionnaireRepositoryMock.Setup(r => r.GetByIdIncludeVisitAsync(It.IsAny<int>()))
            .ReturnsAsync(new Questionnaire { BlobPath = blobPath, Visit = visit });

            _unitOfWorkMock.Setup(uow => uow.Repository<Visit>().GetByIdAsync(It.IsAny<int>(), false))
            .ReturnsAsync(visit);

            _blobStorageMock.Setup(bs => bs.DeleteAsync(It.IsAny<string>(), It.IsAny<LogRow>()))
                .ReturnsAsync(true);

            _unitOfWorkMock.Setup(uow => uow.Repository<Questionnaire>().UpdateAsync(It.IsAny<Questionnaire>(), false))
            .Returns(Task.CompletedTask);

            _unitOfWorkMock.Setup(uow => uow.SaveAsync(It.IsAny<CancellationToken>(), It.IsAny<int>(), It.IsAny<string>()))
                .ReturnsAsync(1);


            _unitOfWorkMock.Setup(uow => uow.Repository<Questionnaire>().GetByIdAsync(It.IsAny<int>(), false))
                .ReturnsAsync(questionnaire);

            _mapperMock.Setup(m => m.Map<QuestionnaireRecordDto>(It.IsAny<Questionnaire>()))
                .Returns(resultsDto);

            var command = new DeleteAnswersCommand(1, new ClaimsPrincipal(), new LogRow());

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(resultsDto, result);
        }

        [Fact]
        public async Task ThrowException_WhenQuestionnaireNotFound()
        {
            _unitOfWorkMock.Setup(uow => uow.Repository<Questionnaire>().GetByIdAsync(It.IsAny<int>(), false))
                .ReturnsAsync((Questionnaire?) null);

            var command = new DeleteAnswersCommand(1, new ClaimsPrincipal(), new LogRow());

            await Assert.ThrowsAsync<NotFoundClientException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task ThrowException_WhenNoResultsToDelete()
        {
            var questionnaire = new Questionnaire { BlobPath = null };
            _unitOfWorkMock.Setup(uow => uow.Repository<Questionnaire>().GetByIdAsync(It.IsAny<int>(), false))
                .ReturnsAsync(questionnaire);

            var command = new DeleteAnswersCommand(1, new ClaimsPrincipal(), new LogRow());

            await Assert.ThrowsAsync<BadRequestClientException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task ThrowException_WhenDeletionFails()
        {
            var fileTime = DateTime.Now.ToFileTime();
            var blobPath = $"Results/1/1/1/{QuestionnaireType.Botulotoxin.ToString()}/{fileTime}.json";
            var questionnaire = new Questionnaire { BlobPath = blobPath};

            _unitOfWorkMock.Setup(uow => uow.Repository<Questionnaire>().GetByIdAsync(It.IsAny<int>(), false))
            .ReturnsAsync(questionnaire);

            _blobStorageMock.Setup(bs => bs.DeleteAsync(It.IsAny<string>(), It.IsAny<LogRow>()))
                .ReturnsAsync(false);

            var command = new DeleteAnswersCommand(1, new ClaimsPrincipal(), new LogRow());

            await Assert.ThrowsAsync<ClientException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task ThrowException_WhenDatabaseFails()
        {
            var fileTime = DateTime.Now.ToFileTime();
            var blobPath = $"Results/1/1/1/{QuestionnaireType.Botulotoxin.ToString()}/{fileTime}.json";
            var questionnaire = new Questionnaire { BlobPath = blobPath };

            _unitOfWorkMock.Setup(uow => uow.Repository<Questionnaire>().GetByIdAsync(It.IsAny<int>(), false))
                .ReturnsAsync(questionnaire);

            _blobStorageMock.Setup(bs => bs.DeleteAsync(It.IsAny<string>(), It.IsAny<LogRow>()))
                .ReturnsAsync(true);

            _unitOfWorkMock.Setup(uow => uow.SaveAsync(It.IsAny<CancellationToken>(), It.IsAny<int>(), It.IsAny<string>()))
                .ThrowsAsync(new Exception());

            var command = new DeleteAnswersCommand(1, new ClaimsPrincipal(), new LogRow());

            await Assert.ThrowsAsync<ClientException>(() => _handler.Handle(command, CancellationToken.None));
            _unitOfWorkMock.Verify(uow => uow.RollbackAsync(), Times.Once);
        }

        [Fact]
        public async Task ThrowException_WhenUnexpectedError()
        {
            var fileTime = DateTime.Now.ToFileTime();
            var blobPath = $"Results/1/1/1/{QuestionnaireType.Botulotoxin.ToString()}/{fileTime}.json";
            var questionnaire = new Questionnaire { BlobPath = blobPath };

            _unitOfWorkMock.Setup(uow => uow.Repository<Questionnaire>().GetByIdAsync(It.IsAny<int>(), false))
                .ReturnsAsync(questionnaire);

            _blobStorageMock.Setup(bs => bs.DeleteAsync(It.IsAny<string>(), It.IsAny<LogRow>()))
                .ReturnsAsync(true);

            _unitOfWorkMock.Setup(uow => uow.Repository<Questionnaire>().UpdateAsync(It.IsAny<Questionnaire>(), false))
                .ThrowsAsync(new Exception());

            var command = new DeleteAnswersCommand(1, new ClaimsPrincipal(), new LogRow());

            await Assert.ThrowsAsync<ClientException>(() => _handler.Handle(command, CancellationToken.None));

            _unitOfWorkMock.Verify(uow => uow.RollbackAsync(), Times.Once);
        }
    }
}
