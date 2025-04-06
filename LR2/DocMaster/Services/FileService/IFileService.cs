using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DocMaster.Models;

namespace DocMaster.Services.FileService
{
    public interface IFileService
    {
        void Save(Document document, string path);
        Document Load(string fullPath);
        List<string> GetAvailableDocuments(string directory);
    }
}
