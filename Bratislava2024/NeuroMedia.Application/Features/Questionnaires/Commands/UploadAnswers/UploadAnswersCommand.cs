using System.Security.Claims;

using AutoMapper;

using MediatR;

using Microsoft.Extensions.Logging;

using NeuroMedia.Application.Interfaces.Repositories;
using NeuroMedia.Application.Logging;
using NeuroMedia.Application.Exceptions;
using NeuroMedia.Domain.Entities;
using NeuroMedia.Application.Features.Questionnaires.Dtos;
using NeuroMedia.Application.Features.Questionnaires.Helpers;
using NeuroMedia.Domain.Enums;
using NeuroMedia.Application.Interfaces.Blobstorages;
using System.Text.Json;
using NeuroMedia.Application.Features.Questionnaires.Queries.GetQuestionnaireByType;
using Microsoft.Graph.Education.Classes.Item.Assignments.Item.Submissions.Item.Return;

namespace NeuroMedia.Application.Features.Questionnaires.Commands.UploadAnswers
{
    public record UploadAnswersCommand(string BlobPath, AnswersDto Dto, ClaimsPrincipal Claims, LogRow LogRow) : IRequest<QuestionnaireRecordDto>;

    public class UploadAnswersCommandHandler(IUnitOfWork unitOfWork, IBlobStorage blobStorage, IMapper mapper, ILogger<UploadAnswersCommandHandler> logger) : IRequestHandler<UploadAnswersCommand, QuestionnaireRecordDto>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IBlobStorage _blobStorage = blobStorage;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<UploadAnswersCommandHandler> _logger = logger;

        public async Task<QuestionnaireRecordDto> Handle(UploadAnswersCommand request, CancellationToken cancellationToken)
        {
            try
            {
                QuestionnaireHelper.ResultPathValidation(request.BlobPath);
                var questionnaireId = QuestionnaireHelper.GetQuestionnaireId(request.BlobPath);
                var questionnaireType = QuestionnaireHelper.GetResultQuestionnaireType(request.BlobPath);
                var questionnaire = await _unitOfWork.Repository<Questionnaire>().GetByIdAsync(questionnaireId);

                if (questionnaire == null)
                {
                    _logger.LogWarningAndThrow404(request.LogRow, "Questionnaire not found");
                }

                if (!string.IsNullOrEmpty(questionnaire!.BlobPath))
                {
                    _logger.LogWarningAndThrow400(request.LogRow, "Results already exist");
                }

                if (questionnaire.QuestionnaireType != questionnaireType)
                {
                    _logger.LogWarningAndThrow400(request.LogRow, "Wrong questionnaire type");
                }

                var jsonContent = JsonSerializer.Serialize(new {Answers = request.Dto.Answers });
                var byteArray = System.Text.Encoding.UTF8.GetBytes(jsonContent);

                using (var memoryStream = new MemoryStream(byteArray))
                {
                    var isUploaded = await _blobStorage.UploadAsync(request.BlobPath, memoryStream);
                    if (!isUploaded)
                    {
                        _logger.LogErrorAndThrow500(request.LogRow, "Failed to upload file to Blob storage.");
                    }
                }

                questionnaire.BlobPath = request.BlobPath;
                await _unitOfWork.Repository<Questionnaire>().UpdateAsync(QuestionnaireHelper.AutoUpdateEntries(questionnaire, request.Claims));

                await _unitOfWork.SaveAsync(cancellationToken, userId: string.Empty);
                var updatedResults = await _unitOfWork.Repository<Questionnaire>().GetByIdAsync(questionnaireId);
                var visit = await _unitOfWork.Repository<Visit>().GetByIdAsync(updatedResults!.VisitId);
                var mappedResults = _mapper.Map<QuestionnaireRecordDto>(updatedResults);
                mappedResults.PatientId = visit!.PatientId;

                return mappedResults;
            }

            catch (ArgumentException)
            {
                _logger.LogWarningAndThrow400(request.LogRow, "Invalid blob path");
            }

            catch (Exception ex) when (ex is not ClientException)
            {
                await _unitOfWork.RollbackAsync();
                _logger.LogErrorAndThrow500(request.LogRow, "Questionnaire creation failed");
            }

            return default!;
        }
    }
}
