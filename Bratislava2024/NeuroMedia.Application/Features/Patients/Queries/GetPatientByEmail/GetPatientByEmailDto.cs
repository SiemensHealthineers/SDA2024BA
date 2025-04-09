using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NeuroMedia.Application.Common.Mappings;
using NeuroMedia.Domain.Entities;
using NeuroMedia.Domain.Enums;

namespace NeuroMedia.Application.Features.Patients.Queries.GetPatientByEmail
{
    public class GetPatientByEmailDto : IMapFrom<Patient>
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

    }
}
