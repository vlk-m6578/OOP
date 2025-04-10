using DocMaster.Models;

namespace DocMaster.Save
{
    public interface IDocumentAdapter
    {
        public string Convert(Document doc);
    }
}
