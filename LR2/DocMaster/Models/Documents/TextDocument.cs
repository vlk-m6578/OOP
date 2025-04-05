using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocMaster.Models.Documents
{
    public class TextDocument : Document
    {
        public TextDocument() => Type = DocumentType.Text;
        public override string GetFormattedContent() => Content;
    }
}
