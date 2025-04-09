using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

using AutoMapper;
using AutoMapper.QueryableExtensions;

using MediatR;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using NeuroMedia.Application.Exceptions;
using NeuroMedia.Application.Features.Questionnaires.Dtos;
using NeuroMedia.Application.Features.Videos.Dtos;
using NeuroMedia.Application.Interfaces.Repositories;
using NeuroMedia.Application.Logging;

using NeuroMedia.Domain.Entities;

namespace NeuroMedia.Application.Features.Videos.Queries.GetAllVideoInfosByVisitId
{
    public record GetAllVideoInfosByVisitIdQuery(int VisitId, ClaimsPrincipal Claims, LogRow LogRow) : IRequest<List<VideoInfoDto>>;

    public class GetAllVideoInfosByVisitIdHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetAllVideoInfosByVisitIdHandler> logger) : IRequestHandler<GetAllVideoInfosByVisitIdQuery, List<VideoInfoDto>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<GetAllVideoInfosByVisitIdHandler> _logger = logger;

        public async Task<List<VideoInfoDto>> Handle(GetAllVideoInfosByVisitIdQuery request, CancellationToken cancellationToken)
        {

            if (request.VisitId <= 0)
            {
                _logger.LogWarningAndThrow400(request.LogRow, "Invalid request");
            }

            try
            {
                var visit = await _unitOfWork.Repository<Visit>().GetByIdAsync(request.VisitId);

                if (visit == null)
                {
                    _logger.LogWarningAndThrow404(request.LogRow, "Visit not found");
                }

                var results = await _unitOfWork.Repository<Questionnaire>().Entities
                   .Where(q => q.VisitId == request.VisitId)
                   .IgnoreQueryFilters()
                   .ProjectTo<VideoInfoDto>(_mapper.ConfigurationProvider)
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
