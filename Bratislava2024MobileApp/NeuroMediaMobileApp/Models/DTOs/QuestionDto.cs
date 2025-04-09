using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuroMediaMobileApp.Models.DTOs
{
    public class QuestionDto
    {
        public int Id { get; set; }
        public string Text { get; set; } = default!;
        public string Type { get; set; } = default!; // Possible values: Radio,DropDown
        public IEnumerable<OptionDto> Options { get; set; } = [];
    }
}
