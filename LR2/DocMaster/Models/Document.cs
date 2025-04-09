using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocMaster.Models
{
    public class Document
    {
        public string Name { get; set; }
        public DocumentFormat Format { get; set; }
        public string Content { get; set; }
        public Document(string name, DocumentFormat format) 
        {
            Name = name;
            Format = format;
            Content = string.Empty;
        }
    }
}
