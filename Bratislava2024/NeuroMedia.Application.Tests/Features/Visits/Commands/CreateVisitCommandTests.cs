using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AutoMapper;
using Microsoft.Extensions.Logging;

using Moq;
using NeuroMedia.Application.Features.Visits.Commands.CreateVisit;
using NeuroMedia.Domain.Entities;

using NeuroMedia.Application;
using NeuroMedia.Application.Interfaces.Repositories;
using NeuroMedia.Application.Logging;
using System.Security.Claims;
using NeuroMedia.Application.Exceptions;

namespace NeuroMedia.Application.Tests.Features.Visits.Commands
{
    public class CreateVisitCommandTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<CreateVisitCommandHandler>> _loggerMock;
        private readonly CreateVisitCommandHandler _handler;
        private readonly Mock<IPatientRepository> _patientRepositoryMock;
        public CreateVisitCommandTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<CreateVisitCommandHandler>>();
            _patientRepositoryMock = new Mock<IPatientRepository>();
            
            _handler = new CreateVisitCommandHandler(_unitOfWorkMock.Object, _mapperMock.Object, _patientRepositoryMock.Object, _loggerMock.Object);
        }
        [Fact]
        public async Task Visit_Created_WhenDataValid()
        {
            //Arrange
            var patient = new Patient()
            {
                Id = 1
            };
            var createDto = new CreateVisitDto
            {
                PatientId = patient.Id,
                DateOfVisit = DateTime.Now,
                VisitType = 0,
            };
            var claims = new ClaimsPrincipal();
            var logRow = new LogRow();
            var visit = new Visit
            {
                PatientId = createDto.PatientId,
                DateOfVisit = createDto.DateOfVisit,
            };
            var savedVisit = new Visit
            {
                Id = 1,
                PatientId = createDto.PatientId,
                DateOfVisit = createDto.DateOfVisit
            };
            var visitMock = new Mock<IGenericRepository<Visit>>();

            //Act
            _patientRepositoryMock.Setup(p => p.GetByIdAsync(savedVisit.PatientId, false)).ReturnsAsync(patient);
            _unitOfWorkMock.Setup(u => u.Repository<Visit>().AddAsync(It.IsAny<Visit>())).ReturnsAsync(savedVisit);
            _unitOfWorkMock.Setup(u => u.SaveAsync(It.IsAny<CancellationToken>(), 1, It.IsAny<string>()));

            _mapperMock.Setup(m => m.Map<Visit>(createDto)).Returns(visit);
            _mapperMock.Setup(m => m.Map<CreateVisitDto>(savedVisit)).Returns(createDto);

            var command = new CreateVisitCommand(createDto, claims, logRow);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(createDto, result);
            _unitOfWorkMock.Verify(u => u.Repository<Visit>().AddAsync(visit), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveAsync(It.IsAny<CancellationToken>(), 1, It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task ThrowException_WhenUnexpectedError()
        {
            var patient = new Patient()
            {
                Id = 1
            };
            var dto = new CreateVisitDto
            {
                PatientId = patient.Id,
                DateOfVisit = DateTime.Now
            };
            _patientRepositoryMock.Setup(p => p.GetByIdAsync(patient.Id, false)).ReturnsAsync(patient);
            var command = new CreateVisitCommand(dto, new ClaimsPrincipal(), new LogRow());
            _unitOfWorkMock.Setup(u => u.Repository<Visit>().AddAsync(It.IsAny<Visit>()))
                .ThrowsAsync(new Exception("Unexpected exception"));

            _mapperMock.Setup(x => x.Map<Visit>(dto))
                .Returns(new Visit());
            var exception = await Assert.ThrowsAsync<ClientException>(() => _handler.Handle(command, CancellationToken.None));

            Assert.Equal("Visit creation failed", exception.Message);
        }

        [Fact]
        public async Task ThrowException_WhenPatientNotFound()
        {
            var dto = new CreateVisitDto()
            {
                PatientId = 0,
                DateOfVisit = DateTime.Now,
                VisitType = 0,
            };
            _patientRepositoryMock.Setup(r => r.GetByIdAsync(dto.PatientId, false)).ReturnsAsync((Patient?) null);
            var command = new CreateVisitCommand(dto, new ClaimsPrincipal(), new LogRow());
            var exception = await Assert.ThrowsAsync<NotFoundClientException>(() => _handler.Handle(command, CancellationToken.None));
            Assert.Equal("Patient not found", exception.Message);
        }
    }
}
