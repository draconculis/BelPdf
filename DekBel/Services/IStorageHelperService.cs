using Dek.Bel.Models;

namespace Dek.Bel.Services
{
    public interface IStorageHelperService
    {
        string CalculateFileMD5(string filePath);
        string GetUniqueStoName(string fileName);
        Storage GetNextStorageFileName(Storage storage);
    }
}