using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NeuroMediaMobileApp.Models.Enums;

namespace NeuroMediaMobileApp.Models.DTOs
{
    public class PendingTaskDto
    {
        public PendingTaskType PendingTaskType { get; set; }
        public string Name { get; set; } = default!;
        public int? PatientId { get; set; }
        public int? VisitId { get; set; }
        public int? Id { get; set; }
        public QuestionnaireType? QuestionnaireType { get; set; }
        public VideoType? VideoType { get; set; }
    }
}
