using NeuroMedia.Application.Common.Mappings;
using AutoMapper;
using NeuroMedia.Application.Features.Patients.Queries.GetAllPatients;
using NeuroMedia.Domain.Entities;
using NeuroMedia.Domain.Enums;
using NeuroMedia.Application.Features.Questionnaires.Dtos;
using NeuroMedia.Application.Features.Videos.Dtos;
namespace NeuroMedia.Application.Features.VisitDetails.Queries.GetVisitDetailsById
{
    public class GetVisitDetailsByIdDto : IMapFrom<Visit>
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public DateTime DateOfVisit { get; set; }
        public VisitType VisitType { get; set; }
        public string? Note { get; set; }
        public ICollection<QuestionnaireRecordDto> Questionnaires { get; set; } = [];
        public ICollection<VideoInfoDto> Videos { get; set; } = [];

    }

}
