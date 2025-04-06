using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DocMaster.Models;

namespace DocMaster.Adapters
{
    public class JsonAdapter : IDocumentAdapter
    {
        public string ConvertContent(Document document)
        {
            return $$"""
                {
                    "name": "{{document.Name}}",
                    "format": "{{document.Format}}",
                    "content": "{{document.Content.Replace("\"", "\\\"")}}"
                }
                """;
        }
    }
}
