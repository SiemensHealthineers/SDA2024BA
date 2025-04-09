using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MediatR;

using Microsoft.AspNetCore.Mvc;

using Moq;

using NeuroMedia.API.Controllers;
using NeuroMedia.Application.Features.VisitDetails.Queries.GetVisitDetailsById;
using FluentAssertions;
using NeuroMedia.Application.Features.Visits.Queries.GetActualVisit;
using NeuroMedia.Domain.Entities;
using NeuroMedia.Domain.Enums;

namespace NeuroMedia.API.Tests.Controllers
{
    public class VisitDetailsControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly VisitDetailsController _controller;

        public VisitDetailsControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new VisitDetailsController(_mediatorMock.Object);
        }

        [Fact]
        public async Task GetById_ShouldReturnOk_WhenVisitDetailsFound()
        {
            // Arrange
            int patientId = 1;
            int visitId = 1;
            var visitDetails = new GetVisitDetailsByIdDto
            {
                Id = visitId,
                PatientId = patientId,
                Note = $"{visitId}_{patientId}"
            };

            _mediatorMock.Setup(
                m => m.Send(It.IsAny<GetVisitDetailsByIdQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(visitDetails);

            // Act
            var result = await _controller.GetById(patientId, visitId);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult.Value.Should().Be(visitDetails);

            _mediatorMock.Verify(m => m.Send(It.Is<GetVisitDetailsByIdQuery>(q => q.PatientId == patientId && q.Id == visitId), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetActualVisit_ShouldReturnOk_WhenActualVisitFound()
        {
            // Arrange
            var patientId = 1;
            var actualVisit = new GetActualVisitDto
            {
                DateOfVisit = DateTime.UtcNow.Date,
                VisitType = VisitType.Personal,
                Questionnaires = []
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<GetActualVisitQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(actualVisit);

            // Act
            var result = await _controller.GetActualVisit(patientId);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult.Value.Should().Be(actualVisit);

            _mediatorMock.Verify(m => m.Send(It.Is<GetActualVisitQuery>(q => q.PatientId == patientId), It.IsAny<CancellationToken>() ), Times.Once);
        }
    }
}
