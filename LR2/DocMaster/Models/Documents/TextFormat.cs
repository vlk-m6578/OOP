using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocMaster.Models.Documents
{
    public class TextFormat
    {
        public ConsoleColor Color { get; set; } = ConsoleColor.Gray;
        public bool Bold { get; set; }
        public bool Italic { get; set; }
        public bool Underline { get; set; }
    }
}
