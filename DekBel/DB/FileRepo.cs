using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.Composition;
using Dek.Bel.Models;

namespace Dek.Bel.DB
{
    [Export(typeof(FileRepo))]
    public class FileRepo
    {
        [Import] IDBService dBService { get; set; }

        public void AddOrUpdateFile(string sourceFile)
        {

        }

        public FileStorageInfo GetFileInfoByHash(string hash)
        {
            var res = dBService.Select($"select SrcPath, StorageName from {dBService.TableFileName} where Hash = '{hash}'");
            if (res.Rows.Count == 0)
                return null;

            string src = (string)(res.Rows[0].ItemArray[0]);
            string sto = (string)(res.Rows[0].ItemArray[1]);

            FileStorageInfo stoInfo = new FileStorageInfo(hash, src, sto);
            return stoInfo;
        }
    }
}
