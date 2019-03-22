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
    // ** Special service **
    // Called & instantiated from interOp
    // Handles files in files storage
    public class StorageService
    {
        [Import] public ModelsForViewing VM { get; set; }
        [Import] public StorageRepo m_StorageRepo { get; set; }
        [Import] public IDBService m_DBService { get; set; }
        [Import] public IUserSettingsService UserSettingsService { get; set; }

        public StorageService()
        {
            if(m_DBService == null)
                Mef.Initialize(this);
        }

        // The meat, entry point from interop
        public ResultFileStorageData GetStorage(RequestFileStorageData fileStorageData)
        {
            string srcPath = fileStorageData.FilePath;
            if (!File.Exists(srcPath))
                return new ResultFileStorageData
                {
                    Cancel = true,
                    StorageFilePath = "",
                };

            string srcHash = CalculateFileMD5(srcPath);

            // Do we even exist in db?
            Storage storage = m_StorageRepo.GetStorageByHash(srcHash);
            if (storage == null)
                storage = CreateNewStorageForFile(fileStorageData, srcHash);

            // Hmm file in store has been removed (this is sooo bad) - recreate it.
            if (!File.Exists(Path.Combine(UserSettingsService.StorageFolder, storage.StorageName)))
            {
                CopyFileToStorage(storage.FilePath, storage.StorageName);
                // TODO: ALSO RECREATE STUFF IN THE FILE FROM DB!! <============================================ o_O
            }

            // Save for posterity.
            var history = new History
            {
                Hash = srcHash,
                OpenDate = DateTime.Now,
                VolumeId = storage.VolumeId,
                StorageId = storage.Id,
            };
            m_DBService.InsertOrUpdate(history);

            var res = new ResultFileStorageData
            {
                StorageFilePath = Path.Combine(UserSettingsService.StorageFolder, storage.StorageName),
                Cancel = false,
            };

            return res;
        }

        /// <summary>
        /// We do not currently have a storage in DB. Create new and copy file to new storage!
        /// </summary>
        /// <param name="fileStorageData"></param>
        /// <param name="sourceHash"></param>
        /// <returns></returns>
        public Storage CreateNewStorageForFile(RequestFileStorageData fileStorageData, string sourceHash = null)
        {
            if (sourceHash.IsNullOrWhiteSpace())
                sourceHash = CalculateFileMD5(fileStorageData.FilePath);

            string stoFolder = UserSettingsService.StorageFolder;
            string stoFileName = GetUniqueStoName(fileStorageData.FilePath);
            string stoPath = Path.Combine(stoFolder, stoFileName);

            CopyFileToStorage(fileStorageData.FilePath, stoFileName);

            var volume = new Volume
            {
                Id = Id.NewId(),
                CreatedDate = DateTime.Now,
                Title = stoFileName,
            };

            var storage = new Storage
            {
                Id = Id.NewId(),
                Hash = sourceHash,
                FileName = Path.GetFileName(fileStorageData.FilePath),
                FilePath = fileStorageData.FilePath,
                StorageName = stoFileName,
                VolumeId = volume.Id,
                Date = DateTime.Now,
            };

            m_DBService.InsertOrUpdate(volume);
            m_DBService.InsertOrUpdate(storage);
            return storage;
        }

        public void CopyFileToStorage(string origFileName, string storageFileName)
        {
            string stoFolder = UserSettingsService.StorageFolder;
            string stoPath = Path.Combine(stoFolder, storageFileName);

            File.Copy(origFileName, stoPath);
        }
        
        /// <summary>
        /// Looks in the storage folder and generates a new unique file name.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="fileStorageFolder"></param>
        /// <returns></returns>
        private string GetUniqueStoName(string fileName)
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
