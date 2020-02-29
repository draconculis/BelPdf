using Dek.Bel.Core.Models;

namespace Dek.Bel.Core.Services
{
    public interface IStorageHelperService
    {
        string CalculateFileMD5(string filePath);
        string GetUniqueStoName(string fileName);
        Storage GetNextStorageFileName(Storage storage);
        void DeleteOldStorageFiles(string storagePath);
    }
}