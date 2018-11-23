using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dek.Bel.UserSettings
{
    [Export(typeof(IUserSettingsService))]
    public class UserSettingsService : IUserSettingsService
    {
        public string ProgramDataPath { get; set; } = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\BelPdf";
        public string DBName { get; set; } = "BelPdf.sqlite";
        public string DBPath => Path.Combine(ProgramDataPath, DBName);


        [ImportingConstructor]
        public UserSettingsService()
        {
            if (!Directory.Exists(ProgramDataPath))
                Directory.CreateDirectory(ProgramDataPath); 
        }
    }
}
