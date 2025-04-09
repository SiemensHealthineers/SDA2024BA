using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NeuroMediaMobileApp.Models.Enums;
using NeuroMediaMobileApp.Helpers;

namespace NeuroMediaMobileApp.Models.DTOs
{
    public class QuestionnaireDto
    {
        public string BlobPath { get; set; } = default!;
        public QuestionnaireType? QuestionnaireType => QuestionnaireHelper.GetQuestionnaireType(BlobPath);

        public IEnumerable<QuestionDto> Questions { get; set; } = [];
    }
}
