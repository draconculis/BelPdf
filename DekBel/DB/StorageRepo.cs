using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.Composition;
using Dek.Bel.Models;
using System.Data;

namespace Dek.Bel.DB
{
    [Export(typeof(StorageRepo))]
    public class StorageRepo
    {
        [Import] IDBService dBService { get; set; }
        string SqlSelectFields = $"SELECT `Id`, `Hash`, `FileName`, `FilePath`, `SourcePath`";

        public void AddOrUpdateFile(string sourceFile)
        {

        }

        public StorageModel GetStorageByHash(string hash)
        {
            string sql = $"{SqlSelectFields} FROM {dBService.TableStorageName} WHERE Hash = '{hash}'";
            return Get(sql);
        }

        public StorageModel GetStorageById(string id)
        {
            string sql = $"{SqlSelectFields} FROM {dBService.TableStorageName} WHERE `Id` = '{id}'";
            return Get(sql);
        }

        public StorageModel Get(string sql)
        {
            var res = dBService.Select(sql);

            if (res.Rows.Count == 0)
                return null;

            StorageModel stoModel = new StorageModel()
            {
                Id = (string)(res.Rows[0].ItemArray[0]),
                Hash = (string)(res.Rows[0].ItemArray[1]),
                FileName = (string)(res.Rows[0].ItemArray[2]),
                FilePath = (string)(res.Rows[0].ItemArray[3]),
                StorageName = (string)(res.Rows[0].ItemArray[4]),
            };

            return stoModel;
        }

        public List<StorageModel> GetMany(string sql)
        {
            var res = dBService.Select(sql);

            if (res.Rows.Count == 0)
                return null;

            List<StorageModel> models = new List<StorageModel>();
            foreach (DataRow row in res.Rows)
            {
                models.Add(new StorageModel()
                {
                    Id = (string)(row.ItemArray[0]),
                    Hash = (string)(row.ItemArray[1]),
                    FileName = (string)(row.ItemArray[2]),
                    FilePath = (string)(row.ItemArray[3]),
                    StorageName = (string)(row.ItemArray[4]),
                });
            }

            return models;
        }
    }
}
