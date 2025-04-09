using System.ComponentModel.DataAnnotations;

namespace NeuroMedia.Domain.Enums
{
    public enum EducationLevel
    {
        [Display(Name = "Complete or incomplete primary school")]
        CompleteOrIncompletePrimarySchool,

        [Display(Name = "Secondary school without diploma")]
        SecondarySchoolWithoutDiploma,

        [Display(Name = "Secondary school with diploma")]
        SecondarySchoolWithDiploma,

        [Display(Name = "University up to bachelor's level")]
        UniversityUpToBachelorsLevel,

        [Display(Name = "University (Master, Engineer, Doctor...)")]
        UniversityMastersEngineerDoctor,

        [Display(Name = "Higher than University")]
        HigherThanUniversity


    }
}
