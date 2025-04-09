using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuroMediaMobileApp.Models.Enums
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
