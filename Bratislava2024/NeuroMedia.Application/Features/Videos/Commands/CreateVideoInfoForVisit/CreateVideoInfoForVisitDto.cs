using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NeuroMedia.Application.Common.Mappings;
using NeuroMedia.Domain.Entities;
using NeuroMedia.Domain.Enums;

namespace NeuroMedia.Application.Features.Videos.Commands.CreateVideoInfoForVisit
{

    public class CreateVideoInfoForVisitDto : IMapFrom<Video>
    {
        public int VisitId { get; set; }
        public VideoType VideoType { get; set; }
        public string? BlobPath { get; private set; }
    }
}
