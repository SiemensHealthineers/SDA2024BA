using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NeuroMedia.Application.Common.Mappings;
using NeuroMedia.Application.Features.Questionnaires.Dtos;
using NeuroMedia.Application.Features.Videos.Dtos;
using NeuroMedia.Domain.Entities;
using NeuroMedia.Domain.Enums;

namespace NeuroMedia.Application.Features.Visits.Queries.GetActualVisit
{
    public class GetActualVisitDto : IMapFrom<Visit>
    {
        public int Id { get; set; }
        public DateTime DateOfVisit { get; set; }
        public VisitType VisitType { get; set; }
        public ICollection<QuestionnaireRecordDto> Questionnaires { get; set; } = [];
        public ICollection<VideoInfoDto> Videos { get; set; } = [];
    }
}
