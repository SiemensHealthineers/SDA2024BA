using AutoMapper;
using AutoMapper.QueryableExtensions;

using Microsoft.EntityFrameworkCore;
using MediatR;
using NeuroMedia.Application.Interfaces.Repositories;
using NeuroMedia.Domain.Entities;
using NeuroMedia.Application.Features.Questionnaires.Dtos;
using System.Security.Claims;
using NeuroMedia.Application.Logging;
using Microsoft.Extensions.Logging;
using NeuroMedia.Application.Exceptions;

namespace NeuroMedia.Application.Features.Questionnaires.Queries.GetQuestionnairesByVisitId
{
    public record GetAllQuestionnairesResultsByVisitIdQuery(int VisitId, ClaimsPrincipal Claims, LogRow LogRow) : IRequest<List<QuestionnaireRecordDto>>;

    public class GetAllQuestionnairesResultsByVisitIdHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetAllQuestionnairesResultsByVisitIdHandler> logger) : IRequestHandler<GetAllQuestionnairesResultsByVisitIdQuery, List<QuestionnaireRecordDto>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<GetAllQuestionnairesResultsByVisitIdHandler> _logger = logger;

        public async Task<List<QuestionnaireRecordDto>> Handle(GetAllQuestionnairesResultsByVisitIdQuery request, CancellationToken cancellationToken)
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

                var visitId = request.VisitId;

                var results = await _unitOfWork.Repository<Questionnaire>().Entities
                   .Where(q => q.VisitId == visitId)
                   .IgnoreQueryFilters()
                   .ProjectTo<QuestionnaireRecordDto>(_mapper.ConfigurationProvider)
                   .ToListAsync(cancellationToken);

                if (results == null)
                {
                    return [];
                }

                return results;
            }
            catch (Exception ex) when (ex is not ClientException)
            {
                _logger.LogErrorAndThrow500(request.LogRow, "Error processing the query", ex);
            }

            return [];
        }
    }
}
