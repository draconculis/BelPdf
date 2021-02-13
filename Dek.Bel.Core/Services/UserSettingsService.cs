using System;
using System.ComponentModel.Composition;
using System.Drawing;
using System.IO;

namespace Dek.Bel.Core.Services
{
    [Export(typeof(IUserSettingsService))]
    public class UserSettingsService : IUserSettingsService
    {
        private const string belPdfSubfolder = "BelPdf";
        private const string dbSubFolderPathBeta = "data_beta1";
        private const string dbSubFolderPath = "Data";
        private const string reportsSubFolderPath = "Reports";
        public string DBName { get; } = "BelPdf.sqlite";
        public string DBPathBeta => Path.Combine(StorageFolder, dbSubFolderPathBeta + "\\" + DBName);
        public string DBPath => Path.Combine(StorageFolder, dbSubFolderPath + "\\" + DBName);
        public string ReportFolder => Path.Combine(StorageFolder, reportsSubFolderPath);
        public string DeselectionMarker => (string)Properties.Settings.Default.DeselectionMarker ?? "…";
        private const string StorageFolderSettingName = "StorageFolder";
        private const string LastSelectedDatabaseFileName = "LastSelectedDatabaseFile";
        

        public string StorageFolder
        {
            get
            {
                EnsureStorageFolderExists();
                return Get<string>(StorageFolderSettingName, Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), belPdfSubfolder));
            }
            set
            {
                Set(StorageFolderSettingName, value);
                EnsureStorageFolderExists();
            }
        }

        public string LastSelectedDatabaseFile
        {
            get => Get<string>(LastSelectedDatabaseFileName, Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), belPdfSubfolder, dbSubFolderPath, DBName));
            set => Set(LastSelectedDatabaseFileName, value);
        }

        public string ReportsFolder
        {
            get => Get<string>(nameof(ReportsFolder), Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), belPdfSubfolder, reportsSubFolderPath));
            set => Set(nameof(ReportsFolder), value);
        }

        public string LatestExportSaveFolder
        {
            get => Get<string>(nameof(LatestExportSaveFolder), ReportsFolder);
            set => Set(nameof(LatestExportSaveFolder), value);
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

        #region PdfMarginBox ==================================================

        public Color PdfMarginBoxColor
        {
            get => Get(nameof(PdfMarginBoxColor), Color.AliceBlue);
            set => Set(nameof(PdfMarginBoxColor), value);
        }

        public int PdfMarginBoxWidth
        {
            get => Get(nameof(PdfMarginBoxWidth), 56);
            set => Set(nameof(PdfMarginBoxWidth), value);
        }

        public int PdfMarginBoxHeight
        {
            get => Get(nameof(PdfMarginBoxHeight), 13);
            set => Set(nameof(PdfMarginBoxHeight), value);
        }

        public int PdfMarginBoxMargin
        {
            get => Get(nameof(PdfMarginBoxMargin), 11);
            set => Set(nameof(PdfMarginBoxMargin), value);
        }

        public float PdfMarginBoxBorder
        {
            get => Get(nameof(PdfMarginBoxBorder), 0f);
            set => Set(nameof(PdfMarginBoxBorder), value);
        }

        public string PdfMarginBoxVisualMode
        {
            get => Get(nameof(PdfMarginBoxVisualMode), Constants.MarginBoxVisualMode.Normal);
            set => Set(nameof(PdfMarginBoxVisualMode), value);
        }

        public bool PdfMarginBoxRightMargin
        {
            get => Get(nameof(PdfMarginBoxRightMargin), false);
            set => Set(nameof(PdfMarginBoxRightMargin), value);
        }

        public string PdfMarginBoxFont
        {
            get => Get(nameof(PdfMarginBoxFont), Constants.PdfFont.TIMES_ROMAN);
            set => Set(nameof(PdfMarginBoxFont), value);
        }

        public float PdfMarginBoxFontSize
        {
            get => Get(nameof(PdfMarginBoxFontSize), 9f);
            set => Set(nameof(PdfMarginBoxFontSize), value);
        }

        #endregion PdfMarginBox ===============================================

        public bool ShowDebugMessages
        {
            get => Get(nameof(ShowDebugMessages), false);
            set => Set(nameof(ShowDebugMessages), value);
        }

        public bool AutoWritePdfOnClose
        {
            get => Get(nameof(AutoWritePdfOnClose), true);
            set => Set(nameof(AutoWritePdfOnClose), value);
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
            EnsureSettingExists(StorageFolderSettingName, Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + $"\\{belPdfSubfolder}");

            string storageFolderPath = (string)Properties.Settings.Default[StorageFolderSettingName];
            if (!Directory.Exists(storageFolderPath))
                Directory.CreateDirectory(storageFolderPath);

            string dbFolderPath = Path.Combine(storageFolderPath, dbSubFolderPath);
            if (!Directory.Exists(dbFolderPath))
                Directory.CreateDirectory(dbFolderPath);
        }
    }
}
