using Microsoft.EntityFrameworkCore;

using NeuroMedia.Domain.Common;
using NeuroMedia.Domain.Enums;

namespace NeuroMedia.Domain.Entities
{
    [Index(nameof(Email), IsUnique = true)]
    [Index(nameof(Pseudonym), IsUnique = true)]
    public class Patient : BaseAuditableEntity
    {
        public string Name { get; set; } = default!;
        public string Surname { get; set; } = default!;
        public DateTime DateOfBirth { get; set; }
        public Sex? Sex { get; set; } = default!;
        public Diagnoses? Diagnosis { get; set; } = default!;
        public DateTime DateOfDiagnosis { get; set; }
        public string Email { get; set; } = default!;
        public string PhoneNumber { get; set; } = default!;
        public EducationLevel? HighestEducation { get; set; }
        public EmploymentStatus? EmploymentStatus { get; set; }
        public bool RapExamination { get; set; }
        public bool PreviousBotulinumToxinApplication { get; set; }
        public string Pseudonym { get; set; } = default!;
        public string UserId { get; set; } = default!;
        public string TenantId { get; set; } = default!;
        public bool IsActive { get; set; } = true;
        public ICollection<Visit> Visits { get; set; } = new List<Visit>();
    }
}
