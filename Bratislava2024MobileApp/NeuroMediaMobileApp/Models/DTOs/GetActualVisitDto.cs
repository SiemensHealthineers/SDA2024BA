using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NeuroMediaMobileApp.Models.Enums;

namespace NeuroMediaMobileApp.Models.DTOs
{
    public class GetActualVisitDto
    {
        public int Id { get; set; }
        public DateTime DateOfVisit { get; set; }
        public VisitType VisitType { get; set; }
        public ICollection<QuestionnaireRecordDto> Questionnaires { get; set; }
        public ICollection<VideoInfoDto> Videos { get; set; } = [];
    }
}
