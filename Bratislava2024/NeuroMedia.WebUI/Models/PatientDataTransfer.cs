using NeuroMedia.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace NeuroMedia.WebUI.Models
{
    public class PatientDataTransfer
    {
        [Required(ErrorMessage = "Name is required.")]
        public string? Name { get; set; } = string.Empty;


        [Required(ErrorMessage = "Surname is required.")]
        public string? Surname { get; set; } = string.Empty;


        [Required(ErrorMessage = "Date of Birth is required.")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }


        [Required(ErrorMessage = "Sex is required.")]
        public Sex? Sex { get; set; }

        [Required(ErrorMessage = "Diagnosis is required.")]
        public Diagnoses? Diagnosis { get; set; }


        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; } = default!;

        [Required(ErrorMessage = "Phone number is required.")]
        [Phone(ErrorMessage = "Invalid phone number.")]
        public string PhoneNumber { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Date of Diagnosis is required.")]
        public DateTime DateOfDiagnosis { get; set; }

        [Required(ErrorMessage = "Highest achieved education is required.")]
        public EducationLevel? HighestEducation { get; set; }

        [Required(ErrorMessage = "Employment status is required.")]
        public EmploymentStatus? EmploymentStatus { get; set; }

        public bool RapExamination { get; set; }
        public bool PreviousBotulinumToxinApplication { get; set; }

        public bool IsActive { get; set; }
    }
}
