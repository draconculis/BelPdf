using BelManagedLib;
using Dek.Bel.DB;
using Dek.Cls;
using Dek.Bel.Services;
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
using Dek.Bel.Cls;
using Dek.Bel.Models;

namespace Dek.Bel.Services
{
    [Export(typeof(IStorageHelperService))]
    public class StorageHelperService : IStorageHelperService
    {
        [Import] public ModelsForViewing VM { get; set; }
        [Import] public StorageRepo m_StorageRepo { get; set; }
        [Import] public IDBService m_DBService { get; set; }
        [Import] public IUserSettingsService UserSettingsService { get; set; }

        /// <summary>
        /// We have a storage in DB. Increment name, and update db.
        /// </summary>
        /// <param name="currentStorage"></param>
        /// <param name="sourceHash"></param>
        /// <returns></returns>
        public Storage GetNextStorageFileName(Storage storage)
        {
            string stoFolder = UserSettingsService.StorageFolder;
            string newStoFileName = storage.StorageName;
            while (File.Exists(Path.Combine(stoFolder, newStoFileName)))
                newStoFileName = GenerateNextStoName(newStoFileName);

            storage.StorageName = newStoFileName;

            m_DBService.InsertOrUpdate(storage);
            return storage;
        }

        /// <summary>
        /// Looks in the storage folder and generates a new unique file name.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="fileStorageFolder"></param>
        /// <returns></returns>
        public string GetUniqueStoName(string fileName)
        {
            string newFileName = GenerateFirstStoName(fileName);
            string stoFolder = UserSettingsService.StorageFolder;
            while (File.Exists(Path.Combine(stoFolder, newFileName)))
                newFileName = GenerateNextStoName(newFileName);

            return newFileName;
        }

        /// <summary>
        /// Return a new filename from a source fileName, e.g:
        /// "myfile.something.pdf" -> "myfile.something.bel.1.pdf"
        /// </summary>
        /// <param name="stoFileName"></param>
        /// <returns></returns>
        private string GenerateFirstStoName(string fileName)             // xx.pdf
        {
            string name0 = Path.GetFileNameWithoutExtension(fileName);   // xx
            string ext = Path.GetExtension(fileName);                    // .pdf

            return $"{name0}.bel.1{ext}";
        }

        /// <summary>
        /// Return a new filename from a generated stoFileName, e.g:
        /// "myfile.something.bel.1.pdf" -> "myfile.something.bel.2.pdf"
        /// </summary>
        /// <param name="stoFileName"></param>
        /// <returns></returns>
        private string GenerateNextStoName(string stoFileName)            // xx.bel.1.pdf
        {
            string name0 = Path.GetFileNameWithoutExtension(stoFileName); // xx.bel.1
            string name1 = Path.GetFileNameWithoutExtension(name0);       // xx.bel
            string name2 = Path.GetFileNameWithoutExtension(name1);       // xx
            string ext = Path.GetExtension(stoFileName);                  // .pdf
            string extNumeral = Path.GetExtension(name0);                 // .1

            int.TryParse(extNumeral.Substring(1), out int numeral);
            numeral++;
            return $"{name2}.bel.{numeral.ToString()}{ext}";
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
