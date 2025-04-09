using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuroMediaMobileApp.Models.DTOs
{
    public class AnswerDto
    {
        public int QuestionId { get; set; }
        public int OptionId { get; set; }
        public string ResultValue { get; set; } = default!;
    }
}
