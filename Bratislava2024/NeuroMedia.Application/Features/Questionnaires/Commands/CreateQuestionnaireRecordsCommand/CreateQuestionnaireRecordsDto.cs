using NeuroMedia.Application.Common.Mappings;
using NeuroMedia.Domain.Entities;
using NeuroMedia.Domain.Enums;

namespace NeuroMedia.Application.Features.Questionnaires.Commands.CreateQuestionnaireRecordsCommand
{
    public class CreateQuestionnaireRecordsDto : IMapFrom<Questionnaire>
    {
        public int VisitId { get; set; }
        public QuestionnaireType QuestionnaireType { get; set; }
        public string? BlobPath { get; private set; }
    }
}
