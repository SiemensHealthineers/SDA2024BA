using System.ComponentModel.DataAnnotations;

using AutoMapper;

using NeuroMedia.Application.Common.Mappings;
using NeuroMedia.Application.Common.Validations;
using NeuroMedia.Domain.Entities;
using NeuroMedia.Domain.Enums;

namespace NeuroMedia.Application.Features.Patients.Commands.UpdatePatient
{
    // [AutoMap(typeof(Patient), ReverseMap = true)]
    public class UpdatePatientDto : IMapFrom<Patient>
    {
        [Required]
        public string Name { get; init; } = default!;
        [Required]
        public string Surname { get; init; } = default!;
        [Required]
        [NotInFuture]
        public DateTime DateOfBirth { get; init; }
        [Required]
        [Range(0, 1)]
        public Sex? Sex { get; init; } = default!;
        [Required]
        [Range(0, 1)]
        public Diagnoses? Diagnosis { get; init; } = default!;
        [Required]
        [NotInFuture]
        public DateTime DateOfDiagnosis { get; init; }
        [Required]
        [EmailAddress]
        public string Email { get; init; } = default!;
        [Required]
        [Phone]
        public string PhoneNumber { get; init; } = default!;
        [Required]
        [Range(0, 5)]
        public EducationLevel? HighestEducation { get; init; }
        [Required]
        [Range(0, 5)]
        public EmploymentStatus? EmploymentStatus { get; init; }
        [Required]
        public bool RapExamination { get; init; }
        [Required]
        public bool PreviousBotulinumToxinApplication { get; init; }
        [Required]
        public bool IsActive { get; init; }

        //public void Mapping(Profile profile)
        //{
        //    profile.CreateMap<Patient, UpdatePatientDto>()
        //            .ReverseMap()
        //            .ForMember(d => d.Id, opt => opt.Ignore())
        //            .ForMember(d => d.CreatedBy, opt => opt.Ignore())
        //            .ForMember(d => d.CreatedDate, opt => opt.Ignore())
        //            .ForMember(d => d.UpdatedBy, opt => opt.Ignore())
        //            .ForMember(d => d.UpdatedDate, opt => opt.Ignore())
        //            .ForMember(d => d.Pseudonym, opt => opt.Ignore())
        //            .ForMember(d => d.TenantId, opt => opt.Ignore())
        //           .ForMember(d => d.UserId, opt => opt.Ignore());
        // }
    }
}
