using DocMaster.Models;

namespace DocMaster.Services.StorageStrategies
{
    public interface IStorageStrategy
    {
        void Save(Document document, DocumentFormat format);
    }
}
