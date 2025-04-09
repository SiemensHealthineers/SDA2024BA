using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NeuroMedia.Application.Common.Mappings;
using NeuroMedia.Application.Features.Questionnaires.Dtos;
using NeuroMedia.Domain.Entities;
using NeuroMedia.Domain.Enums;

namespace NeuroMedia.Application.Features.Visits.Commands.CreateVisit
{
    public class CreateVisitDto : IMapFrom<Visit>
    {
        [Required]
        [Range(0, 1)]
        public VisitType VisitType { get; set; }
        [Required]
        public int PatientId { get; set; }
        [Required]
        public DateTime DateOfVisit { get; set; }
        [Required]
        public int VisitId { get; set; }
    }
}
