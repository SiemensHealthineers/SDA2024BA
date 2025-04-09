using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NeuroMediaMobileApp.Models.Enums;
using NeuroMediaMobileApp.Helpers;

namespace NeuroMediaMobileApp.Models.DTOs
{
    public class QuestionnaireRecordDto
    {
        // Example value: Results/[PatientId]/[VisitId]/[QuestionnaireId]/[Type]/[Name]   ([Name]=[FileDateTime].json)
        public string BlobPath { get; set; } = default!;
        public int? PatientId { get; set; }
        public int? VisitId { get; set; }
        public int? Id { get; set; }
        public QuestionnaireType? QuestionnaireType { get; set; }
        public string? FileName { get; set; }
        public DateTime? FileDateTime { get; set; }

        public IEnumerable<AnswerDto> Answers { get; set; } = [];
    }
}
