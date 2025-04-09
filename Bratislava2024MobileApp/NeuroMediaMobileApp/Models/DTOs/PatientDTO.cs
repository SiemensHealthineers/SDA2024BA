using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NeuroMediaMobileApp.Models.Enums;

namespace NeuroMediaMobileApp.Models.DTOs
{
    public class PatientDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Sex Sex { get; set; }
        public Diagnosis Diagnosis { get; set; }
        public DateTime DateOfDiagnosis { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public EducationLevel HighestEducation { get; set; }
        public EmploymentStatus EmploymentStatus { get; set; }
        public bool RapExamination { get; set; }
        public bool PreviousBotulinumToxinApplication { get; set; }
        public string Pseudonym { get; set; }
        public string UserId { get; set; }
        public string TenantId { get; set; }
        public bool IsActive { get; set; }
    }
}
