﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuroMediaMobileApp.Models.Enums
{
    public enum Diagnosis
    {
        [Display(Name = "Cervical Dystonia")]
        CervicalDystonia,

        [Display(Name = "Other")]
        OtherDiagnosis
    }
}

