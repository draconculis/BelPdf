﻿using Dek.Bel.Models;
using Dek.Bel.Cls;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dek.Bel.DB
{
    [Export]
    public class HistoryRepo
    {
        [Import] public IDBService DBService { get; set; }

        public History GetLastOpened()
        {
            string sql = $"SELECT * FROM {nameof(History)} ORDER BY {nameof(History.OpenDate)} DESC LIMIT 1";

            DataTable dt = DBService.SelectBySql(sql);
            if (dt.Rows.Count != 1)
                return null;

            DataRow row = dt.Rows[0];

            var history = new History
            {
                Hash = (string)row[nameof(History.Hash)],
                OpenDate = ((string)row[nameof(History.OpenDate)]).ToSaneDateTime(),
                VolumeId = ((string)row[nameof(History.VolumeId)]).ToId(),
                StorageId = ((string)row[nameof(History.StorageId)]).ToId(),
            };

            return history;
        }

        public List<History> GetHistory()
        {
            string sql = $"SELECT * FROM {nameof(History)} ORDER BY {nameof(History.OpenDate)} DESC";

            DataTable dt = DBService.SelectBySql(sql);
            if (dt.Rows.Count < 1)
                return new List<History>();

            var histories = new List<History>();
            foreach (DataRow row in dt.Rows)
            {
                var history = new History
                {
                    Hash = (string)row[nameof(History.Hash)],
                    OpenDate = ((string)row[nameof(History.OpenDate)]).ToSaneDateTime(),
                    VolumeId = ((string)row[nameof(History.VolumeId)]).ToId(),
                    StorageId = ((string)row[nameof(History.StorageId)]).ToId(),
                };

                histories.Add(history);
            }

            return histories;
        }
    }
}
