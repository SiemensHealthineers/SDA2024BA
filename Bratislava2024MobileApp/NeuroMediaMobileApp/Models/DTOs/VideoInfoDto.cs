using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NeuroMediaMobileApp.Models.Enums;

namespace NeuroMediaMobileApp.Models.DTOs
{
    public class VideoInfoDto
    {
        // Example value: Videos/[PatientId]/[VisitId]/[VideoId]/[Type]/[Name]   ([Name]=[FileDateTime].mp4)
        public string BlobPath { get; set; } = default!;
        public int? PatientId { get; set; }
        public int? VisitId { get; set; }
        public int? Id { get; set; }
        public VideoType? VideoType { get; set; }
        public string? FileName { get; set; }
        public DateTime? FileDateTime { get; set; }
    }
}
