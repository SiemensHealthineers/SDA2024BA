using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NeuroMedia.Application.Common.Mappings;
using NeuroMedia.Domain.Entities;
using NeuroMedia.Domain.Enums;

namespace NeuroMedia.Application.Features.Visits.Queries
{
    public class GetAllVisitsDto : IMapFrom<Visit>
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public DateTime DateOfVisit { get; set; }
        public string? Note { get; set; }
        public VisitType VisitType { get; set; }

    }
}
