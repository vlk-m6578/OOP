using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocMaster.Models.Documents
{
    public class MarkdownDocument : Document
    {
        public MarkdownDocument() => Type = DocumentType.Markdown;
        public override string GetFormattedContent() => Content;
    }

}
