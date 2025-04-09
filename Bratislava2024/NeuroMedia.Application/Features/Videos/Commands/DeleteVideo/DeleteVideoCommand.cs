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
using NeuroMedia.Application.Features.Questionnaires.Dtos;
using NeuroMedia.Application.Features.Videos.Dtos;
using NeuroMedia.Application.Features.Videos.Helpers;
using NeuroMedia.Application.Interfaces.Blobstorages;
using NeuroMedia.Application.Interfaces.Repositories;
using NeuroMedia.Application.Logging;

using NeuroMedia.Domain.Entities;

namespace NeuroMedia.Application.Features.Videos.Commands.DeleteVideo
{
    public record DeleteVideoCommand(int VideoId, ClaimsPrincipal Claims, LogRow LogRow) : IRequest<VideoInfoDto>;

    public class DeleteVideoCommandHandler(IUnitOfWork unitOfWork, IVideoRepository videoRepository, IBlobStorage blobStorage, IMapper mapper, ILogger<DeleteVideoCommandHandler> logger) : IRequestHandler<DeleteVideoCommand, VideoInfoDto>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IVideoRepository _videoRepository = videoRepository;
        private readonly IBlobStorage _blobStorage = blobStorage;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<DeleteVideoCommandHandler> _logger = logger;

        public async Task<VideoInfoDto> Handle(DeleteVideoCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var video = await _unitOfWork.Repository<Video>().GetByIdAsync(request.VideoId);
                if (video == null)
                {
                    _logger.LogWarningAndThrow404(request.LogRow, "Video not found");
                }

                if (string.IsNullOrEmpty(video!.BlobPath))
                {
                    _logger.LogWarningAndThrow400(request.LogRow, "No results to delete");
                }

                var isDeleted = await _blobStorage.DeleteAsync(video.BlobPath!, request.LogRow);
                if (!isDeleted)
                {
                    _logger.LogErrorAndThrow500(request.LogRow, "Failed to delete file from Blob storage.");
                }

                video.BlobPath = null;

                await _unitOfWork.Repository<Video>().UpdateAsync(VideoHelper.AutoUpdateEntries(video, request.Claims));
                await _unitOfWork.SaveAsync(cancellationToken);

                var updatedResults = await _unitOfWork.Repository<Video>().GetByIdAsync(request.VideoId);
                var visit = await _unitOfWork.Repository<Visit>().GetByIdAsync(updatedResults!.VisitId);
                var mappedResults = _mapper.Map<VideoInfoDto>(updatedResults);
                mappedResults.PatientId = visit!.PatientId;

                //var updatedResults = await _videoRepository.GetByIdIncludeVisitAsync(request.VideoId);
                //var mappedResults = _mapper.Map<VideoInfoDto>(updatedResults);

                return mappedResults;
            }
            catch (Exception ex) when (ex is not ClientException)
            {
                await _unitOfWork.RollbackAsync();
                _logger.LogErrorAndThrow500(request.LogRow, $"Video deletion failed", ex);
            }

            return default!;
        }
    }
}
