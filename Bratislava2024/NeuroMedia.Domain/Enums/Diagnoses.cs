using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuroMedia.Domain.Enums
{
    public enum Diagnoses
    {
        [Display(Name = "Cervical Dystonia")]
        CervicalDystonia,

        [Display(Name = "Other")]
        OtherDiagnosis
    }
}
