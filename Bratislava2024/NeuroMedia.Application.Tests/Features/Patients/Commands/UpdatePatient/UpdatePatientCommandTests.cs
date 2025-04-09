using NeuroMedia.Application.Features.Patients.Commands.UpdatePatient;
using NeuroMedia.Domain.Entities;

using Moq;
using AutoMapper;


using NeuroMedia.Application.Interfaces.Repositories;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using NeuroMedia.Application.Logging;
using NeuroMedia.Application.Exceptions;
using System.ComponentModel.DataAnnotations;
using NeuroMedia.Application.Features.Patients.Commands.CreatePatient;

namespace NeuroMedia.Application.Tests.Features.Patients.Commands.UpdatePatient
{
    public class UpdatePatientCommandTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<UpdatePatientCommandHandler>> _loggerMock;
        private readonly UpdatePatientCommandHandler _handler;
        private readonly Mock<IPatientRepository> _patientRepositoryMock;

        public UpdatePatientCommandTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<UpdatePatientCommandHandler>>();
            _patientRepositoryMock = new Mock<IPatientRepository>();
            _handler = new UpdatePatientCommandHandler(_unitOfWorkMock.Object, _patientRepositoryMock.Object, _mapperMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task Patient_Updated_WhenDataValid()
        {
            var patientId = 1;
            var updateDto = new UpdatePatientDto { Email = "newemail@example.com" };
            var claims = new ClaimsPrincipal();
            var logRow = new LogRow();
            var patient = new Patient { Id = patientId, Email = "oldemail@example.com" };
            var newPatient = new Patient { Id = patientId, Email = updateDto.Email };

            _patientRepositoryMock.Setup(pr => pr.GetByIdAsync(patientId, false)).ReturnsAsync(patient);
            _patientRepositoryMock.Setup(pr => pr.GetByEmailAsync(updateDto.Email, patientId)).ReturnsAsync((Patient?) null);
            _patientRepositoryMock.Setup(pr => pr.UpdateAsync(It.IsAny<Patient>(), false)).Returns(Task.CompletedTask);

            _mapperMock.Setup(m => m.Map<UpdatePatientDto, Patient?>(updateDto, patient))
                .Returns(newPatient);
            _mapperMock.Setup(m => m.Map<UpdatePatientDto>(newPatient))
                .Returns(updateDto);

            var result = await _handler.Handle(new UpdatePatientCommand(patientId, updateDto, claims, logRow), CancellationToken.None);

            Assert.Equal(updateDto, result);
            _patientRepositoryMock.Verify(pr => pr.UpdateAsync(newPatient, false), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveAsync(It.IsAny<CancellationToken>(), It.IsAny<int>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task ThrowException_WhenPatientNotFound()
        {
            var patientId = 1;
            var updateDto = new UpdatePatientDto { Email = "newemail@example.com" };
            var claims = new ClaimsPrincipal();
            var logRow = new LogRow();

            _patientRepositoryMock.Setup(pr => pr.GetByIdAsync(patientId, false)).ReturnsAsync((Patient?) null);

            var exception = await Assert.ThrowsAsync<NotFoundClientException>(() =>
            _handler.Handle(new UpdatePatientCommand(patientId, updateDto, claims, logRow), CancellationToken.None));

            Assert.Equal("Patient not found", exception.Message);
        }

        [Fact]
        public async Task ThrowException_WhenEmailAlreadyExists()
        {
            var patientId = 1;
            var updateDto = new UpdatePatientDto { Email = "existingemail@example.com" };
            var claims = new ClaimsPrincipal();
            var logRow = new LogRow();
            var existingPatient = new Patient();

            _patientRepositoryMock.Setup(pr => pr.GetByIdAsync(patientId, false)).ReturnsAsync(new Patient { Id = patientId });
            _patientRepositoryMock.Setup(pr => pr.GetByEmailAsync(updateDto.Email, patientId)).ReturnsAsync(existingPatient);

            var exception = await Assert.ThrowsAsync<BadRequestClientException>(() =>
                _handler.Handle(new UpdatePatientCommand(patientId, updateDto, claims, logRow), CancellationToken.None));

            Assert.Equal($"Attempt to update patient with an existing email.", exception.Message);
        }

        [Fact]
        public async Task ThrowException_WhenUnexpectedError()
        {
            var dto = new UpdatePatientDto { Email = "test@example.com" };
            var command = new UpdatePatientCommand(1, dto, new ClaimsPrincipal(), new LogRow());

            _patientRepositoryMock.Setup(pr => pr.GetByIdAsync(command.Id, false)).ReturnsAsync(new Patient());
            _patientRepositoryMock.Setup(pr => pr.GetByEmailAsync(dto.Email, command.Id)).ReturnsAsync((Patient?) null);
            _patientRepositoryMock.Setup(pr => pr.UpdateAsync(It.IsAny<Patient>(), false)).ThrowsAsync(new Exception("Unexpected exception"));

            var exception = await Assert.ThrowsAsync<ClientException>(() => _handler.Handle(command, CancellationToken.None));
            Assert.Equal("Patient update failed", exception.Message);
        }
    }
}
