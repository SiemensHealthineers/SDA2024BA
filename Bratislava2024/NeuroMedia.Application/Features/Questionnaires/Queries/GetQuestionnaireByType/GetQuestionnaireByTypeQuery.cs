using System.Security.Claims;
using System.Text.Json;

using MediatR;

using Microsoft.Extensions.Logging;

using NeuroMedia.Application.Features.Questionnaires.Dtos;
using NeuroMedia.Application.Features.Questionnaires.Helpers;
using NeuroMedia.Application.Interfaces.Blobstorages;
using NeuroMedia.Application.Logging;
using NeuroMedia.Domain.Enums;

namespace NeuroMedia.Application.Features.Questionnaires.Queries.GetQuestionnaireByType
{
    public record GetQuestionnaireByTypeQuery(QuestionnaireType Type, ClaimsPrincipal Claims, LogRow LogRow) : IRequest<QuestionnaireDto>;

    public class GetQuestionnaireByTypeQueryHandler(IBlobStorage blobStorage, ILogger<GetQuestionnaireByTypeQueryHandler> logger) : IRequestHandler<GetQuestionnaireByTypeQuery, QuestionnaireDto>
    {
        private readonly IBlobStorage _blobStorage = blobStorage;
        private readonly ILogger<GetQuestionnaireByTypeQueryHandler> _logger = logger;

        public async Task<QuestionnaireDto> Handle(GetQuestionnaireByTypeQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var blobPath = QuestionnaireHelper.GetQuestionnairePath(request.Type);

                var blobStream = await _blobStorage.DownloadAsync(blobPath);
                if (blobStream == null)
                {
                    _logger.LogWarningAndThrow400(request.LogRow, $"Blob stream is null for path: {blobPath}");
                }

                using var reader = new StreamReader(blobStream);
                var blobOutput = await reader.ReadToEndAsync(cancellationToken);

                if (blobOutput is "null" or "Null" or "NULL")
                {
                    _logger.LogWarningAndThrow400(request.LogRow, $"Content is invalid for path: {blobPath}");
                }

                var quesionnaire = JsonSerializer.Deserialize<QuestionnaireDto>(blobOutput) ?? new QuestionnaireDto();

                if (quesionnaire == null)
                {
                    _logger.LogWarningAndThrow400(request.LogRow, $"Questionnaire of type {request.Type} could not be found or deserialized.");
                }
                return quesionnaire ?? new QuestionnaireDto();
            }
            catch (JsonException ex)
            {
                _logger.LogErrorAndThrow500(request.LogRow, "Invalid JSON format on response", ex);
            }
            catch (IOException ex)
            {
                _logger.LogErrorAndThrow500(request.LogRow, $"Error reading stream for blob path", ex);
            }

            return new QuestionnaireDto();
        }
    }
}
