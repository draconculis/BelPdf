using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dek.Bel.UserSettings
{
    [Export(typeof(IUserSettingsService))]
    public class UserSettingsService : IUserSettingsService
    {
        private const string dbSubFolderPath = "db";
        public string DBName { get; } = "BelPdf.sqlite";
        public string DBPath => Path.Combine(StorageFolder, dbSubFolderPath + "\\" + DBName);

        private const string StorageFolderSettingName = "StorageFolder";
        public string StorageFolder {
            get
            {
                EnsureStorageFolderExists();
                return (string)Properties.Settings.Default[StorageFolderSettingName];
            }
            set
            {
                Properties.Settings.Default[StorageFolderSettingName] = value;
                EnsureStorageFolderExists();
                EnsureSettingExists("Font", new Font(FontFamily.GenericSerif, (float)12, FontStyle.Regular));
            }
        }

        [ImportingConstructor]
        public UserSettingsService()
        {
            EnsureStorageFolderExists();
        }

        /// <summary>
        /// Ensure setting exists. Ensure storage folder and db folder exists.
        /// </summary>
        public void EnsureSettingExists(string setting, object defaultValue)
        {
            if (string.IsNullOrWhiteSpace((string)Properties.Settings.Default[setting]))
                Properties.Settings.Default[setting] = defaultValue;
        }

        public void EnsureStorageFolderExists()
        {
            EnsureSettingExists(StorageFolderSettingName, Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\BelPdf")                    ;

            string storageFolderPath = (string)Properties.Settings.Default[StorageFolderSettingName];
            if (!Directory.Exists(storageFolderPath))
                Directory.CreateDirectory(storageFolderPath);

            string dbFolderPath = Path.Combine(storageFolderPath, dbSubFolderPath);
            if (!Directory.Exists(dbFolderPath))
                Directory.CreateDirectory(dbFolderPath);
        }

    }
}
