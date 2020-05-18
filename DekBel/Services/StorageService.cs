using BelManagedLib;
using Dek.Cls;
using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using Dek.Bel.Core.Models;
using Dek.DB;
using Dek.Bel.Core.DB;
using System.Collections.Generic;
using Dek.Bel.Core.Services;
using Dek.Bel.Core.GUI;

namespace Dek.Bel.Services
{
    // ---- Special service ----
    // Called & instantiated from interOp
    // Handles files in files storage
    public class StorageService
    {
        [Import] public ModelsForViewing VM { get; set; }
        [Import] public StorageRepo m_StorageRepo { get; set; }
        [Import] public IDBService m_DBService { get; set; }
        [Import] public IUserSettingsService UserSettingsService { get; set; }
        [Import] public IStorageHelperService StorageHelperService { get; set; }
        [Import] public IPdfService PdfService { get; set; }
        [Import] public VolumeService m_VolumeService { get; set; }

        public StorageService()
        {
            if(m_DBService == null)
                Mef.Initialize(this, new List<Type> { typeof(StorageRepo), typeof(MessageboxService) });
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

            string srcHash = StorageHelperService.CalculateFileMD5(srcPath);

            // Do we even exist in db?
            Storage storage = m_StorageRepo.GetStorageByHash(srcHash);
            if (storage == null)
                storage = CreateNewStorageForFile(fileStorageData, srcHash);

            if(VM.CurrentStorage == null)
                VM.CurrentStorage = storage;

            VM.CurrentStorage.FilePath = srcPath;

            // Hmm file in store has been removed (this is not bad) - recreate it.
            if (!File.Exists(Path.Combine(UserSettingsService.StorageFolder, storage.StorageName)))
            {
                //Volume vol = m_DBService.SelectById<Volume>(storage.VolumeId);
                //Citation citation = m_DBService.Select<Citation>($"`{nameof(Citation.VolumeId)}` = '{storage.VolumeId}'").FirstOrDefault();
                // TODO: RECREATE STUFF IN THE FILE FROM DB!! <============================================ o_O
                // Current citation not loaded at this point... Get current citation from db

                //VolumeService volSvc = new VolumeService();
                m_VolumeService.LoadVolume(storage.VolumeId);
                m_VolumeService.LoadCitations();
                if(m_VolumeService.Citations.Any())
                    PdfService.RecreateTheWholeThing(VM, m_VolumeService);
                else
                    CopyFileToStorage(storage.FilePath, storage.StorageName);
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
                sourceHash = StorageHelperService.CalculateFileMD5(fileStorageData.FilePath);

            string stoFolder = UserSettingsService.StorageFolder;
            string stoFileName = StorageHelperService.GetUniqueStoName(fileStorageData.FilePath);
            string stoPath = Path.Combine(stoFolder, stoFileName);

            CopyFileToStorage(fileStorageData.FilePath, stoFileName);

            var volume = new Volume
            {
                Id = Id.NewId(),
                CreatedDate = DateTime.Now,
                Title = Path.GetFileNameWithoutExtension(fileStorageData.FilePath),
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
    }
}
