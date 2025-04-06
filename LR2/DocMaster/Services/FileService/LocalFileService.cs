using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DocMaster.Models;

namespace DocMaster.Services.FileService
{
    public class LocalFileService : IFileService
    {
        private readonly Dictionary<DocumentFormat, string> _extensions = new()
        {
            {DocumentFormat.TXT, ".txt" },
            {DocumentFormat.Markdown, ".md" },
            {DocumentFormat.RichText, ".rtf" }
        };
        public void Save(Document document, string path)
        {
            var fullPath = Path.Combine(path, $"{document.Name}{_extensions[document.Format]}");
            File.WriteAllText(fullPath, document.Content);
        }
        public Document Load(string fullPath)
        {
            var content = File.ReadAllText(fullPath);
            var fileName = Path.GetFileNameWithoutExtension(fullPath);
            var format = Path.GetExtension(fullPath).ToLower() switch
            {
                ".txt" => DocumentFormat.TXT,
                ".md" => DocumentFormat.Markdown,
                ".rtf" => DocumentFormat.RichText,
                _ => throw new NotSupportedException("Unsupported file format")
            };
            return new Document(fileName, format) { Content = content };
        }
        public List<string> GetAvailableDocuments(string directory)
        {
            var files = new List<string>();
            foreach (var format in _extensions.Values)
            {
                files.AddRange(Directory.GetFiles(directory, $"*{format}"));
            }
            return files;
        }
    }
}
