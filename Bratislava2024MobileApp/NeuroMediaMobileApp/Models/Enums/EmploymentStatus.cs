using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuroMediaMobileApp.Models.Enums
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
