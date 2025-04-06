using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocMaster.Services.StorageStrategies
{
    public class LocalStorageStrategy : IStorageStrategy
    {
        public void Save(string content, string path, string fileName)
        {
            File.WriteAllText(Path.Combine(path, fileName), content);
        }
    }
}
