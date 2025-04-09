using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

using AutoMapper;

using MediatR;

using Microsoft.Extensions.Logging;

using NeuroMedia.Application.Common.Helpers;
using NeuroMedia.Application.Exceptions;
using NeuroMedia.Application.Features.Questionnaires.Helpers;
using NeuroMedia.Application.Features.Videos.Helpers;
using NeuroMedia.Application.Interfaces.Repositories;
using NeuroMedia.Application.Logging;
using NeuroMedia.Domain.Entities;
using NeuroMedia.Domain.Enums;

namespace NeuroMedia.Application.Features.Visits.Commands.CreateVisit
{
    public record CreateVisitCommand(CreateVisitDto Dto, ClaimsPrincipal Claims, LogRow LogRow) : IRequest<CreateVisitDto>;
    public class CreateVisitCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IPatientRepository patientRepository, ILogger<CreateVisitCommandHandler> logger) : IRequestHandler<CreateVisitCommand, CreateVisitDto>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        private readonly IPatientRepository _patientRepository = patientRepository;
        private readonly ILogger<CreateVisitCommandHandler> _logger = logger;


        public async Task<CreateVisitDto> Handle(CreateVisitCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var patient = await _patientRepository.GetByIdAsync(command.Dto.PatientId);
                if (patient == null)
                {
                    _logger.LogWarningAndThrow404(command.LogRow, "Patient not found");
                }
                var visit = VisitHelper.AutoUpdateEntries(_mapper.Map<Visit>(command.Dto), command.Claims);

                CreateRequiredVideoEntites(command, visit);

                var result = await _unitOfWork.Repository<Visit>().AddAsync(visit);
                await _unitOfWork.SaveAsync(cancellationToken);
                var createdVisitDto = _mapper.Map<CreateVisitDto>(result);
                createdVisitDto.VisitId = result.Id;
                return createdVisitDto;
            }
            catch (Exception ex) when (ex is not ClientException)
            {
                _logger.LogErrorAndThrow500(command.LogRow, $"Visit creation failed", ex);
            }
            return default!;
        }

        private static void CreateRequiredVideoEntites(CreateVisitCommand command, Visit visit)
        {
            foreach (VideoType videoType in Enum.GetValues(typeof(VideoType)))
            {
                var video = VideoHelper.AutoUpdateEntries(new Video
                {
                    BlobPath = string.Empty,
                    VideoType = videoType
                }, command.Claims);

                visit.Videos.Add(video);
            }
        }
    }
}
