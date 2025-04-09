using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

using NeuroMedia.Application.Exceptions;
using NeuroMedia.Application.Features.Videos.Helpers;
using NeuroMedia.Application.Interfaces.Repositories;
using NeuroMedia.Application.Logging;
using NeuroMedia.Domain.Entities;

using NeuroMedia.Domain.Enums;

namespace NeuroMedia.Application.Features.Videos.Commands.CreateVideoInfoForVisit
{
    public record CreateVideoInfoForVisitCommand(int VisitId, int Type, ClaimsPrincipal Claims, LogRow LogRow) : IRequest<CreateVideoInfoForVisitDto>;

    public class CreateVideoInfoForVisitCommandHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IVideoRepository videoRepository,
        ILogger<CreateVideoInfoForVisitCommandHandler> logger) : IRequestHandler<CreateVideoInfoForVisitCommand, CreateVideoInfoForVisitDto>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        private readonly IVideoRepository _videoRepository = videoRepository;
        private readonly ILogger<CreateVideoInfoForVisitCommandHandler> _logger = logger;


        public async Task<CreateVideoInfoForVisitDto> Handle(CreateVideoInfoForVisitCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var visit = await _unitOfWork.Repository<Visit>().GetByIdAsync(request.VisitId);
                if (visit == null)
                {
                    _logger.LogWarningAndThrow404(request.LogRow, "Visit not found");
                }

                var resultsFromDb = await _videoRepository.GetByVisitIdAndTypeAsync(request.VisitId, (VideoType) request.Type);
                if (resultsFromDb != null)
                {
                    _logger.LogWarningAndThrow400(request.LogRow, "Results already exist");
                }

                var video = new Video
                {
                    VisitId = request.VisitId,
                    VideoType = (VideoType) request.Type
                };

                var questionnaireResults = VideoHelper.AutoUpdateEntries(video, request.Claims);
                var result = await _videoRepository.AddAsync(questionnaireResults);
                await _unitOfWork.SaveAsync(cancellationToken, userId: string.Empty);

                return _mapper.Map<CreateVideoInfoForVisitDto>(result);
            }
            catch (Exception ex) when (ex is not ClientException)
            {
                _logger.LogErrorAndThrow500(request.LogRow, $"Results creation failed", ex);
            }
            return default!;
        }
    }
}
