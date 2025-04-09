using AutoMapper;
using NeuroMedia.Application.Common.Mappings;
using NeuroMedia.Application.Features.Patients.Queries.GetAllPatients;
using NeuroMedia.Domain.Entities;
using NeuroMedia.Domain.Enums;

namespace NeuroMedia.Application.Features.Patients.Queries.GetPatientById
{
    public class GetPatientByIdDto : IMapFrom<Patient>
    {
        public int Id { get; init; }
        public string Name { get; init; }
        public string Surname { get; init; }
        public DateTime DateOfBirth { get; init; }
        public Sex? Sex { get; init; }
        public Diagnoses? Diagnosis { get; init; }
        public DateTime DateOfDiagnosis { get; init; }
        public string Email { get; init; } = default!;
        public string PhoneNumber { get; init; }
        public EducationLevel? HighestEducation { get; init; }
        public EmploymentStatus? EmploymentStatus { get; init; }
        public bool RapExamination { get; init; }
        public bool PreviousBotulinumToxinApplication { get; init; }
        public string Pseudonym { get; init; } = default!;
        public string UserId { get; init; } = default!;
        public string TenantId { get; init; } = default!;
        public bool IsActive { get; init; }

        //public void Mapping(Profile profile)
        //{
        //    profile.CreateMap<Patient, GetPatientByIdDto>();
        //}
    }
}
