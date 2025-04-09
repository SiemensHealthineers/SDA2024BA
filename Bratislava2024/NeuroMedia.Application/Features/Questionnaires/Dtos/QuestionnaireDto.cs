using NeuroMedia.Application.Features.Questionnaires.Helpers;
using NeuroMedia.Domain.Enums;

namespace NeuroMedia.Application.Features.Questionnaires.Dtos
{
    public class QuestionnaireDto
    {
        // Example value: Questionnaires/[PatientId]/[VisitId]/[QuestionnaireId]/[Type]/[FileName]   ([FileName]=[FileDateTime].json)
        public string BlobPath { get; set; } = default!;
        public QuestionnaireType? QuestionnaireType => (BlobPath == null) ? null : QuestionnaireHelper.GetQuestionnaireType(BlobPath);

        public IEnumerable<QuestionDto> Questions { get; set; } = [];
    }
}
