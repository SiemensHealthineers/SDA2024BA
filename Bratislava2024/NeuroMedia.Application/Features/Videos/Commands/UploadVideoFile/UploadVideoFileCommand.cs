using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;

using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Update.Internal;
using Microsoft.Extensions.Logging;

using NeuroMedia.Application.Common.Helpers;
using NeuroMedia.Application.Exceptions;
using NeuroMedia.Application.Features.Questionnaires.Helpers;
using NeuroMedia.Application.Features.Videos.Dtos;
using NeuroMedia.Application.Features.Videos.Helpers;
using NeuroMedia.Application.Interfaces.Blobstorages;
using NeuroMedia.Application.Interfaces.Repositories;
using NeuroMedia.Application.Logging;

using NeuroMedia.Domain.Entities;
using NeuroMedia.Domain.Enums;

using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace NeuroMedia.Application.Features.Videos.Commands.UploadVideoFile
{
    public record UploadVideoFileCommand(string BlobPath, IFormFile FileToUpload, ClaimsPrincipal Claims, LogRow LogRow) : IRequest<VideoInfoDto>;

    public class UploadVideoFileCommandHandler(IUnitOfWork unitOfWork, IVideoRepository videoRepository, IBlobStorage blobStorage, IMapper mapper, ILogger<UploadVideoFileCommandHandler> logger) : IRequestHandler<UploadVideoFileCommand, VideoInfoDto>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IVideoRepository _videoRepository = videoRepository;
        private readonly IBlobStorage _blobStorage = blobStorage;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<UploadVideoFileCommandHandler> _logger = logger;

        public async Task<VideoInfoDto> Handle(UploadVideoFileCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var blobPath = request.BlobPath;

                var videoId = VideoHelper.GetVideoId(blobPath);
                var visitId = VideoHelper.GetVisitId(blobPath);
                var patientId = VideoHelper.GetPatientId(blobPath);
                var videoType = VideoHelper.GetVideoType(blobPath);

                var video = await _videoRepository.GetByIdAsync(videoId);
                if (video == null)
                {
                    _logger.LogWarningAndThrow404(request.LogRow, "Video not found");
                }

                if (video!.VisitId != visitId)
                {
                    _logger.LogWarningAndThrow400(request.LogRow, "Invalid visit ID");
                }

                var visit = await _unitOfWork.Repository<Visit>().GetByIdAsync(visitId);
                if (visit == null)
                {
                    _logger.LogWarningAndThrow404(request.LogRow, "Video not found");
                }

                if (visit!.PatientId != patientId)
                {
                    _logger.LogWarningAndThrow404(request.LogRow, "Invalid patient ID");
                }

                if (videoType == null)
                {
                    _logger.LogWarningAndThrow404(request.LogRow, "Invalid video type");
                }

                if (video.VideoType != videoType)
                {
                    _logger.LogWarningAndThrow400(request.LogRow, "Wrong video type");
                }

                if (!string.IsNullOrEmpty(video!.BlobPath))
                {
                    _logger.LogWarningAndThrow400(request.LogRow, "Video already exists");
                }

                var uploadBlobPath = await UploadSingleFile(patientId, visitId, videoId, (VideoType) videoType!, request.FileToUpload, request.LogRow);

                video.BlobPath = uploadBlobPath;
                video = VideoHelper.AutoUpdateEntries(video, request.Claims);

                await _videoRepository.UpdateAsync(video);
                await _unitOfWork.SaveAsync(cancellationToken);

                return new VideoInfoDto
                {
                    BlobPath = uploadBlobPath,
                    VideoType = videoType,
                    Id = videoId,
                    PatientId = patientId,
                    VisitId = visitId
                };
            }
            catch (Exception ex) when (ex is not ClientException)
            {
                await _unitOfWork.RollbackAsync();
                _logger.LogErrorAndThrow500(request.LogRow, $"Video upload failed", ex);
            }

            return default!;
        }

        private async Task<string> UploadSingleFile(int patientId, int visitId, int videoId, VideoType videoType, IFormFile fileToUpload, LogRow logRow)
        {
            var fileName = $"{DateTime.Now.ToFileTimeUtc()}{Path.GetExtension(fileToUpload.FileName)}";
            var blobPath = VideoHelper.GetVideoFilePath(patientId, visitId, videoId, videoType, fileName);

            using var stream = fileToUpload.OpenReadStream();
            await _blobStorage.UploadAsync(blobPath, stream, logRow: logRow);

            return blobPath;
        }
    }

}
