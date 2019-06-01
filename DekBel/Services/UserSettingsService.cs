﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dek.Bel.Services
{
    [Export(typeof(IUserSettingsService))]
    public class UserSettingsService : IUserSettingsService
    {
        private const string dbSubFolderPath = "data_alpha2";
        public string DBName { get; } = "BelPdf.sqlite";
        public string DBPath => Path.Combine(StorageFolder, dbSubFolderPath + "\\" + DBName);
        public string DeselectionMarker => (string)Properties.Settings.Default.DeselectionMarker ?? "…";
        private const string StorageFolderSettingName = "StorageFolder";

        public string StorageFolder
        {
            get
            {
                EnsureStorageFolderExists();
                return Get<string>(StorageFolderSettingName);
            }
            set
            {
                Set(StorageFolderSettingName, value);
                EnsureStorageFolderExists();
            }
        }

        public Font CitationFont
        {
            get => Get(nameof(CitationFont), new Font(FontFamily.GenericSerif, (float)12, FontStyle.Regular));
            set => Set(nameof(CitationFont), value);
        }

        public bool BoldEmphasis
        {
            get => Get(nameof(BoldEmphasis), true);
            set => Set(nameof(BoldEmphasis), value);
        }

        public bool UnderlineEmphasis
        {
            get => Get(nameof(UnderlineEmphasis), false);
            set => Set(nameof(UnderlineEmphasis), value);
        }

        public Color PdfHighLightColor
        {
            get => Get(nameof(PdfHighLightColor), Color.AliceBlue);
            set => Set(nameof(PdfHighLightColor), value);
        }

        public Color PdfUnderlineColor
        {
            get => Get(nameof(PdfUnderlineColor), Color.DarkRed);
            set => Set(nameof(PdfUnderlineColor), value);
        }
        public bool ShowDebugMessages
        {
            get => Get(nameof(ShowDebugMessages), false);
            set => Set(nameof(ShowDebugMessages), value);
        }

        private T Get<T>(string settingName, T defaultvalue = default(T))
        {
            EnsureSettingExists(settingName, defaultvalue);
            return (T)Properties.Settings.Default[settingName];
        }

        private void Set<T>(string settingName, T setting)
        {
            EnsureSettingExists(settingName, default(T));
            Properties.Settings.Default[settingName] = setting;
        }

        [ImportingConstructor]
        public UserSettingsService()
        {
            EnsureStorageFolderExists();
        }

        /// <summary>
        /// Ensure setting exists. Ensure storage folder and db folder exists.
        /// </summary>
        private void EnsureSettingExists(string setting, object defaultValue)
        {
            if (Properties.Settings.Default[setting] is string text)
                if (string.IsNullOrWhiteSpace(text))
                {
                    Properties.Settings.Default[setting] = defaultValue;
                    return;
                }

            if (Properties.Settings.Default[setting] == null)
                Properties.Settings.Default[setting] = defaultValue;
        }

        public void EnsureStorageFolderExists()
        {
            EnsureSettingExists(StorageFolderSettingName, Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\BelPdf");

            string storageFolderPath = (string)Properties.Settings.Default[StorageFolderSettingName];
            if (!Directory.Exists(storageFolderPath))
                Directory.CreateDirectory(storageFolderPath);

            string dbFolderPath = Path.Combine(storageFolderPath, dbSubFolderPath);
            if (!Directory.Exists(dbFolderPath))
                Directory.CreateDirectory(dbFolderPath);
        }

    }
}
