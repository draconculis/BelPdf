using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dek.Bel.Services
{
    [Export]
    public class TempFileService
    {
        [Import] IUserSettingsService UserSettingsService { get; set; }
        public string TmpFolderPath => Path.Combine(UserSettingsService.StorageFolder, "Tmp");

        /// <summary>
        /// Return a new unique tmp file name, pointing to the Tmp directory in storage.
        /// </summary>
        /// <param name="seedName"></param>
        /// <returns></returns>
        public string GetNewTmpFileName(string seedName = "")
        {
            EnsureTmpFolderExists();

            string guid = Guid.NewGuid().ToString().Replace("-", "").ToUpper();
            if(string.IsNullOrWhiteSpace(seedName))
                return Path.Combine(TmpFolderPath, guid + ".tmp");

            string name = Path.GetFileNameWithoutExtension(seedName);
            name = name.Substring(0, Math.Min(name.Length, 15));

            string res = Path.Combine(TmpFolderPath, name + "_" + guid.Substring(0, 4) + ".tmp");
            if (File.Exists(res))
                return GetNewTmpFileName(seedName);

            return res;
        }

        public bool DeleteAllTempFiles()
        {
            EnsureTmpFolderExists();

            var files = Directory.EnumerateFiles(TmpFolderPath);
            bool fail = false;
            foreach(var file in files)
            {
                try
                {
                    File.Delete(file);
                }
                catch
                {
                    fail = true;
                }
            }
            return fail;
        }

        public void EnsureTmpFolderExists()
        {
            if (!Directory.Exists(UserSettingsService.StorageFolder))
                Directory.CreateDirectory(UserSettingsService.StorageFolder);

            if (!Directory.Exists(TmpFolderPath))
                Directory.CreateDirectory(TmpFolderPath);
        }

    }
}
