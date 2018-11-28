using System.Data;

namespace Dek.Bel.DB
{
    public interface IDBService
    {
        /*Some table names*/
        string TableCitationName { get; }

        string TableCategoryName { get; }
        string TableCitationCategoryName { get; }

        string TableFileName { get; }
        string TableCitationFileName { get; }

        DataTable Select(string query);
        bool ValueExists(string table, string column, string value);
        void Insert(string tablename, string columns, string values);
        void Update(string tablename, string where, string columnsValues);
    }
}