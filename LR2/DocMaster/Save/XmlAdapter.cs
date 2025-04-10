using DocMaster.Models;

namespace DocMaster.Save
{
    public class XmlAdapter : IDocumentAdapter
    {
        public string Convert(Document doc)
        {
            var mdContent = new MarkdownObject(doc.Content);
            return $"""
            <Document>
                <Name>{doc.Name}</Name>
                <Format>{doc.Format}</Format>
                <Content>{mdContent.ToXml()}</Content>
            </Document>
            """;
        }
    }
}
