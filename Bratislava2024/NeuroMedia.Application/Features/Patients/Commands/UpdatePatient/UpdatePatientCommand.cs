using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Design;
using System.Security.Claims;

using AutoMapper;

using MediatR;

using Microsoft.Extensions.Logging;

using NeuroMedia.Application.Common.Helpers;
using NeuroMedia.Application.Interfaces.Repositories;
using NeuroMedia.Application.Logging;
using NeuroMedia.Application.Exceptions;

using NeuroMedia.Domain.Entities;

namespace NeuroMedia.Application.Features.Patients.Commands.UpdatePatient
{
    public record UpdatePatientCommand(int Id, UpdatePatientDto Dto, ClaimsPrincipal Claims, LogRow LogRow) : IRequest<UpdatePatientDto>;

     public class UpdatePatientCommandHandler(IUnitOfWork unitOfWork, IPatientRepository patientRepository, IMapper mapper, ILogger<UpdatePatientCommandHandler> logger) : IRequestHandler<UpdatePatientCommand, UpdatePatientDto>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IPatientRepository _patientRepository = patientRepository;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<UpdatePatientCommandHandler> _logger = logger;

        public async Task<UpdatePatientDto> Handle(UpdatePatientCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var patient = await _patientRepository.GetByIdAsync(command.Id);

                if (patient == null)
                {
                    _logger.LogWarningAndThrow404(command.LogRow, "Patient not found");
                }

                var existingPatient = await _patientRepository.GetByEmailAsync(command.Dto.Email, command.Id);
                if (existingPatient != null)
                {
                    _logger.LogWarningAndThrow400(command.LogRow, "Attempt to update patient with an existing email.");
                }

                patient = _mapper.Map(command.Dto, patient);

                await _patientRepository.UpdateAsync(PatientHelper.AutoUpdateEntries(patient, command.Claims));
                await _unitOfWork.SaveAsync(cancellationToken);

                return _mapper.Map<UpdatePatientDto>(patient);
            }
            catch (Exception ex) when (ex is not ClientException)
            {
                _logger.LogErrorAndThrow500(command.LogRow, "Patient update failed", ex);
            }

            return default!;
        }
    }
}
