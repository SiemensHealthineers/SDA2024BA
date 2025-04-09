using System.Buffers;
using System.Collections;
using System.Text;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using NeuroMedia.API.Policies;
using NeuroMedia.Application.Common.Helpers;
using NeuroMedia.Application.Features.Videos.Commands.CreateVideoInfoForVisit;
using NeuroMedia.Application.Features.Videos.Commands.DeleteVideo;
using NeuroMedia.Application.Features.Videos.Commands.UploadVideoFile;
using NeuroMedia.Application.Features.Videos.Helpers;
using NeuroMedia.Application.Features.Videos.Queries.GetAllVideoInfosByVisitId;
using NeuroMedia.Application.Features.Videos.Queries.GetVideoStreamByBlobPath;
using NeuroMedia.Application.Logging;
using NeuroMedia.Domain.Enums;

using Swashbuckle.AspNetCore.Annotations;

namespace NeuroMedia.API.Controllers
{
    [Route("api/[controller]")]
    public class VideosController(IMediator mediator) : ApiController
    {
        [HttpGet("{visitId:int}")]
        [CustomRoleAuthorize(Roles.MedicalGroup)]
        public async Task<IActionResult> GetAllVideoInfosByVisitId(int visitId)
        {
            return await MediatorTrySend(mediator, new GetAllVideoInfosByVisitIdQuery(visitId, User, new LogRow().UpdateCallerProperties()));
        }


        [HttpPost("{visitId:int}/{type:int}")]
        [CustomRoleAuthorize(Roles.MedicalGroup)]
        public async Task<IActionResult> CreateVideoInfoForVisit(int visitId, int type)
        {
            return await MediatorTrySend(mediator, new CreateVideoInfoForVisitCommand(visitId, type, User, new LogRow().UpdateCallerProperties()));
        }

        [HttpDelete("{videoId:int}")]
        [CustomRoleAuthorize(Roles.MedicalGroup)]
        public async Task<IActionResult> Delete(int videoId)
        {
            return await MediatorTrySend(mediator, new DeleteVideoCommand(videoId, User, new LogRow().UpdateCallerProperties()));
        }

        [AllowAnonymous]
        [HttpGet("stream")]
        [CustomRoleAuthorize(Roles.MedicalGroup)]
        public async Task<IActionResult/*System.Web.Mvc.FileContentResult*/> GetVideoStreamByBlobPath([FromQuery] string blobPath, [FromQuery] string accessToken = "", [FromQuery] string idToken = "")
        {
            var result = await mediator.Send(new GetVideoStreamByBlobPathQuery(blobPath, Roles.MedicalGroup, accessToken, idToken, new LogRow().UpdateCallerProperties()));

            return new FileContentResult(result.FileContents, result.ContentType);
        }

        [HttpPost("upload")]
        [SwaggerIgnore]
        [RequestSizeLimit(104857600)]
        [CustomRoleAuthorize(Roles.MedicalGroup)]
        public async Task<IActionResult> UploadVideoFile([FromBody] VideoDto videoDto)
        {
            var fileName = VideoHelper.GetFileName(videoDto.BlobPath);
            var bytes = Convert.FromBase64String(videoDto.ContentBase64);
            var stream = new MemoryStream(bytes);
            var fileToUpload = new FormFile(stream, 0, stream.Length, fileName, fileName);

            return await MediatorTrySend(mediator, new UploadVideoFileCommand(videoDto.BlobPath, fileToUpload, User, new LogRow().UpdateCallerProperties()));
        }

        public class VideoDto
        {
            public string BlobPath { get; set; } = default!;
            public string ContentBase64 { get; set; } = default!;
        }
    }
}
