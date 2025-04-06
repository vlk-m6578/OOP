using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DocMaster.Models;

namespace DocMaster.Adapters
{
    public class XmlAdapter : IDocumentAdapter
    {
        public string ConvertContent(Document document)
        {
            return $"""
                <document>
                    <name>{document.Name}</name>
                    <format>{document.Format}</format>
                    <content><![CDATA[{document.Content}]]></content>
                </document>
                """;
        }
    }
}
