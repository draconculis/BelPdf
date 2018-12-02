using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dek.Bel.Models
{
    public class StorageModel
    {
        public string Id { get; set; }
        public string Hash { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string StorageName { get; set; }

        public StorageModel() { }

        public StorageModel(string id, string hash, string fileName, string filePath, string storageName)
        {
            Id = id;
            Hash = hash;
            FileName = fileName;
            FilePath = filePath;
            StorageName = storageName;
        }
    }
}
