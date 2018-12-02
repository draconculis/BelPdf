using BelManagedLib;
using Dek.Bel.DB;
using Dek.Bel.UserSettings;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dek.Bel.Storage
{
    // Called & instantiated from interOp
    // Handles files in files storage
    public class StorageService
    {
        //[Import] public IDBService DBService { get; set; }
        [Import] public StorageRepo FileRepo { get; set; }
        [Import] public IUserSettingsService UserSettingsService { get; set; }

        public StorageService()
        {
            Meffify();
        }

        private void Meffify()
        {
            var catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new AssemblyCatalog(typeof(BelGui).Assembly));

            var container = new CompositionContainer(catalog);

            try
            {
                container.ComposeParts(this);
            }
            catch (CompositionException compositionException)
            {
                MessageBox.Show($"{Environment.NewLine}{compositionException}", "Composition error");
            }
        }

        // The meat, entry point from interop
        public ResultFileStorageData InitiateNewStorageForFile(RequestFileStorageData fileStorageData)
        {
            string srcPath = fileStorageData.FilePath;
            string stoFileName = GetStorageFileName(srcPath);
            string stoFolder = UserSettingsService.StorageFolder;
            string stoPath = Path.Combine(stoFolder, stoFileName);
            string srcHash = CalculateFileMD5(srcPath);

            // Do we exist in db



            if (File.Exists(stoPath))
            {
                stoPath = GenerateUniqueStoName(stoFileName);
            }
            else
            {
                File.Copy(srcPath, stoPath);
            }

            



            var res = new ResultFileStorageData();
            res.StorageFilePath = stoPath;
            return res;
        }

        private string GenerateUniqueStoName(string stoPath)
        {
            




            return stoPath;
        }

        private string GetStorageFileName(string fileName, int iteration = 1)
        {
            string name = Path.GetFileNameWithoutExtension(fileName);
            string ext = Path.GetExtension(fileName);

            string storageFileName = name + ".bel." + iteration.ToString() + ext;

            return storageFileName;
        }

        public string CalculateFileMD5(string filePath)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(filePath))
                {
                    var hash = md5.ComputeHash(stream);
                    return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                }
            }
        }


    }
}
