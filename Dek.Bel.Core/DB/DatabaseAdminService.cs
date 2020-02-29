using Dek.Bel.Core.Services;
using System;
using System.ComponentModel.Composition;
using System.IO;
using Dek.Cls;
using Dek.DB;

namespace Dek.Bel.Core.DB
{
    /// <summary>
    /// A class to administer physical database, backups etc
    /// </summary>
    [Export]
    public class DatabaseAdminService
    {
        [Import] public IUserSettingsService m_UserSettingsService { get; set; }
        [Import] public IDBService m_DBService { get; set; }

        public (bool error, string message) CopyDatabaseFile(string oldPath, string newPath)
        {
            try
            {
                if (oldPath == newPath)
                    return (false, "Old database and new database cannot be the same.");

                if (!File.Exists(oldPath))
                    return (false, "Cannot find database file to copy.");

                // For security, do not allow overwriting file in destination
                if (File.Exists(newPath))
                    return (false, "Cannot overwrite existing database file.");

                File.Copy(oldPath, newPath);

                return (true, string.Empty);
            }
            catch (Exception ex)
            {
                return (false, $"Exception: {ex}");
            }
        }

        /// <summary>
        /// Restore a backup, renaming current file
        /// </summary>
        /// <param name="oldPath"></param>
        /// <param name="newPath"></param>
        /// <returns></returns>
        public (bool success, string message) RestoreDatabaseFile(string sourcePath)
        {
            try
            {
                m_DBService.CloseDb(true);

                if (!File.Exists(sourcePath))
                    return (false, "Backup database file does not exist.");

                string databasePath = m_UserSettingsService.DBPath;

                // Rename database if it exists
                if (File.Exists(databasePath))
                {
                    DateTime now = DateTime.Now;
                    string datetimeCompact = now.ToCompactStringShort();
                    string buFileName = Path.GetFileNameWithoutExtension(databasePath) + "_" + datetimeCompact + ".sqlite";
                    string buFilePath = Path.Combine(Path.GetDirectoryName(databasePath), buFileName);
                    CopyDatabaseFile(databasePath, buFilePath);
                    File.Delete(databasePath);
                }

                File.Copy(sourcePath, databasePath, true);

                m_DBService.RenitializeDbConnection(); // Good as new

                return (true, string.Empty);
            }
            catch (Exception ex)
            {
                m_DBService.RenitializeDbConnection(); // Good as new
                return (false, $"Exception: {ex}");
            }
        }


    }
}
