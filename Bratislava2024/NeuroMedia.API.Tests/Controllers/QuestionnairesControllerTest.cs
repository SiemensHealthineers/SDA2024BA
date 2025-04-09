using FluentAssertions;

using MediatR;

using Microsoft.AspNetCore.Mvc;

using Moq;

using NeuroMedia.API.Controllers;
using NeuroMedia.Application.Features.Questionnaires.Commands.CreateQuestionnaireRecordsCommand;
using NeuroMedia.Application.Features.Questionnaires.Commands.DeleteAnswers;
using NeuroMedia.Application.Features.Questionnaires.Commands.UploadAnswers;
using NeuroMedia.Application.Features.Questionnaires.Dtos;
using NeuroMedia.Application.Features.Questionnaires.Queries.GetQuestionnaireByType;
using NeuroMedia.Application.Features.Questionnaires.Queries.GetQuestionnairesByVisitId;
using NeuroMedia.Application.Interfaces.Blobstorages;
using NeuroMedia.Domain.Enums;
using NeuroMedia.Application.Features.Questionnaires.Queries.GetQuestionnaireResultsByBlobPath;

namespace NeuroMedia.API.Tests.Controllers;

public class QuestionnairesControllerTest
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly QuestionnairesController _controller;
    private readonly Mock<IBlobStorage> _blobStorageMock;


    public QuestionnairesControllerTest()
    {
        _mediatorMock = new Mock<IMediator>();
        _blobStorageMock = new Mock<IBlobStorage>();
        _controller = new QuestionnairesController(_mediatorMock.Object);
    }


    [Fact]
    public async Task GetQuestionnaireByType_ShouldReturnOkWithExpectedDto_WhenQuestionnaireOfTypeIsFound()
    {
        //Arrange
        var type = QuestionnaireType.TWSTRSDoctor;
        var expectedDto = new QuestionnaireDto();
        _mediatorMock.Setup(m => m.Send(It.IsAny<GetQuestionnaireByTypeQuery>(), default))
            .ReturnsAsync(expectedDto);

        //Act
        var result = await _controller.GetQuestionnaireByType(type);

        //Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualDto = Assert.IsType<QuestionnaireDto>(okResult.Value);
        Assert.Equal(expectedDto, actualDto);
        _mediatorMock.Verify(m => m.Send(It.Is<GetQuestionnaireByTypeQuery>(q => q.Type == type), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetAllQuestionnairesResultsByVisitId_ShouldReturnOkWithExpectedList_WhenValidVisitIdProvided()
    {
        //Arrange
        var visitId = 1;
        var expectedDto = new List<QuestionnaireRecordDto> {new QuestionnaireRecordDto()};
        _mediatorMock.Setup(m => m.Send(It.IsAny<GetAllQuestionnairesResultsByVisitIdQuery>(), default))
            .ReturnsAsync(expectedDto);

        //Act
        var result = await _controller.GetAllQuestionnairesResultsByVisitId(visitId);

        //Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualDto = Assert.IsType<List<QuestionnaireRecordDto>>(okResult.Value);
        Assert.Equal(expectedDto, actualDto);
        _mediatorMock.Verify(m => m.Send(
                It.Is<GetAllQuestionnairesResultsByVisitIdQuery>(q => q.VisitId == visitId), // Ensure the query has the correct VisitId
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task GetQuestionnaireResultsByBlobPath_ShouldreturnOkWithExpectedDto_WhenValidBlobPathProvided()
    {
        //Arrange
        var blobPath = "1/2/QuestionnaireDoctor";
        var expectedDto = new AnswersDto();
        _mediatorMock.Setup(m => m.Send(It.IsAny<GetQuestionnaireResultsByBlobPathQuery>(), default))
            .ReturnsAsync(expectedDto);


        //Act
        var result = await _controller.GetQuestionnaireResultsByBlobPath(blobPath);

        //Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualDto = Assert.IsType<AnswersDto>(okResult.Value);
        Assert.Equal(expectedDto, actualDto);
        _mediatorMock.Verify(m => m.Send(
                It.Is<GetQuestionnaireResultsByBlobPathQuery>(q => q.BlobPath == blobPath),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task CreateResultsForVisit_ShouldReturnOkWithExpectedDto_WhenValidVisitIdAndTypeProvided()
    {
        //Arrange
        var visitId = 1;
        QuestionnaireType type = QuestionnaireType.TWSTRSDoctor;
        var expectedDto = new CreateQuestionnaireRecordsDto();
        _mediatorMock.Setup(m => m.Send(It.IsAny<CreateQuestionnaireRecordsCommand>(), default))
            .ReturnsAsync(expectedDto);

        //Act
        var result = await _controller.CreateResultsForVisit(visitId, type);

        //Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualDto = Assert.IsType<CreateQuestionnaireRecordsDto>(okResult.Value);
        Assert.Equal(expectedDto, actualDto);
        _mediatorMock.Verify(m => m.Send(
                It.Is<CreateQuestionnaireRecordsCommand>(q => q.VisitId == visitId && q.Type == type),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task CreateResults_ShouldReturnOkWithExpectedDto_WhenValidBlobPathAndAnswersDtoProvided()
    {
        //Arrange
        var blobPath = "path/of/blob";

        var answersDto = new AnswersDto();
        var expectedDto = new QuestionnaireRecordDto();

        _mediatorMock.Setup(m => m.Send(It.IsAny<UploadAnswersCommand>(), default))
            .ReturnsAsync(expectedDto);

        //Act
        var result = await _controller.CreateResults(blobPath, answersDto);

        //Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualDto = Assert.IsType<QuestionnaireRecordDto>(okResult.Value);
        Assert.Equal(expectedDto, actualDto);
        _mediatorMock.Verify(m => m.Send(
                It.Is<UploadAnswersCommand>(q => q.BlobPath == blobPath && q.Dto == answersDto),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Delete_ShouldReturnOkWithExpectedDto_WhenValidQuestionnaireIdProvided()
    {
        //Arrange
        var questionnaireId = 1;
        var expectedDto = new QuestionnaireRecordDto();
        _mediatorMock.Setup(m => m.Send(It.IsAny<DeleteAnswersCommand>(), default))
            .ReturnsAsync(expectedDto);

        //Act
        var result = await _controller.Delete(questionnaireId);

        //Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualDto = Assert.IsType<QuestionnaireRecordDto>(okResult.Value);
        Assert.Equal(expectedDto, actualDto);
        _mediatorMock.Verify(m => m.Send(
                It.Is<DeleteAnswersCommand>(q => q.QuestionnaireId == questionnaireId),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

}
