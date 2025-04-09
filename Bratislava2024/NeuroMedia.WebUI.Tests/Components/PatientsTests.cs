using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuroMedia.WebUI.Pages;
using Xunit;
using Bunit;
using Moq;
using Moq.Protected;
using NeuroMedia.Application.Features.Visits.Queries;
using System.Net.Http.Json;
using System.Net;
using NeuroMedia.Application.Features.Patients.Queries.GetAllPatients;
using NeuroMedia.Application.Features.Patients.Queries.GetPatientById;
using NeuroMedia.Domain.Entities;
using Azure.Storage.Blobs.Models;
using NeuroMedia.Domain.Enums;
using Microsoft.Extensions.DependencyInjection;
using AngleSharp.Dom;
using NeuroMedia.WebUI.Services.Interfaces;

namespace NeuroMedia.WebUI.Tests.Components
{
    public class PatientsTests : TestContext
    {
        private readonly Mock<IHttpClientService> _httpClientServiceMock;
        private readonly Pages.Patients _patientsComponent;

        public PatientsTests()
        {
            _httpClientServiceMock = new Mock<IHttpClientService>();
            _patientsComponent = new Pages.Patients
            {
                HttpClientService = _httpClientServiceMock.Object
            };

            Services.AddSingleton(_httpClientServiceMock.Object);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task GetAllStatusMatchingPatients_ReturnsPatients_OnSuccess(bool areActive)
        {
            // Arrange
            var expectedPatients = new List<GetAllPatientsDto>();
            for (var i = 1; i <= 4; i++)
            {
                expectedPatients.Add(
                        new GetAllPatientsDto()
                        {
                            Id = i,
                            Name = $"Test{i}",
                            Surname = "User",
                            DateOfBirth = new DateTime(1990, i, 1),
                            Email = $"test{i}.user@test.com",
                            IsActive = areActive
                        }
                    );
            }

            var url = areActive ? "api/patients/deactivated" : "api/patients";
            _httpClientServiceMock
                .Setup(service => service.GetListAsync<GetAllPatientsDto>(url, It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedPatients);

            // Act
            var result = await _patientsComponent.GetAllStatusMatchingPatients(areActive);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedPatients.Count, result.Count);
            for (var i = 0; i < expectedPatients.Count; i++)
            {
                Assert.Equal(expectedPatients[i].Id, result[i].Id);
                Assert.Equal(expectedPatients[i].Name, result[i].Name);
                Assert.Equal(expectedPatients[i].Surname, result[i].Surname);
                Assert.Equal(expectedPatients[i].DateOfBirth, result[i].DateOfBirth);
                Assert.Equal(expectedPatients[i].Sex, result[i].Sex);
                Assert.Equal(expectedPatients[i].Email, result[i].Email);
                Assert.Equal(expectedPatients[i].IsActive, result[i].IsActive);
            }
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task GetAllStatusMatchingPatients_ReturnsPatients_OnApiFailure(bool areActive)
        {
            // Arrange
            var url = areActive ? "api/patients/deactivated" : "api/patients";
            _httpClientServiceMock
                .Setup(service => service.GetListAsync<GetAllPatientsDto>(url, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new HttpRequestException());

            // Act & Assert
            await Assert.ThrowsAsync<HttpRequestException>(() => _patientsComponent.GetAllStatusMatchingPatients(areActive));
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task GetAllStatusMatchingPatients_ReturnsEmptyList_OnNoMatchingPatient(bool areActive)
        {
            // Arrange
            var url = areActive ? "api/patients/deactivated" : "api/patients";
            _httpClientServiceMock
                .Setup(service => service.GetListAsync<GetAllPatientsDto>(url, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<GetAllPatientsDto>());

            // Act
            var result = await _patientsComponent.GetAllStatusMatchingPatients(areActive);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task GetAllStatusMatchingPatients_ReturnsVisits_OnValidResponse(bool areActive)
        {
            // Arrange
            var expectedPatients = new List<GetAllPatientsDto>
            {
                new()
                {
                    Id = 1,
                    Name = "Test",
                    Surname = "User",
                    DateOfBirth = new DateTime(1990, 1, 1),
                    Email = $"test.user@test.com",
                    IsActive = areActive
                }
            };

            var url = areActive ? "api/patients/deactivated" : "api/patients";
            _httpClientServiceMock
                .Setup(service => service.GetListAsync<GetAllPatientsDto>(url, It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedPatients);

            // Act
            var result = await _patientsComponent.GetAllStatusMatchingPatients(areActive);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(1, result.First().Id);
        }

        [Theory]
        [InlineData(true, 1)]
        [InlineData(false, 2)]
        public async Task GetPatientDetailsById_ReturnsDetails_OnSuccess(bool activePatient, int patientId)
        {
            // Arrange
            var expectedPatient = new GetPatientByIdDto
            {
                Id = patientId,
                Name = "Test",
                Surname = "User",
                DateOfBirth = new DateTime(1990, 1, 1),
                Sex = Domain.Enums.Sex.Male,
                Email = "test.user@test.com",
                IsActive = activePatient
            };

            var expectedUrl = activePatient ? $"api/patients/{patientId}" : $"api/patients/deactivated/{patientId}";
            _httpClientServiceMock
                .Setup(service => service.GetAsync<GetPatientByIdDto>(expectedUrl, It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedPatient);

            // Act
            var result = await _patientsComponent.GetPatientDetailsById(patientId, activePatient);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedPatient.Id, result.Id);
            Assert.Equal(expectedPatient.Name, result.Name);
            Assert.Equal(expectedPatient.Surname, result.Surname);
            Assert.Equal(expectedPatient.DateOfBirth, result.DateOfBirth);
            Assert.Equal(expectedPatient.Sex, result.Sex);
            Assert.Equal(expectedPatient.Email, result.Email);
            Assert.Equal(expectedPatient.IsActive, result.IsActive);
        }


        [Theory]
        [InlineData(true, 1)]
        [InlineData(false, 2)]
        public async Task GetPatientDetailsById_ThrowsException_OnApiFailure(bool activePatient, int patientId)
        {
            // Arrange
            var expectedUrl = activePatient ? $"api/patients/{patientId}" : $"api/patients/deactivated/{patientId}";

            _httpClientServiceMock
                .Setup(service => service.GetAsync<GetPatientByIdDto>(expectedUrl, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new HttpRequestException());

            // Act & Assert
            await Assert.ThrowsAsync<HttpRequestException>(() => _patientsComponent.GetPatientDetailsById(patientId, activePatient));
        }

        [Fact]
        public async Task GetActualPatientGroup_ReturnsActiveString_OnActiveState()
        {
            // Arrange
            var activeState = true;

            // Act && Assert
            Assert.Equal("Active", actual: Pages.Patients.GetActualPatientGroup(activeState));
        }

        [Fact]
        public async Task GetActualPatientGroup_ReturnsDeactivatedString_OnDeactivatedState()
        {
            // Arrange
            var deactivatedState = false;

            // Act && Assert
            Assert.Equal("Deactivated", actual: Pages.Patients.GetActualPatientGroup(deactivatedState));
        }

        [Fact]
        public async Task SetPatientsGroup_ChangesToDeactivated_OnActiveState()
        {
            // Arrange
            var activeState = true;
            var localIndexComponent = new Pages.Patients
            {
                HttpClientService = _httpClientServiceMock.Object,
                ActiveState = activeState
            };

            // Act
            localIndexComponent.SetPatientsGroup();

            // Assert
            Assert.Equal(!activeState, localIndexComponent.ActiveState);
        }

        [Fact]
        public async Task SetPatientsGroup_ChangesToActive_OnDeactivatedState()
        {
            // Arrange
            var activeState = false;
            var localIndexComponent = new Pages.Patients
            {
                HttpClientService = _httpClientServiceMock.Object,
                ActiveState = activeState
            };

            // Act
            localIndexComponent.SetPatientsGroup();

            // Assert
            Assert.Equal(!activeState, localIndexComponent.ActiveState);
        }

        [Fact]
        public async Task GetListOfVisitsByPatientId_ReturnsVisits_OnSuccess()
        {
            // Arrange
            var expectedVisits = new List<GetAllVisitsDto>
            {
                new() { Id = 1, PatientId = 1, DateOfVisit = DateTime.Now },
                new() { Id = 2, PatientId = 1, DateOfVisit = DateTime.Now.AddDays(-1) }
            };

            _httpClientServiceMock
                .Setup(service => service.GetListAsync<GetAllVisitsDto>(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedVisits);

            // Act
            var result = await _patientsComponent.GetListOfVisitsByPatientId(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedVisits.Count, result.Count);
            Assert.Equal(expectedVisits[0].Id, result[0].Id);
        }

        [Fact]
        public async Task GetListOfVisitsByPatientId_ThrowsException_OnApiFailure()
        {
            // Arrange
            _httpClientServiceMock
                .Setup(service => service.GetListAsync<GetAllVisitsDto>(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new HttpRequestException());

            // Act & Assert
            await Assert.ThrowsAsync<HttpRequestException>(() => _patientsComponent.GetListOfVisitsByPatientId(1));
        }

        [Fact]
        public async Task GetListOfVisitsByPatientId_ReturnsEmptyList_OnNoVisits()
        {
            // Arrange
            _httpClientServiceMock
                .Setup(service => service.GetListAsync<GetAllVisitsDto>(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<GetAllVisitsDto>());

            // Act
            var result = await _patientsComponent.GetListOfVisitsByPatientId(1);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetListOfVisitsByPatientId_ReturnsVisits_OnValidResponse()
        {
            // Arrange
            var visits = new List<GetAllVisitsDto>
            {
                new() { Id = 1, PatientId = 1, DateOfVisit = DateTime.Now, Note = "Test Note" }
            };

            _httpClientServiceMock
                .Setup(service => service.GetListAsync<GetAllVisitsDto>(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(visits);

            // Act
            var result = await _patientsComponent.GetListOfVisitsByPatientId(1);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(1, result.First().Id);
        }
    }
}
