using Dek.Bel.Services;
using System;
using System.ComponentModel.Composition;
using System.IO;
using Dek.Bel.Cls;
using System.Windows.Forms;

namespace Dek.Bel.DB
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

        public (bool cancel, string path) SelectDatabasePathForBackup()
        {
            SaveFileDialog fd = new SaveFileDialog();
            if (!string.IsNullOrWhiteSpace(m_UserSettingsService.LastSelectedDatabaseFile))
                fd.InitialDirectory = Path.GetDirectoryName(m_UserSettingsService.LastSelectedDatabaseFile);

            fd.FileName = Path.GetFileNameWithoutExtension(m_UserSettingsService.DBName) + "_" + DateTime.Now.ToCompactStringShort() + ".sqlite";

            //fd.Filter = "Database files (*.sqlite)|*.sqlite|All files (*.*)|*.*";
            fd.Filter = "Database files (*.sqlite)|*.sqlite";

            DialogResult result = fd.ShowDialog();
            if (result != DialogResult.OK)
                return (true, string.Empty);

            m_UserSettingsService.LastSelectedDatabaseFile = fd.FileName;
            return (false, fd.FileName);
        }

        public (bool cancel, string path) SelectDatabasePathForRestore()
        {
            OpenFileDialog fd = new OpenFileDialog();
            if (!string.IsNullOrWhiteSpace(m_UserSettingsService.LastSelectedDatabaseFile))
                fd.InitialDirectory = Path.GetDirectoryName(m_UserSettingsService.LastSelectedDatabaseFile);

            //fd.Filter = "Database files (*.sqlite)|*.sqlite|All files (*.*)|*.*";
            fd.Filter = "Database files (*.sqlite)|*.sqlite";

            DialogResult result = fd.ShowDialog();
            if (result != DialogResult.OK)
                return (true, string.Empty);

            m_UserSettingsService.LastSelectedDatabaseFile = fd.FileName;
            return (false, fd.FileName);
        }

    }
}
