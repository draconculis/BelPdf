using System;
using System.ComponentModel.Composition;
using System.IO;
using Dek.Cls;
using Dek.DB;
using System.Windows.Forms;
using Dek.Bel.Core.Services;

namespace Dek.Bel.DB
{
    [Export]
    public class DatabaseAdminServiceSaveOpen
    {
        [Import] public IUserSettingsService m_UserSettingsService { get; set; }
        [Import] public IDBService m_DBService { get; set; }

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
