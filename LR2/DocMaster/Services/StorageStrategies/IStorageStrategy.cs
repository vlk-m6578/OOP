
namespace DocMaster.Services.StorageStrategies
{
    public interface IStorageStrategy
    {
        void Save(string content, string path, string fileName);
    }
}
