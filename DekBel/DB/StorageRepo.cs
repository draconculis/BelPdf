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

        public Storage GetStorageByHash(string hash)
        {
            string where = $"`{nameof(Storage.Hash)}` = '{hash}'";
            var res = dBService.Select<Storage>(where);

            if (res.Count == 0)
                return null;

            return res.First();
        }



        public Storage Get(string sql)
        {
            var res = dBService.SelectBySql(sql);

            if (res.Rows.Count == 0)
                return null;

            Models.Storage stoModel = new Models.Storage()
            {
                Id = new Id(res.Rows[0].ItemArray[0] as string),
                Hash = (string)(res.Rows[0].ItemArray[1]),
                FileName = (string)(res.Rows[0].ItemArray[2]),
                FilePath = (string)(res.Rows[0].ItemArray[3]),
                StorageName = (string)(res.Rows[0].ItemArray[4]),
                VolumeId = new Id(res.Rows[0].ItemArray[5] as string),
                //Date = DateTime.Parse((string)(row.ItemArray[4])),
            };

            return stoModel;
        }

        public List<Storage> GetMany(string sql)
        {
            var res = dBService.SelectBySql(sql);

            if (res.Rows.Count == 0)
                return null;

            List<Models.Storage> models = new List<Models.Storage>();
            foreach (DataRow row in res.Rows)
            {
                models.Add(new Models.Storage()
                {
                    Id = new Id(res.Rows[0].ItemArray[0] as string),
                    Hash = (string)(row.ItemArray[1]),
                    FileName = (string)(row.ItemArray[2]),
                    FilePath = (string)(row.ItemArray[3]),
                    StorageName = (string)(row.ItemArray[4]),
                    VolumeId = new Id(res.Rows[0].ItemArray[5] as string),
                    //Date = DateTime.Parse((string)(row.ItemArray[6])),
                    Comment = (string)(row.ItemArray[7]),
                });
            }

            return models;
        }
    }
}
