using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuroMediaMobileApp.Models.DTOs
{
    public class AnswersDto
    {
        public IEnumerable<AnswerDto> Answers { get; set; } = [];
    }
}
