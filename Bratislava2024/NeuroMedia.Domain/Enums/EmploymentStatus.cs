using System.ComponentModel.DataAnnotations;

namespace NeuroMedia.Domain.Enums
{
    public enum EmploymentStatus
    {
        [Display(Name = "Full-time employed")]
        FullTimeEmployed,

        [Display(Name = "Part-time employed")]
        PartTimeEmployed,

        [Display(Name = "Self-employed")]
        SelfEmployed,

        Unemployed,
        Student,
        Retired
    }
}
