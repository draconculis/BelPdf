using System.Data;

namespace Dek.Bel.DB
{
    public interface IDBService
    {
        DataTable Select(string query);
        bool ValueExists(string table, string column, string value);
        void Insert(string tablename, string columns, string values);
    }
}