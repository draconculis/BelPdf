using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.Composition;
using Dek.Bel.Models;
using System.Data;
using Dek.Bel.Cls;

namespace Dek.Bel.DB
{
    [Export(typeof(StorageRepo))]
    public class StorageRepo
    {
        [Import] IDBService dBService { get; set; }
        string SqlSelectFields = $"SELECT `Id`, `Hash`, `SourceFileName`, `SourceFilePath`, `StorageFileName`, `BookId`, `Date`, `Comment`";

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
                Id = new Id(res.Rows[0].ItemArray[0] as string),
                Hash = (string)(res.Rows[0].ItemArray[1]),
                FileName = (string)(res.Rows[0].ItemArray[2]),
                FilePath = (string)(res.Rows[0].ItemArray[3]),
                StorageName = (string)(res.Rows[0].ItemArray[4]),
                BookId = new Id(res.Rows[0].ItemArray[5] as string),
                //Date = DateTime.Parse((string)(row.ItemArray[4])),
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
                    Id = new Id(res.Rows[0].ItemArray[0] as string),
                    Hash = (string)(row.ItemArray[1]),
                    FileName = (string)(row.ItemArray[2]),
                    FilePath = (string)(row.ItemArray[3]),
                    StorageName = (string)(row.ItemArray[4]),
                    BookId = new Id(res.Rows[0].ItemArray[5] as string),
                    //Date = DateTime.Parse((string)(row.ItemArray[6])),
                    Comment = (string)(row.ItemArray[7]),
                });
            }

            return models;
        }


        public string Insert(StorageModel model)
        {
            if (model.Id.IsNull)
                model.Id = new Id();

            dBService.Insert(dBService.TableStorageName,
                $"`Id`, `Hash`, `SourceFileName`, `SourceFilePath`, `StorageFileName`, `BookId`, `Date`, `Comment`",
                $"'{model.Id}','{model.Hash}','{model.FileName}','{model.FilePath}','{model.StorageName}','{model.BookId}','{model.Date.ToSaneString()}','{model.Comment}'");

            return model.Id.ToString();
        }
    }
}
