using System.Data.SQLite;

namespace Dek.Bel.DB
{
    public interface IDBCreator
    {
        void Create(IDBService dbConnection);
    }
}