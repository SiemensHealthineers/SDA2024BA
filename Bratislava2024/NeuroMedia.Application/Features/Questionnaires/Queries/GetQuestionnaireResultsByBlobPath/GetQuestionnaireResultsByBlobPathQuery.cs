using System.Security.Claims;
using System.Text.Json;

using MediatR;

using Microsoft.Extensions.Logging;
using Microsoft.Graph;

using NeuroMedia.Application.Features.Questionnaires.Dtos;
using NeuroMedia.Application.Features.Questionnaires.Helpers;
using NeuroMedia.Application.Interfaces.Blobstorages;
using NeuroMedia.Application.Logging;

namespace NeuroMedia.Application.Features.Questionnaires.Queries.GetQuestionnaireResultsByBlobPath
{
    public record GetQuestionnaireResultsByBlobPathQuery(string? BlobPath, ClaimsPrincipal Claims, LogRow LogRow) : IRequest<AnswersDto>;

    public class GetQuestionnaireResultsByBlobPathHandler(IBlobStorage blobStorage, ILogger<GetQuestionnaireResultsByBlobPathHandler> logger) : IRequestHandler<GetQuestionnaireResultsByBlobPathQuery, AnswersDto>
    {
        private readonly IBlobStorage _blobStorage = blobStorage;
        private readonly ILogger<GetQuestionnaireResultsByBlobPathHandler> _logger = logger;

        public async Task<AnswersDto> Handle(GetQuestionnaireResultsByBlobPathQuery request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.BlobPath))
            {
                return new AnswersDto();
            }

            try
            {
                QuestionnaireHelper.ResultPathValidation(request.BlobPath);

                var blobStream = await _blobStorage.DownloadAsync(request.BlobPath);

                if (blobStream.Length == 0)
                {
                    return new AnswersDto();
                }

                using var reader = new StreamReader(blobStream);
                var blobOutput = await reader.ReadToEndAsync(cancellationToken);

                return JsonSerializer.Deserialize<AnswersDto>(blobOutput) ?? new AnswersDto();
            }

            catch (JsonException)
            {
                _logger.LogErrorAndThrow500(request.LogRow, "Invalid JSON format on response");
            }

            catch (IOException)
            {
                _logger.LogErrorAndThrow500(request.LogRow, "Error reading stream");
            }

            catch (ArgumentException)
            {
                _logger.LogWarningAndThrow400(request.LogRow, "Invalid blob path");
            }

            catch (Exception ex) when (ex is not ClientException)
            {
                _logger.LogErrorAndThrow500(request.LogRow, "An unexpected error has occurred");
            }

            return default!;
        }
    }
}
