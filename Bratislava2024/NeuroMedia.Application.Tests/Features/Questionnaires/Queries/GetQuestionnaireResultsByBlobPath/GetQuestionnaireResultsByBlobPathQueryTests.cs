using System.Text;
using System.Security.Claims;

using Moq;

using NeuroMedia.Application.Logging;
using NeuroMedia.Domain.Enums;
using NeuroMedia.Application.Interfaces.Blobstorages;
using Microsoft.Extensions.Logging;
using NeuroMedia.Application.Exceptions;
using NeuroMedia.Application.Features.Questionnaires.Queries.GetQuestionnaireResultsByBlobPath;

namespace NeuroMedia.Application.Tests.Features.Questionnaires.Queries.GetQuestionnaireById
{
    public class GetQuestionnaireResultsByBlobPathQueryTests
    {
        private readonly Mock<IBlobStorage> _blobStorageMock;
        private readonly GetQuestionnaireResultsByBlobPathHandler _handler;
        private readonly Mock<ILogger<GetQuestionnaireResultsByBlobPathHandler>> _loggerMock;

        public GetQuestionnaireResultsByBlobPathQueryTests()
        {
            _blobStorageMock = new Mock<IBlobStorage>();
            _loggerMock = new Mock<ILogger<GetQuestionnaireResultsByBlobPathHandler>>();
            _handler = new GetQuestionnaireResultsByBlobPathHandler(_blobStorageMock.Object, _loggerMock.Object);

        }


        [Fact]
        public async Task Handle_ReturnsAnswersDto_WhenBlobExists()
        {
            // Arrange
            var type = QuestionnaireType.CGIPatient;
            var blobPath = $"Results/1/1/1/{type.ToString()}/{DateTime.Now.ToFileTime()}.json";
            var jsonContent = "{\"Answers\" : [ { \"OptionId\" : 1, \"QuestionId\" : 1, \"ResultValue\" : \"Sample Value\" }]}";
            var blobStream = new MemoryStream(Encoding.UTF8.GetBytes(jsonContent));

            var claimsPrincipal = new ClaimsPrincipal();
            var logRow = new LogRow();

            _blobStorageMock
                .Setup(b => b.DownloadAsync(blobPath, null))
                .ReturnsAsync(blobStream);

            var query = new GetQuestionnaireResultsByBlobPathQuery(blobPath, claimsPrincipal, logRow);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result.Answers);
            Assert.Equal(1, result.Answers.First().OptionId);
            Assert.Equal(1, result.Answers.First().QuestionId);
            Assert.Equal("Sample Value", result.Answers.First().ResultValue);
        }


        [Fact]
        public async Task Handle_ReturnsEmptyAnswerDto_WhenBlobDoesNotExist()
        {
            // Arrange
            var dateTime = DateTime.Now.ToFileTime();
            var questionnaireType = QuestionnaireType.CGIDoctor;
            var blobPath = $"Results/1/1/1/{questionnaireType}/{dateTime}.json";

            var claimsPrincipal = new ClaimsPrincipal();
            var logRow = new LogRow();

            _blobStorageMock
                .Setup(b => b.DownloadAsync(blobPath, null))
                .ReturnsAsync(new MemoryStream());

            var query = new GetQuestionnaireResultsByBlobPathQuery(blobPath, claimsPrincipal, logRow);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result.Answers);
        }


        [Fact]
        public async Task Handle_ThrowsClientException_WhenJsonIsInvalid()
        {
            // Arrange
            var dateTime = DateTime.Now.ToFileTime();
            var questionnaireType = QuestionnaireType.CGIDoctor;
            var blobPath = $"Results/1/1/1/{questionnaireType}/{dateTime}.json";

            var invalidJsonContent = "{\"Answers\" : [ { \"OptionId\" : 1, \"QuestionId\" : 1 \"ResultValue\" : \"Sample Value\" }]}";
            var blobStream = new MemoryStream(Encoding.UTF8.GetBytes(invalidJsonContent));
            var claimsPrincipal = new ClaimsPrincipal();
            var logRow = new LogRow();

            _blobStorageMock
                .Setup(b => b.DownloadAsync(blobPath, null))
                .ReturnsAsync(blobStream);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ClientException>(() => _handler.Handle(new GetQuestionnaireResultsByBlobPathQuery(blobPath, claimsPrincipal, logRow), CancellationToken.None));
            Assert.Equal("Invalid JSON format on response", exception.Message);
        }


        [Fact]
        public async Task Handle_ReturnsEmptyAnswerDto_WhenBlobPathIsNull()
        {
            // Arrange
            var claimsPrincipal = new ClaimsPrincipal();
            var logRow = new LogRow();

            var query = new GetQuestionnaireResultsByBlobPathQuery(null, claimsPrincipal, logRow);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result.Answers);
        }

        [Fact]
        public async Task Handle_ThrowsBadRequestException_WhenBlobPathIsInvalid()
        {
            // Arrange
            var dateTime = DateTime.Now.ToFileTime();
            var questionnaireType = QuestionnaireType.CGIDoctor;
            var blobPath = $"Result/1/1/1/{questionnaireType}/{dateTime}.json";

            var claimsPrincipal = new ClaimsPrincipal();
            var logRow = new LogRow();

            var query = new GetQuestionnaireResultsByBlobPathQuery(null, claimsPrincipal, logRow);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<BadRequestClientException>(() => _handler.Handle(new GetQuestionnaireResultsByBlobPathQuery(blobPath, claimsPrincipal, logRow), CancellationToken.None));
            Assert.Equal("Invalid blob path", exception.Message);
        }

        [Fact]
        public async Task Handle_ReturnsEmptyAnswerDto_WhenBlobPathPointsToDirectory()
        {
            // Arrange
            var dateTime = DateTime.Now.ToFileTime();
            var questionnaireType = QuestionnaireType.CGIDoctor;
            var blobPath = $"Results/1/1/1/{questionnaireType}/{dateTime}.json";

            var claimsPrincipal = new ClaimsPrincipal();
            var logRow = new LogRow();

            _blobStorageMock
                .Setup(b => b.DownloadAsync(blobPath, null))
                .ReturnsAsync(new MemoryStream());

            var query = new GetQuestionnaireResultsByBlobPathQuery(blobPath, claimsPrincipal, logRow);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result.Answers);
        }


        [Fact]
        public async Task Handle_ReturnsAnswersDto_WhenBlobContentIsLarge()
        {
            // Arrange
            var dateTime = DateTime.Now.ToFileTime();
            var questionnaireType = QuestionnaireType.CGIDoctor;
            var blobPath = $"Results/1/1/1/{questionnaireType}/{dateTime}.json";

            var largeJsonContent = new StringBuilder("{\"Answers\" : [");
            for (var i = 0; i < 1000; i++)
            {
                if (i is > 0)
                {
                    largeJsonContent.Append(",");
                }
                largeJsonContent.Append($"{{\"AnswerId\": {i}, \"QuestionId\": {i}, \"ResultValue\": \"Sample Value {i}\"}}");
            }
            largeJsonContent.Append("]}");

            var blobStream = new MemoryStream(Encoding.UTF8.GetBytes(largeJsonContent.ToString()));
            var claimsPrincipal = new ClaimsPrincipal();
            var logRow = new LogRow();

            _blobStorageMock
                .Setup(b => b.DownloadAsync(blobPath, null))
                .ReturnsAsync(blobStream);

            var query = new GetQuestionnaireResultsByBlobPathQuery(blobPath, claimsPrincipal, logRow);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1000, result.Answers.Count());
        }


        [Fact]
        public async Task Handle_ThrowsClientException_WhenStreamReadFails()
        {
            // Arrange
            var dateTime = DateTime.Now.ToFileTime();
            var questionnaireType = QuestionnaireType.CGIDoctor;
            var blobPath = $"Results/1/1/1/{questionnaireType}/{dateTime}.json";

            var claimsPrincipal = new ClaimsPrincipal();
            var logRow = new LogRow();

            var blobStorageMock = new Mock<IBlobStorage>();
            blobStorageMock
                .Setup(b => b.DownloadAsync(blobPath, null))
                .ThrowsAsync(new IOException("Stream read error"));

            var loggerMock = new Mock<ILogger<GetQuestionnaireResultsByBlobPathHandler>>();

            var handler = new GetQuestionnaireResultsByBlobPathHandler(blobStorageMock.Object, loggerMock.Object);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ClientException>(() =>
                handler.Handle(new GetQuestionnaireResultsByBlobPathQuery(blobPath, claimsPrincipal, logRow), CancellationToken.None));

            Assert.Equal("Error reading stream", exception.Message);
        }
    }
}
