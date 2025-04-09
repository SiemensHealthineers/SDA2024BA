using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MediatR;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using NeuroMedia.Application.Exceptions;
using NeuroMedia.Application.Interfaces.Repositories;
using NeuroMedia.Application.Logging;
using NeuroMedia.Domain.Entities;

namespace NeuroMedia.Application.Features.Patients.Commands.ReactivatePatient
{
    public record ReactivatePatientCommand(int Id, LogRow LogRow) : IRequest<int>;
    internal class ReactivatePatientCommandHandler(IUnitOfWork unitOfWork, ILogger<ReactivatePatientCommandHandler> logger) : IRequestHandler<ReactivatePatientCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ILogger<ReactivatePatientCommandHandler> _logger = logger;
        public async Task<int> Handle(ReactivatePatientCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var patient = await _unitOfWork.Repository<Patient>().GetByIdAsync(command.Id, true);

                if (patient == null)
                {
                    _logger.LogWarningAndThrow404(command.LogRow, "Patient not found");
                }

                patient!.IsActive = true;

                await _unitOfWork.Repository<Patient>().UpdateAsync(patient, true);
                await _unitOfWork.SaveAsync(cancellationToken);

                return patient.Id;
            }
            catch (Exception ex) when (ex is not ClientException)
            {
                _logger.LogErrorAndThrow500(command.LogRow, "Patient reactivation failed", ex);
            }
            return default;
        }
    }
}
