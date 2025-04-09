using System.Security.Claims;

using AutoMapper;

using Microsoft.Extensions.Logging;

using Moq;

using NeuroMedia.Application.Exceptions;
using NeuroMedia.Application.Features.Patients.Commands.CreatePatient;
using NeuroMedia.Application.Interfaces.Repositories;
using NeuroMedia.Application.Logging;

using NeuroMedia.Domain.Entities;

namespace NeuroMedia.Application.Tests.Features.Patients.Commands.CreatePatient
{
    public class CreatePatientCommandTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<CreatePatientCommandHandler>> _loggerMock;
        private readonly CreatePatientCommandHandler _handler;
        private readonly Mock<IPatientRepository> _patientRepositoryMock;

        public CreatePatientCommandTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<CreatePatientCommandHandler>>();
            _patientRepositoryMock = new Mock<IPatientRepository>();
            _handler = new CreatePatientCommandHandler(_unitOfWorkMock.Object, _patientRepositoryMock.Object, _mapperMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task Patient_Created_WhenDataValid()
        {
            var createDto = new CreatePatientDto
            {
                Name = "John",
                Surname = "Doe",
                DateOfBirth = DateTime.Now.AddYears(-10),
                Sex = Domain.Enums.Sex.Male,
                Diagnosis = Domain.Enums.Diagnoses.OtherDiagnosis,
                DateOfDiagnosis = DateTime.Now.AddDays(-10),
                Email = "john.doe@example.com",
                PhoneNumber = "1234567890",
                HighestEducation = Domain.Enums.EducationLevel.SecondarySchoolWithDiploma,
                EmploymentStatus = Domain.Enums.EmploymentStatus.Retired,
                RapExamination = false,
                PreviousBotulinumToxinApplication = true
            };
            var claims = new ClaimsPrincipal();
            var logRow = new LogRow();
            var patient = new Patient();
            var expectedDtoWithId = new CreatePatientDtoWithId
            {
                Email = createDto.Email,
                Id = 1
            };

            _patientRepositoryMock.Setup(pr => pr.GetByEmailAsync(createDto.Email)).ReturnsAsync((Patient?) null);
            _patientRepositoryMock.Setup(pr => pr.AddAsync(It.IsAny<Patient>())).ReturnsAsync(patient);

            _mapperMock.Setup(m => m.Map<Patient>(createDto))
                .Returns(patient);
            _mapperMock.Setup(m => m.Map<CreatePatientDto>(patient))
                .Returns(expectedDtoWithId);

            var result = await _handler.Handle(new CreatePatientCommand(createDto, claims, logRow), CancellationToken.None);

            Assert.NotNull(result);
            Assert.IsType<CreatePatientDtoWithId>(result);
            Assert.Equal(expectedDtoWithId.Email, result.Email);

            _patientRepositoryMock.Verify(pr => pr.GetByEmailAsync(createDto.Email), Times.Once);
            _patientRepositoryMock.Verify(pr => pr.AddAsync(It.IsAny<Patient>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveAsync(It.IsAny<CancellationToken>(), It.IsAny<int>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task ThrowException_WhenEmailAlreadyExists()
        {
            var createDto = new CreatePatientDto { Email = "existingemail@example.com" };
            var claims = new ClaimsPrincipal();
            var logRow = new LogRow();
            var existingPatient = new Patient();

            _patientRepositoryMock.Setup(pr => pr.GetByEmailAsync(createDto.Email))
                .ReturnsAsync(existingPatient);

            var exception = await Assert.ThrowsAsync<BadRequestClientException>(() =>
                _handler.Handle(new CreatePatientCommand(createDto, claims, logRow), CancellationToken.None));

            Assert.Equal("Attempt to create a patient with an existing email.", exception.Message);
        }

        [Fact]
        public async Task ThrowException_WhenUnexpectedError()
        {
            var dto = new CreatePatientDto { Email = "test@example.com" };
            var command = new CreatePatientCommand(dto, new ClaimsPrincipal(), new LogRow());

            _patientRepositoryMock.Setup(pr => pr.GetByEmailAsync(dto.Email))
                .ReturnsAsync((Patient?) null);
            _patientRepositoryMock.Setup(pr => pr.AddAsync(It.IsAny<Patient>()))
                .ThrowsAsync(new Exception("Unexpected exception"));

            _mapperMock.Setup(x => x.Map<Patient>(dto))
                .Returns(new Patient());

            var exception = await Assert.ThrowsAsync<ClientException>(() => _handler.Handle(command, CancellationToken.None));

            Assert.Equal("Patient creation failed", exception.Message);
        }
    }
}
