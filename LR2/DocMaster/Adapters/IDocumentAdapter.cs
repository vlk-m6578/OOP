using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DocMaster.Models;

namespace DocMaster.Adapters
{
    public interface IDocumentAdapter
    {
        string ConvertContent(Document document);
    }
}
