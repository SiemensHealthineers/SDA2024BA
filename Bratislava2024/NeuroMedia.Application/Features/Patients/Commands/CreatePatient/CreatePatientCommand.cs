using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

using AutoMapper;

using MediatR;

using Microsoft.Extensions.Logging;

using NeuroMedia.Application.Common.Helpers;
using NeuroMedia.Application.Interfaces.Repositories;
using NeuroMedia.Application.Logging;
using NeuroMedia.Application.Exceptions;
using NeuroMedia.Domain.Entities;


namespace NeuroMedia.Application.Features.Patients.Commands.CreatePatient
{
    public record CreatePatientCommand(CreatePatientDto Dto, ClaimsPrincipal Claims, LogRow LogRow) : IRequest<CreatePatientDto>;

    public class CreatePatientCommandHandler(IUnitOfWork unitOfWork, IPatientRepository patientRepository, IMapper mapper, ILogger<CreatePatientCommandHandler> logger) : IRequestHandler<CreatePatientCommand, CreatePatientDto>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IPatientRepository _patientRepository = patientRepository;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<CreatePatientCommandHandler> _logger = logger;

        public async Task<CreatePatientDto> Handle(CreatePatientCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var existingPatient = await _patientRepository.GetByEmailAsync(command.Dto.Email);
                if (existingPatient != null)
                {
                    _logger.LogWarningAndThrow400(command.LogRow, "Attempt to create a patient with an existing email.");
                }

                var patient = PatientHelper.AutoUpdateEntries(_mapper.Map<Patient>(command.Dto), command.Claims);
                var result = await _patientRepository.AddAsync(patient);
                await _unitOfWork.SaveAsync(cancellationToken);

                return _mapper.Map<CreatePatientDtoWithId>(result);
            }
            catch (Exception ex) when (ex is not ClientException)
            {
                _logger.LogErrorAndThrow500(command.LogRow, $"Patient creation failed", ex);
            }

            return default!;
        }
    }
}
