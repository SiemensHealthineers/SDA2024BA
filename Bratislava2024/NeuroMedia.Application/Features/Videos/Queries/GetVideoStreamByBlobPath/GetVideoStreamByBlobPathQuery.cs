using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

using AutoMapper;
using MediatR;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
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

using Newtonsoft.Json.Linq;

namespace NeuroMedia.Application.Features.Videos.Queries.GetVideoStreamByBlobPath
{
    public record GetVideoStreamByBlobPathQuery(string BlobPath, Roles AllowedRoles, string AccessToken, string IdToken, LogRow LogRow) : IRequest<FileContentResult>;

    public class GetVideoStreamByBlobPathHandler(IUnitOfWork unitOfWork, IBlobStorage blobStorage, IConfiguration configuration, ILogger<GetVideoStreamByBlobPathHandler> logger) : IRequestHandler<GetVideoStreamByBlobPathQuery, FileContentResult>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IBlobStorage _blobStorage = blobStorage;
        private readonly IConfiguration _configuration = configuration;
        private readonly ILogger<GetVideoStreamByBlobPathHandler> _logger = logger;

        public async Task<FileContentResult> Handle(GetVideoStreamByBlobPathQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var useIdToken = bool.TrueString == _configuration["Token:UseIdToken"];

                var token = useIdToken ? request.IdToken : request.AccessToken;

                var jwtSecurityToken = await JwtValidationHelper.ValidateToken(token, _configuration, _logger, true, request.LogRow);
                if (jwtSecurityToken == null)
                {
                    _logger.LogWarningAndThrow403(null, request.LogRow, $"Forbidden access");
                }

                var blobPath = request.BlobPath;
                var video = await _unitOfWork.Repository<Video>()
                    .GetByIdAsync(VideoHelper.GetVideoId(blobPath));

                if (video == null)
                {
                    _logger.LogWarningAndThrow400(request.LogRow, "Invalid request");
                }

                if (video!.VisitId != VideoHelper.GetVisitId(blobPath))
                {
                    _logger.LogWarningAndThrow400(request.LogRow, "Invalid request");
                }

                if (video.BlobPath != blobPath)
                {
                    _logger.LogWarningAndThrow400(request.LogRow, "Invalid request");
                }

                var identity = new ClaimsPrincipal(new ClaimsIdentity(jwtSecurityToken!.Claims));
                var jwtRoles = RolesHelper.GetEnumRoles(identity!.GetRoles());

                if (!((request.AllowedRoles & jwtRoles) > Roles.None))
                {
                    _logger.LogWarningAndThrow403(null, request.LogRow, $"Forbidden access for this role");
                }

                var fileResponse = await GetFile(blobPath, request.LogRow);

                return fileResponse;
            }
            catch (Exception ex) when (ex is not ClientException)
            {
                _logger.LogErrorAndThrow500(request.LogRow, "Error processing the query", ex);
            }

            return default!;
        }

        private async Task<FileContentResult> GetFile(string blobPath, LogRow logRow)
        {
            var stream = await _blobStorage.DownloadAsync(blobPath, logRow);

            using var memoryStream = new MemoryStream();
            await stream.CopyToAsync(memoryStream);

            var fileBytes = memoryStream.ToArray();

            var file = new FileContentResult(fileBytes, "application/octet-stream")
            {
                FileDownloadName = VideoHelper.GetFileName(blobPath)
            };

            return file;
        }
    }
}
