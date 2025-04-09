﻿using Microsoft.EntityFrameworkCore;
using NeuroMedia.Domain.Common;
using NeuroMedia.Domain.Enums;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuroMedia.Domain.Entities
{
    public class Visit : BaseAuditableEntity
    {
        public VisitType VisitType { get; set; }
        public int PatientId { get; set; }
        public DateTime DateOfVisit { get; set; }
        public string? Note { get; set; } = string.Empty;
        public Patient Patient { get; set; } = default!;
        public ICollection<Questionnaire> Questionnaires { get; set; } = [];
        public ICollection<Video> Videos { get; set; } = [];
    }
}
