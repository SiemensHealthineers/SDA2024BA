using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NeuroMedia.Domain.Common;
using NeuroMedia.Domain.Enums;

namespace NeuroMedia.Domain.Entities
{
    public class Questionnaire : BaseAuditableEntity
    {
        public int VisitId { get; set; }
        public Visit Visit { get; set; } = default!;
        public QuestionnaireType QuestionnaireType { get; set; }
        public string? BlobPath { get; set; }
    }
}
