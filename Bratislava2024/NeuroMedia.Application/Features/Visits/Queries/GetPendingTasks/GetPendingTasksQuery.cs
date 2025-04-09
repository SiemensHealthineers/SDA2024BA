using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AutoMapper;
using AutoMapper.QueryableExtensions;

using MediatR;

using Microsoft.EntityFrameworkCore;

using NeuroMedia.Application.Features.Visits.Queries.GetActualVisit;
using NeuroMedia.Application.Interfaces.Repositories;
using NeuroMedia.Domain.Entities;
using NeuroMedia.Domain.Enums;

namespace NeuroMedia.Application.Features.Visits.Queries.GetPendingTasks
{
    public record GetPendingTasksQuery(int PatientId) : IRequest<List<GetPendingTaskDto>>;

    public class GetPendingTasksQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetPendingTasksQuery, List<GetPendingTaskDto>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<List<GetPendingTaskDto>> Handle(GetPendingTasksQuery request, CancellationToken cancellationToken)
        {
            var today = DateTime.UtcNow.Date;

            var visit = await _unitOfWork.Repository<Visit>().Entities
                .Include(x => x.Questionnaires)
                .Include(x => x.Videos)
                .IgnoreQueryFilters()
                .Where(x => x.PatientId == request.PatientId && x.DateOfVisit <= today)
                .OrderByDescending(x => x.DateOfVisit == today)
                .ThenByDescending(x => x.DateOfVisit <= today)
                .ThenByDescending(x => x.DateOfVisit)
                .FirstOrDefaultAsync(cancellationToken);

            var list = new List<GetPendingTaskDto>();

            if (visit != null)
            {
                AddQuestionnaires(request, visit, list);
                AddVideos(request, visit, list);
            }

            return list;
        }

        private static void AddQuestionnaires(GetPendingTasksQuery request, Visit visit, List<GetPendingTaskDto> list)
        {
            list.AddRange(visit.Questionnaires.Where(questionnaire => string.IsNullOrEmpty(questionnaire.BlobPath))
                .Select(questionnaire => new GetPendingTaskDto
                {
                    Name = $"{PendingTaskType.Questionnaire} ({questionnaire.QuestionnaireType}) - {visit.DateOfVisit.ToShortDateString()}",
                    PatientId = request.PatientId,
                    VisitId = visit.Id,
                    Id = questionnaire.Id,
                    QuestionnaireType = questionnaire.QuestionnaireType,
                    VideoType = null
                }));
        }

        private static void AddVideos(GetPendingTasksQuery request, Visit visit, List<GetPendingTaskDto> list)
        {
            list.AddRange(visit.Videos.Where(video => string.IsNullOrEmpty(video.BlobPath))
                .Select(video => new GetPendingTaskDto
                {
                    Name = $"{PendingTaskType.Video} ({video.VideoType}) - {visit.DateOfVisit.ToShortDateString()}",
                    PatientId = request.PatientId,
                    VisitId = visit.Id,
                    Id = video.Id,
                    QuestionnaireType = null,
                    VideoType = video.VideoType
                }));
        }
    }
}
