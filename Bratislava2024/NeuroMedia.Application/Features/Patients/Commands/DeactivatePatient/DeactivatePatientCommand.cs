using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MediatR;

using Microsoft.Extensions.Logging;

using NeuroMedia.Application.Interfaces.Repositories;
using NeuroMedia.Application.Logging;
using NeuroMedia.Domain.Entities;

namespace NeuroMedia.Application.Features.Patients.Commands.DeactivatePatient
{
    public record DeactivatePatientCommand(int Id, LogRow LogRow) : IRequest<int>;
    internal class DeactivatePatientCommandHandler(IUnitOfWork unitOfWork, ILogger<DeactivatePatientCommandHandler> logger) : IRequestHandler<DeactivatePatientCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ILogger<DeactivatePatientCommandHandler> _logger = logger;
        public async Task<int> Handle(DeactivatePatientCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var patient = await _unitOfWork.Repository<Patient>().GetByIdAsync(command.Id);

                if (patient == null)
                {
                    _logger.LogWarningAndThrow404(command.LogRow, "Patient not found");
                }

                patient!.IsActive = false;

                await _unitOfWork.Repository<Patient>().UpdateAsync(patient);
                await _unitOfWork.SaveAsync(cancellationToken);

                return patient.Id;
            }
            catch (Exception ex)
            {
                _logger.LogErrorAndThrow500(command.LogRow, "Patient deactivation failed", ex);
            }
            return default;
        }
    }
}

