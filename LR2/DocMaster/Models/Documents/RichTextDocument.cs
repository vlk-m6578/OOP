using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocMaster.Models.Documents
{
    public class RichTextDocument : Document
    {
        private List<TextFormat> _formatting = new();
        public RichTextDocument() => Type = DocumentType.RichText;
        public override void Insert(string text, int position)
        {
            base.Insert(text, position);
            _formatting.InsertRange(position, new TextFormat[text.Length].Select(_ => new TextFormat()));
        }
        public override string GetFormattedContent()
        {
            return Content;
        }
    }
}
