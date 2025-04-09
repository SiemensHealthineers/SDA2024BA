using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NeuroMedia.Application.Common.Mappings;
using NeuroMedia.Domain.Entities;
using NeuroMedia.Domain.Enums;

namespace NeuroMedia.Application.Features.Visits.Commands.CreateVisit
{
    public class CreateQuestionnaireDto : IMapFrom<Questionnaire>
    {
        public string? BlobPath { get; set; }
        public QuestionnaireType QuestionnaireType { get; set; }
    }
}
