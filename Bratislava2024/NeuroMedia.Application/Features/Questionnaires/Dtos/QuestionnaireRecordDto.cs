using NeuroMedia.Application.Common.Mappings;
using NeuroMedia.Application.Features.Questionnaires.Helpers;
using NeuroMedia.Domain.Enums;
using NeuroMedia.Domain.Entities;
using AutoMapper;

namespace NeuroMedia.Application.Features.Questionnaires.Dtos
{
    public class QuestionnaireRecordDto : IMapFrom<Questionnaire>
    {
        // Example value: Results/[PatientId]/[VisitId]/[QuestionnaireId]/[Type]/[Name]   ([Name]=[FileDateTime].json)
        public string BlobPath { get; set; } = default!;
        public int? PatientId { get; set; }
        public int? VisitId { get; set; }
        public int? Id { get; set; }
        public QuestionnaireType QuestionnaireType { get; set; }
        public string? FileName => string.IsNullOrEmpty(BlobPath) ? null : QuestionnaireHelper.GetFileName(BlobPath);
        public DateTime? FileDateTime => string.IsNullOrEmpty(BlobPath) ? null : QuestionnaireHelper.GetFileDateTime(BlobPath);

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Questionnaire, QuestionnaireRecordDto>()
                .ForMember(dest => dest.PatientId, opt => opt.MapFrom(src => src.Visit.PatientId));
        }
    }
}
