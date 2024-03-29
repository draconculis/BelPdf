﻿using Dek.Bel.Core.Models;
using System;
using System.ComponentModel.Composition;
using System.Linq;
using Dek.DB;


namespace Dek.Bel.Core.DB
{
    [Export(typeof(IDBUpdater))]
    public class DBUpdater : IDBUpdater
    {
        IDBService Repo;
        private const string DBVersionTableName = "DBVersion";

        public void Upgrade(IDBService repo)
        {
            Repo = repo;
            DoUpgrade();
        }

        private void DoUpgrade()
        {
            int version = GetDBVersion();
            if (version < 0)
                throw new Exception("Failed to get DB version.");

            if (version == 0)
            {
                Repo.AddColumn(nameof(RawCitation), "`VolumeId` TEXT");
                version = IncrementDBVersion();
            }

            if (version == 1)
            {
                Repo.AddColumn(nameof(Volume), "`ISBN` TEXT");
                version = IncrementDBVersion();
            }


        }


        #region Helpers ======================================================================

        private int GetDBVersion()
        {
            if (!Repo.TableExists(DBVersionTableName))
            {
                Repo.CreateTable(typeof(DBVersion));
                InsertDBVersion(0);
                return 0;
            }

            DBVersion dbversion = Repo.Select<DBVersion>().FirstOrDefault();
            if(dbversion == null)
                return -1;

            int version;
            try
            {
                version = int.Parse(dbversion.Version);
            }
            catch
            {
                version = -1;
            }

            return version;
        }

        private int IncrementDBVersion()
        {
            try
            {
                int version = GetDBVersion();
                version++;
                InsertDBVersion(version);
                return version;
            }
            catch
            {
                return -1;
            }
        }

        private void InsertDBVersion(int version)
        {
            Repo.DeleteAll<DBVersion>();
            Repo.Insert(DBVersionTableName, "Version", version.ToString());
        }
        #endregion Helpers ===================================================================
    }
}
