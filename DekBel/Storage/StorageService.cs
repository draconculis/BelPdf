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
        [Import] public FileRepo FileRepo { get; set; }
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
        public ResultFileStorageData InitiateStorageForFile(RequestFileStorageData fileStorageData)
        {
            string srcPath = fileStorageData.FilePath;
            string stoFileName = GetStorageFileName(srcPath);
            string stoFolder = UserSettingsService.StorageFolder;
            string stoPath = Path.Combine(stoFolder, stoFileName);
            

            if (File.Exists(stoPath))
            {
                string srcHash = CalculateFileMD5(srcPath);
                stoPath = HandleStoFileCollision(srcHash, stoPath);
            }
            else
            {
                File.Copy(srcPath, stoPath);
            }

            



            var res = new ResultFileStorageData();
            res.StorageFilePath = stoPath;
            return res;
        }

        private string HandleStoFileCollision(string srcHash, string stoPath)
        {
            string stoHash = CalculateFileMD5(stoPath);




            return stoPath;
        }

        private string GetStorageFileName(string fileName)
        {
            string name = Path.GetFileNameWithoutExtension(fileName);
            string ext = Path.GetExtension(fileName);

            string storageFileName = name + ".bel" + ext;

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
