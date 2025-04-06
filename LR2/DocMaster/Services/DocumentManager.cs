using DocMaster.Services.FileService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DocMaster.Models;

namespace DocMaster.Services
{
    public class DocumentManager
    {
        private readonly IFileService _fileService;
        private readonly string _storagePath = Directory.GetCurrentDirectory();
        private const string ManifestFileName = ".docmaster_manifest";
        private readonly List<string> _createdDocuments = new();

        public DocumentManager(IFileService fileService)
        {
            _fileService = fileService;
            LoadManifest();
        }
        private void LoadManifest()
        {
            var manifestPath = Path.Combine(_storagePath, ManifestFileName);
            if (File.Exists(manifestPath))
            {
                var lines = File.ReadAllLines(manifestPath);
                _createdDocuments.AddRange(lines.Where(File.Exists));
            }
        }
        private void SaveManifest()
        {
            var manifestPath = Path.Combine(_storagePath, ManifestFileName);
            File.WriteAllLines(manifestPath, _createdDocuments);
        }

        public List<string> GetAvailableDocuments()
        {
            // Обновляем список, удаляя несуществующие файлы
            _createdDocuments.RemoveAll(path => !File.Exists(path));
            return new List<string>(_createdDocuments);
        }

        public Document CreateDocument(string name, DocumentFormat format)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Document name cannot be empty");

            var doc = new Document(name, format);
            var fullPath = GetFullPath(doc);
            _createdDocuments.Add(fullPath);
            SaveManifest();

            return doc;
        }

        private string GetFullPath(Document document)
        {
            var extensions = new Dictionary<DocumentFormat, string>
            {       
                { DocumentFormat.TXT, ".txt" },
                { DocumentFormat.Markdown, ".md" },
                { DocumentFormat.RichText, ".rtf" }
            };
            return Path.Combine(_storagePath, $"{document.Name}{extensions[document.Format]}");
        }
        public void SaveDocument(Document document)
        {
            var fullPath = GetFullPath(document);
            if (!_createdDocuments.Contains(fullPath))
            {
                _createdDocuments.Add(fullPath);
                SaveManifest();
            }
            _fileService.Save(document, _storagePath);
        }

        public List<string> GetDocumentList()
        {
            return _fileService.GetAvailableDocuments(_storagePath);
        }

        public Document OpenDocument(string filePath)
        {
            return _fileService.Load(filePath);
        }
    }
}
