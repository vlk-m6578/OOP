
using DocMaster.Save;

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
        public string ToJson() => new JsonAdapter().Convert(this);
        public string ToXml() => new XmlAdapter().Convert(this);
        public string ConvertTo(DocumentFormat format)
        {
            IDocumentAdapter adapter = format switch
            {
                DocumentFormat.TXT => new TxtAdapter(),
                DocumentFormat.JSON => new JsonAdapter(),
                DocumentFormat.XML => new XmlAdapter(),
                _ => throw new NotSupportedException($"Format {format} not supported")
            };

            return adapter.Convert(this);
        }
    }
}
