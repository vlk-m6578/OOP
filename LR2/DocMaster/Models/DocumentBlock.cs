using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocMaster.Models
{
    public class DocumentBlock
    {
        public string FilePath { get; set; }
        public List<string> BlockedUsers { get; set; } = new();
    }
}
