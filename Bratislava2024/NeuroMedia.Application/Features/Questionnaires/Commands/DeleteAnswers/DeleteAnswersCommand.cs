using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using NeuroMedia.Application.Features.Questionnaires.Dtos;
using NeuroMedia.Application.Features.Questionnaires.Helpers;
using NeuroMedia.Application.Interfaces.Blobstorages;
using NeuroMedia.Application.Interfaces.Repositories;
using NeuroMedia.Application.Logging;
using NeuroMedia.Domain.Entities;
using NeuroMedia.Domain.Enums;
using System.Security.Claims;
using System.Text.Json;
using NeuroMedia.Application.Exceptions;
using Microsoft.Graph.Models;
using Azure.Core;
using NeuroMedia.Application.Features.Videos.Dtos;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace NeuroMedia.Application.Features.Questionnaires.Commands.DeleteAnswers
{
    public record DeleteAnswersCommand(int QuestionnaireId, ClaimsPrincipal Claims, LogRow LogRow) : IRequest<QuestionnaireRecordDto>;

    public class DeleteAnswersCommandHandler(IUnitOfWork unitOfWork, IQuestionnaireRepository questionnaireRepository, IBlobStorage blobStorage, IMapper mapper, ILogger<DeleteAnswersCommandHandler> logger) : IRequestHandler<DeleteAnswersCommand, QuestionnaireRecordDto>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IQuestionnaireRepository _questionnaireRepository = questionnaireRepository;
        private readonly IBlobStorage _blobStorage = blobStorage;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<DeleteAnswersCommandHandler> _logger = logger;

        public async Task<QuestionnaireRecordDto> Handle(DeleteAnswersCommand request, CancellationToken cancellationToken)
        {
            if (request.QuestionnaireId <= 0)
            {
                _logger.LogWarningAndThrow400(request.LogRow, "Invalid questionnaire ID");
            }

            try
            {
                var questionnaire = await _unitOfWork.Repository<Questionnaire>().GetByIdAsync(request.QuestionnaireId);
                if (questionnaire == null)
                {
                    _logger.LogWarningAndThrow404(request.LogRow, "Questionnaire not found");
                }

                if (string.IsNullOrEmpty(questionnaire!.BlobPath))
                {
                    _logger.LogWarningAndThrow400(request.LogRow, "No results to delete");
                }

                var isDeleted = await _blobStorage.DeleteAsync(questionnaire.BlobPath!, request.LogRow);
                if (!isDeleted)
                {
                    _logger.LogErrorAndThrow500(request.LogRow, "Failed to delete file from Blob storage.");
                }

                questionnaire.BlobPath = null;
                await _unitOfWork.Repository<Questionnaire>().UpdateAsync(QuestionnaireHelper.AutoUpdateEntries(questionnaire, request.Claims));
                await _unitOfWork.SaveAsync(cancellationToken);

                var updatedResults = await _unitOfWork.Repository<Questionnaire>().GetByIdAsync(request.QuestionnaireId);
                var visit = await _unitOfWork.Repository<Visit>().GetByIdAsync(updatedResults!.VisitId);
                var mappedResults = _mapper.Map<QuestionnaireRecordDto>(updatedResults);
                mappedResults.PatientId = visit!.PatientId;

                //var updatedResults = await _questionnaireRepository.GetByIdIncludeVisitAsync(request.QuestionnaireId);
                //var mappedResults = _mapper.Map<QuestionnaireRecordDto>(updatedResults);

                return mappedResults;
            }
            catch (Exception ex) when (ex is not ClientException)
            {
                await _unitOfWork.RollbackAsync();
                _logger.LogErrorAndThrow500(request.LogRow, $"Questionnaire deletion failed", ex);
            }

            return default!;
        }
    }
}
