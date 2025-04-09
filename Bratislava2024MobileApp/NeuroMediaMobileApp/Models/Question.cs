using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuroMediaMobileApp.Models
{
    public class Question
    {
        public string Text { get; set; }
        public string Type { get; set; } // "SingleChoice", "TextInput", etc.
        public List<string> Options { get; set; } // Only for SingleChoice
    }
}
