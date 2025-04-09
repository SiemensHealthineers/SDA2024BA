using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NeuroMedia.Domain.Enums;

namespace NeuroMedia.Application.Features.Visits.Queries.GetPendingTasks
{
    public class GetPendingTaskDto
    {
        public PendingTaskType PendingTaskType => VideoType == null ? PendingTaskType.Questionnaire : PendingTaskType.Video;
        public string Name { get; set; } = default!;
        public int? PatientId { get; set; }
        public int? VisitId { get; set; }
        public int? Id { get; set; }
        public QuestionnaireType? QuestionnaireType { get; set; }
        public VideoType? VideoType { get; set; }
    }
}
