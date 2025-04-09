using System.Security.Claims;

using AutoMapper;

using MediatR;

using Microsoft.Extensions.Logging;

using NeuroMedia.Application.Common.Helpers;
using NeuroMedia.Application.Features.Patients.Commands.UpdatePatient;
using NeuroMedia.Application.Interfaces.Repositories;
using NeuroMedia.Application.Logging;

using NeuroMedia.Domain.Entities;

namespace NeuroMedia.Application.Features.UserInfos.Commands.CreateOrUpdateUserInfo
{
    internal class CreateOrUpdateUserInfoCommand
    {
    }

    public record UpdatePatientCommand(int Id, UpdatePatientDto Dto, ClaimsPrincipal Claims, LogRow LogRow) : IRequest<UpdatePatientDto>;

    internal class UpdatePatientCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<UpdatePatientCommandHandler> logger) : IRequestHandler<UpdatePatientCommand, UpdatePatientDto>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<UpdatePatientCommandHandler> _logger = logger;

        public async Task<UpdatePatientDto> Handle(UpdatePatientCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var patient = await _unitOfWork.Repository<Patient>().GetByIdAsync(command.Id);
                if (patient == null)
                {
                    _logger.LogWarningAndThrow404(command.LogRow, "Patient not found");
                }

                patient = _mapper.Map<Patient>(command.Dto);
                await _unitOfWork.Repository<Patient>().UpdateAsync(PatientHelper.AutoUpdateEntries(patient, command.Claims));
                await _unitOfWork.SaveAsync(cancellationToken);

                return _mapper.Map<UpdatePatientDto>(patient);
            }
            catch (Exception ex)
            {
                _logger.LogErrorAndThrow500(command.LogRow, "Patient update failed", ex);
            }

            return default;
        }
    }
}
