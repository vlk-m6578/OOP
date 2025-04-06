using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using DocMaster.Models;

namespace DocMaster.Adapters
{
    public class TextAdapter : IDocumentAdapter
    {
        public string ConvertContent(Document document) => document.Content;
    }
}
