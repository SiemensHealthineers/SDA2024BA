using System.Security.Claims;

using AutoMapper;

using MediatR;

using Microsoft.Extensions.Logging;

using NeuroMedia.Application.Interfaces.Repositories;
using NeuroMedia.Application.Logging;
using NeuroMedia.Domain.Entities;
using NeuroMedia.Application.Exceptions;
using NeuroMedia.Application.Features.Questionnaires.Helpers;
using NeuroMedia.Domain.Enums;

namespace NeuroMedia.Application.Features.Questionnaires.Commands.CreateQuestionnaireRecordsCommand
{
    public record CreateQuestionnaireRecordsCommand(int VisitId, QuestionnaireType Type, ClaimsPrincipal Claims, LogRow LogRow) : IRequest<CreateQuestionnaireRecordsDto>;

    public class CreateResultsForVisitCommandHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IQuestionnaireRepository questionnaireRepository,
        ILogger<CreateResultsForVisitCommandHandler> logger) : IRequestHandler<CreateQuestionnaireRecordsCommand, CreateQuestionnaireRecordsDto>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        private readonly IQuestionnaireRepository _questionnaireRepository = questionnaireRepository;
        private readonly ILogger<CreateResultsForVisitCommandHandler> _logger = logger;


        public async Task<CreateQuestionnaireRecordsDto> Handle(CreateQuestionnaireRecordsCommand request, CancellationToken cancellationToken)
        {
            if (request.VisitId <= 0)
            {
                _logger.LogWarningAndThrow400(request.LogRow, "Invalid visit ID");
            }

            try
            {
                var visit = await _unitOfWork.Repository<Visit>().GetByIdAsync(request.VisitId);
                if (visit == null)
                {
                    _logger.LogWarningAndThrow404(request.LogRow, "Visit not found");
                }

                var resultsFromDb = await _questionnaireRepository.GetByVisitIdAndTypeAsync(request.VisitId, request.Type);
                if (resultsFromDb != null)
                {
                    _logger.LogWarningAndThrow400(request.LogRow, "Questionnaire records already exist");
                }

                var dto = new CreateQuestionnaireRecordsDto()
                {
                    VisitId = request.VisitId,
                    QuestionnaireType = request.Type
                };

                var questionnaireResults = QuestionnaireHelper.AutoUpdateEntries(_mapper.Map<Questionnaire>(dto), request.Claims);
                var result = await _questionnaireRepository.AddAsync(questionnaireResults);
                await _unitOfWork.SaveAsync(cancellationToken);
                return _mapper.Map<CreateQuestionnaireRecordsDto>(result);
            }
            catch (Exception ex) when (ex is not ClientException)
            {
                _logger.LogErrorAndThrow500(request.LogRow, $"Results creation failed", ex);
            }
            return default!;
        }
    }

}
