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
                _ => throw new NotSupportedException("-----> Unsupported file format")
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
        public void RemoveFromManifest(string manifestPath, string filePath)
        {
            if (!File.Exists(manifestPath)) return;

            var entries = File.ReadAllLines(manifestPath)
                .Where(entry => entry != filePath)
                .ToList();

            File.WriteAllLines(manifestPath, entries);
        }

        public void AddToManifest(string manifestPath, string filePath)
        {
            var entries = new List<string>();
            if (File.Exists(manifestPath))
            {
                entries = File.ReadAllLines(manifestPath).ToList();
            }

            if (!entries.Contains(filePath))
            {
                entries.Add(filePath);
                File.WriteAllLines(manifestPath, entries);
            }
        }

        public List<string> ReadManifest(string manifestPath)
        {
            return File.Exists(manifestPath)
                ? File.ReadAllLines(manifestPath).ToList()
                : new List<string>();
        }
    }
}
