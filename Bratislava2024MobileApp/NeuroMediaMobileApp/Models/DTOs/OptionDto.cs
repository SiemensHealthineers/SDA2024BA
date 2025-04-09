using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuroMediaMobileApp.Models.DTOs
{
    public class OptionDto
    {
        public int Id { get; set; }
        public string Text { get; set; } = default!;
        public string Value { get; set; } = default!;
    }
}
