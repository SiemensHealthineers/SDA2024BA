using System.Security.Claims;
using System.Text.Json;
using AutoMapper;

using MediatR;

using Microsoft.Extensions.Logging;
using Microsoft.Graph;

using NeuroMedia.Application.Features.Questionnaires.Dtos;
using NeuroMedia.Application.Features.Questionnaires.Helpers;
using NeuroMedia.Application.Interfaces.Blobstorages;
using NeuroMedia.Application.Interfaces.Repositories;
using NeuroMedia.Application.Logging;
using NeuroMedia.Domain.Entities;

using Newtonsoft.Json;

namespace NeuroMedia.Application.Features.Questionnaires.Commands.EditQuestionnaireAnswers
{
    public record EditAnswersCommand(int QuestionnaireId, AnswersDto Dto, ClaimsPrincipal Claims, LogRow LogRow) : IRequest<QuestionnaireRecordDto>;

    public class EditAnswersCommandHandler(IUnitOfWork unitOfWork, IBlobStorage blobStorage, IMapper mapper, ILogger<EditAnswersCommandHandler> logger) : IRequestHandler<EditAnswersCommand, QuestionnaireRecordDto>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IBlobStorage _blobStorage = blobStorage;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<EditAnswersCommandHandler> _logger = logger;

        public async Task<QuestionnaireRecordDto> Handle(EditAnswersCommand command, CancellationToken cancellationToken)
        {
            if (command.QuestionnaireId <= 0)
            {
                _logger.LogWarningAndThrow400(command.LogRow, "Invalid questionnaire ID");
            }

            try
            {
                var questionnaire = await _unitOfWork.Repository<Questionnaire>().GetByIdAsync(command.QuestionnaireId);
                if (questionnaire == null)
                {
                    _logger.LogWarningAndThrow404(command.LogRow, "Questionnaire not found");
                }
                var blobPath = questionnaire.BlobPath;
                if (blobPath == null)
                {
                    _logger.LogWarningAndThrow400(command.LogRow, "No results to update");
                }

                var isDeleted = await _blobStorage.DeleteAsync(questionnaire.BlobPath);
                if (!isDeleted)
                {
                    _logger.LogErrorAndThrow500(command.LogRow, "Failed to delete existing blob in Blob storage");
                }

                var jsonContent = System.Text.Json.JsonSerializer.Serialize(new { Answers = command.Dto.Answers});
                var byteArray = System.Text.Encoding.UTF8.GetBytes(jsonContent);

                using (var memoryStream = new MemoryStream(byteArray))
                {
                    var isUploaded = await _blobStorage.UploadAsync(blobPath, memoryStream);
                    if (!isUploaded)
                    {
                        _logger.LogErrorAndThrow500(command.LogRow, "Failed to edit file in Blob storage");
                    }
                }

                await _unitOfWork.Repository<Questionnaire>()
                    .UpdateAsync(QuestionnaireHelper.AutoUpdateEntries(questionnaire, command.Claims));
                await _unitOfWork.SaveAsync(cancellationToken);
                var updatedResults = await _unitOfWork.Repository<Questionnaire>().GetByIdAsync(command.QuestionnaireId);
                var visit = await _unitOfWork.Repository<Visit>().GetByIdAsync(updatedResults.VisitId);
                var mappedResults = _mapper.Map<QuestionnaireRecordDto>(updatedResults);
                mappedResults.PatientId = visit.PatientId;

                return mappedResults;
            }

            catch (Exception ex) when (ex is not ClientException)
            {
                await _unitOfWork.RollbackAsync();
                _logger.LogErrorAndThrow500(command.LogRow, $"Questionnaire edit failed");
            }

            return default!;

        }
    }
}
