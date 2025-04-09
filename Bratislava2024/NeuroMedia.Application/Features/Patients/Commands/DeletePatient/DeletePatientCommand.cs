using MediatR;

using Microsoft.Extensions.Logging;

using NeuroMedia.Application.Exceptions;
using NeuroMedia.Application.Interfaces.Repositories;
using NeuroMedia.Application.Logging;

using NeuroMedia.Domain.Entities;

namespace NeuroMedia.Application.Features.Patients.Commands.DeletePatient
{
    public record DeletePatientCommand(int Id, LogRow LogRow) : IRequest<int>;

    internal class DeletePatientCommandHandler(IUnitOfWork unitOfWork, ILogger<DeletePatientCommandHandler> logger) : IRequestHandler<DeletePatientCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ILogger<DeletePatientCommandHandler> _logger = logger;

        public async Task<int> Handle(DeletePatientCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var patient = await _unitOfWork.Repository<Patient>().GetByIdAsync(command.Id, true);

                if (patient == null)
                {
                    _logger.LogWarningAndThrow404(command.LogRow, "Patient not found");
                }
                else if (patient.IsActive)
                {
                    _logger.LogWarningAndThrow400(command.LogRow, "Cannot delete active patient");
                }

                await _unitOfWork.Repository<Patient>().DeleteAsync(patient!, true);
                await _unitOfWork.SaveAsync(cancellationToken);

                return patient!.Id;
            }
            catch (Exception ex) when (ex is not ClientException)
            {
                _logger.LogErrorAndThrow500(command.LogRow, "Patient deletion failed", ex);
            }

            return default;
        }
    }
}
