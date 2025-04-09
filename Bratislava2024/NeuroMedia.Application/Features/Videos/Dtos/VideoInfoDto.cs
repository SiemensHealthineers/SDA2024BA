using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AutoMapper;

using NeuroMedia.Application.Common.Mappings;
using NeuroMedia.Application.Features.Questionnaires.Helpers;
using NeuroMedia.Application.Features.Videos.Helpers;
using NeuroMedia.Domain.Entities;
using NeuroMedia.Domain.Enums;

namespace NeuroMedia.Application.Features.Videos.Dtos
{
    public class VideoInfoDto : IMapFrom<Video>
    {
        // Example value: Videos/[PatientId]/[VisitId]/[VideoId]/[Type]/[Name]   ([Name]=[FileDateTime].mp4)
        public string BlobPath { get; set; } = default!;
        public int? PatientId { get; set; }
        public int? VisitId { get; set; }
        public int? Id { get; set; }
        public VideoType? VideoType { get; set; }
        public string? FileName => string.IsNullOrEmpty(BlobPath) ? null : VideoHelper.GetFileName(BlobPath);
        public DateTime? FileDateTime => string.IsNullOrEmpty(BlobPath) ? null : VideoHelper.GetFileDateTime(BlobPath);

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Video, VideoInfoDto>()
                .ForMember(dest => dest.PatientId, opt => opt.MapFrom(src => src.Visit.PatientId));
        }
    }
}
