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
        private readonly string _manifestPath;

        public DocumentManager(IFileService fileService, string storagePath)
        {
            _fileService = fileService;
            _storagePath = storagePath;
            _manifestPath = Path.Combine(storagePath, ManifestFileName);
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
            _fileService.Save(document, _storagePath);
            _fileService.AddToManifest(_manifestPath, fullPath);
        }

        public List<string> GetDocumentList()
        {
            return _fileService.ReadManifest(_manifestPath)
            .Where(File.Exists)
            .ToList();
        }

        public Document OpenDocument(string filePath)
        {
            return _fileService.Load(filePath);
        }
        public void DeleteDocument(string filePath)
    {
        try
        {
            // Удаляем из файловой системы
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            
            // Удаляем из манифеста
            _fileService.RemoveFromManifest(_manifestPath, filePath);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting document: {ex.Message}");
        }
    }
    }
}
