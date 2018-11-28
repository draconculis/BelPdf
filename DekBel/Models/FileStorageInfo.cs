using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dek.Bel.Models
{
    public class FileStorageInfo
    {
        public string StorageName { get; set; }
        public string SrcPath { get; set; }
        public string Hash { get; set; }

        public FileStorageInfo(string hash, string srcPath, string storageName)
        {
            StorageName = storageName;
            SrcPath = srcPath;
            Hash = hash;
        }
    }
}
