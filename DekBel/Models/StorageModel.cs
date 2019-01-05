using Dek.Bel.Cls;
using Dek.Bel.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dek.Bel.Models
{
    public class StorageModel
    {
        [Key]
        public Id Id { get; set; }
        public string Hash { get; set; }
        public string FileName { get; set; } // Original file
        public string FilePath { get; set; } // Original path
        public string StorageName { get; set; } // Name of file in storage
        public Id BookId { get; set; }
        public DateTime Date { get; set; }
        public string Comment { get; set; }

        public StorageModel() { }

        public StorageModel(Id id, string hash, string fileName, string filePath, string storageName, Id bookId, string comment)
        {
            Id = id;
            Hash = hash;
            FileName = fileName;
            FilePath = filePath;
            StorageName = storageName;
            Date = System.DateTime.Now;
            BookId = bookId;
            Comment = comment;
        }

        public StorageModel(string hash, string fileName, string filePath, string storageName, Id bookId, string comment)
            : this(new Id(), hash, fileName, filePath, storageName, bookId, comment)
        {
        }

    }
}
