using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocMaster.Models.Documents
{
    public abstract class Document
    {
        public string Content { get; protected set; } = "";
        public DocumentType Type { get; protected set; }
        public string FilePath { get; set; }
        public virtual void Insert(string text, int position) => Content = Content.Insert(position, text);
        public virtual void Delete(int position, int length) => Content = Content.Remove(position, length);
        public abstract string GetFormattedContent();
        public virtual void Save() => File.WriteAllText(FilePath, Content);
        public virtual void Load(string path)
        {
            FilePath = path;
            Content = File.ReadAllText(path);
        }

    }
}
