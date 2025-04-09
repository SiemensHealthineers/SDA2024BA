using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuroMedia.WebUI.Pages;

using Moq;
using Moq.Protected;
using NeuroMedia.Application.Features.Visits.Queries;
using System.Net.Http.Json;
using System.Net;
using static System.Net.WebRequestMethods;
using NeuroMedia.Application.Features.VisitDetails.Queries.GetVisitDetailsById;
using NeuroMedia.WebUI.Services.Interfaces;
using NeuroMedia.Application.Features.Patients.Queries.GetAllPatients;

namespace NeuroMedia.WebUI.Tests.Components
{
    public class VisitPersonalTests
    {
        private readonly Mock<IHttpClientService> _httpClientServiceMock;
        private readonly Pages.VisitPersonal _VisitPersonalComponent;

        public VisitPersonalTests()
        {
            _httpClientServiceMock = new Mock<IHttpClientService>();
            _VisitPersonalComponent = new Pages.VisitPersonal
            {
                HttpClientService = _httpClientServiceMock.Object
            };
        }

        [Fact]
        public async Task GetVisitDetailsByPatientIdVisitId_Returns_VisitDetails_OnSuccess()
        {
            //Arrange
            var expectedVisitDetails = new GetVisitDetailsByIdDto
            {
                DateOfVisit = DateTime.Now,
                Id = 1,
                PatientId = 1,
                Note = "Everything okey",
                Questionnaires = [],
                Videos = []
            };

            _httpClientServiceMock
                .Setup(service => service.GetAsync<GetVisitDetailsByIdDto>(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedVisitDetails);

            //Act
            var result = await _VisitPersonalComponent.GetVisitDetails(1, 1);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(expectedVisitDetails.Note, result.Note);
            Assert.Equal(expectedVisitDetails.Id, result.Id);
            Assert.Equal(expectedVisitDetails.DateOfVisit, result.DateOfVisit);
        }

        [Fact]
        public async Task GetVisitDetailsByPatientIdVisitId_Returns_VisitDetails_OnApiFailure()
        {
            // Arrange
            _httpClientServiceMock
                .Setup(service => service.GetAsync<GetVisitDetailsByIdDto>(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new HttpRequestException());

            // Act
            var result = await _VisitPersonalComponent.GetVisitDetails(1, 1);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetVisitDetailsByPatientIdVisitId_Returns_VisitDetails_OnNoDetails()
        {
            //Arrange
            _httpClientServiceMock
                .Setup(service => service.GetAsync<GetVisitDetailsByIdDto>(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GetVisitDetailsByIdDto());

            //Act
            var result = await _VisitPersonalComponent.GetVisitDetails(1, 1);

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetVisitDetailsByPatientIdVisitId_Returns_VisitDetails_OnValidResponse()
        {
            //Arrange
            var expectedVisitDetails = new GetVisitDetailsByIdDto
            {
                DateOfVisit = DateTime.Now,
                Id = 1,
                PatientId = 1,
                Note = "Everything okey",
                Questionnaires = [],
                Videos = []
            };

            _httpClientServiceMock
                .Setup(service => service.GetAsync<GetVisitDetailsByIdDto>(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedVisitDetails);

            //Act
            var result = await _VisitPersonalComponent.GetVisitDetails(1, 1);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal(1, result.PatientId);
        }
    }

}
