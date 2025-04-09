using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using AutoMapper;

using Microsoft.Extensions.Logging;

using Moq;

using NeuroMedia.Application.Exceptions;
using NeuroMedia.Application.Features.Questionnaires.Dtos;
using NeuroMedia.Application.Features.Questionnaires.Helpers;
using NeuroMedia.Application.Features.Questionnaires.Queries.GetQuestionnaireByType;
using NeuroMedia.Application.Interfaces.Blobstorages;
using NeuroMedia.Application.Interfaces.Repositories;
using NeuroMedia.Application.Logging;
using NeuroMedia.Domain.Enums;

namespace NeuroMedia.Application.Tests.Features.Questionnaires.Queries.GetQuestionnaireByType
{
    public class GetQuestionnaireByTypeQueryTests
    {
        private readonly Mock<IBlobStorage> _blobStorageMock;
        private readonly Mock<ILogger<GetQuestionnaireByTypeQueryHandler>> _loggerMock;
        private readonly GetQuestionnaireByTypeQueryHandler _handler;
        private readonly ClaimsPrincipal _claims;
        private readonly LogRow _logRow;
        private static readonly QuestionnaireType s_questionnaireType = QuestionnaireType.CGIDoctor;

        private readonly QuestionnaireDto _expectedQuestionnaire = new()
        {
            BlobPath = QuestionnaireHelper.GetQuestionnairePath(s_questionnaireType),
            Questions =
            [
                new QuestionDto
                {
                    Id = 1,
                    Type = "Radio",
                    Text = "Stupeň závažnosti cervikálnej dystónie je podľa vás:",
                    Options =
                    [
                        new() { Id = 1, Text = "extrémne chorý", Value = "1" },
                        new() { Id = 2, Text = "závažne chorý", Value = "2" },
                        new() { Id = 3, Text = "zjavne chorý", Value = "3" },
                        new() { Id = 4, Text = "stredne chorý", Value = "4" },
                        new() { Id = 5, Text = "mierne chorý", Value = "5" },
                        new() { Id = 6, Text = "hranične chorý", Value = "6" }
                    ]
                },
                new QuestionDto
                {
                    Id = 2,
                    Type = "Radio",
                    Text = "Stav pacienta súvisiaci s cervikálnou dystóniou je po podaní botulotoxínu:",
                    Options =
                    [
                        new() { Id = 1, Text = "veľmi zlepšený", Value = "1" },
                        new() { Id = 2, Text = "zlepšený", Value = "2" },
                        new() { Id = 3, Text = "minimálne zlepšený", Value = "3" },
                        new() { Id = 4, Text = "bez zmeny", Value = "4" },
                        new() { Id = 5, Text = "minimálne zhoršený", Value = "5" },
                        new() { Id = 6, Text = "zhoršený", Value = "6" },
                        new() { Id = 7, Text = "výrazne zhoršený", Value = "7" }
                    ]
                }
            ]
        };


        public GetQuestionnaireByTypeQueryTests()
        {
            _blobStorageMock = new Mock<IBlobStorage>();
            _loggerMock = new Mock<ILogger<GetQuestionnaireByTypeQueryHandler>> { CallBase = true };
            _handler = new GetQuestionnaireByTypeQueryHandler(_blobStorageMock.Object, _loggerMock.Object);
            _claims = new ClaimsPrincipal();
            _logRow = new LogRow();
        }


        [Fact]
        public async Task Handle_ReturnsQuestionnaireDto_WhenBlobExistsAndIsValid()
        {
            // Arrange
            var blobPath = QuestionnaireHelper.GetQuestionnairePath(s_questionnaireType);
            var jsonContent = JsonSerializer.Serialize(_expectedQuestionnaire);
            var blobStream = new MemoryStream(Encoding.UTF8.GetBytes(jsonContent));

            _blobStorageMock.Setup(b => b.DownloadAsync(blobPath, null))
                        .ReturnsAsync(blobStream);

            var query = new GetQuestionnaireByTypeQuery(s_questionnaireType, _claims, _logRow.UpdateCallerProperties());

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(_expectedQuestionnaire.QuestionnaireType, result.QuestionnaireType);

            _blobStorageMock.Reset();
        }


        [Fact]
        public async Task Handle_ThrowsClientException_WhenDeserializationResultsInNull()
        {
            // Arrange
            var query = new GetQuestionnaireByTypeQuery(s_questionnaireType, _claims, _logRow.UpdateCallerProperties());
            var blobContent = "null";
            var blobPath = QuestionnaireHelper.GetQuestionnairePath(s_questionnaireType);
            var blobStream = new MemoryStream(Encoding.UTF8.GetBytes(blobContent));

            _blobStorageMock.Setup(x => x.DownloadAsync(blobPath, null))
                .ReturnsAsync(blobStream);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<BadRequestClientException>(() => _handler.Handle(query, CancellationToken.None));
            Assert.Equal(exception.Message, $"Content is invalid for path: {blobPath}");

        }


        [Fact]
        public async Task Handle_ThrowsException_WhenDeserializationFails()
        {
            // Arrange
            var query = new GetQuestionnaireByTypeQuery(QuestionnaireType.CGIDoctor, _claims, _logRow.UpdateCallerProperties());
            var invalidJson = "Invalid JSON";
            var blobPath = QuestionnaireHelper.GetQuestionnairePath(s_questionnaireType);
            var blobStream = new MemoryStream(Encoding.UTF8.GetBytes(invalidJson));

            _blobStorageMock.Setup(x => x.DownloadAsync(blobPath, null))
                .ReturnsAsync(blobStream);

            // Act & Assert
            await Assert.ThrowsAsync<ClientException>(() => _handler.Handle(query, CancellationToken.None));
        }
    }
}
