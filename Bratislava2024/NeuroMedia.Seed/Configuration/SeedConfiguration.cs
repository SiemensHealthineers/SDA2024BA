using NeuroMedia.Domain.Enums;

namespace NeuroMedia.Seed.Configuration
{
    public class SeedConfiguration
    {
        public IReadOnlyList<PatientConfiguration> Patients { get; init; } = default!;
        public IReadOnlyList<VisitConfiguration> Visits { get; init; } = default!;
        public IReadOnlyList<QuestionnaireConfiguration> Questionnaires { get; init; } = default!;
        public IReadOnlyList<VideoConfiguration> Videos { get; init; } = default!;
    }

    public class PatientConfiguration
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
    }

    public class VisitConfiguration
    {
        public VisitType VisitType { get; set; }
        public DateTime DateOfVisit { get; set; }
        public string Note { get; set; } = default!;
        public int PatientId { get; set; }
        public string UpdatedBy { get; set; } = default!;
        public string CreatedBy { get; set; } = default!;
    }

    public class QuestionnaireConfiguration
    {
        public int VisitId { get; set; }
        public QuestionnaireType QuestionnaireType { get; set; }
        public string? BlobPath { get; set; }
        public string UpdatedBy { get; set; } = default!;
        public string CreatedBy { get; set; } = default!;
    }

    public class VideoConfiguration
    {
        public int VisitId { get; set; }
        public VideoType VideoType { get; set; }
        public string? BlobPath { get; set; }
        public string UpdatedBy { get; set; } = default!;
        public string CreatedBy { get; set; } = default!;
    }
}
