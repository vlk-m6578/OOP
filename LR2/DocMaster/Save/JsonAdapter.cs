using DocMaster.Models;

namespace DocMaster.Save
{
    public class JsonAdapter : IDocumentAdapter
    {
        public string Convert(Document doc)
        {
            var mdContent = new MarkdownObject(doc.Content);
            return $$"""
            {
                "metadata": {
                    "name": "{{doc.Name}}",
                    "format": "{{doc.Format}}"
                },
                "content": {{mdContent.ToJson()}}
            }
            """;
        }
    }
}
