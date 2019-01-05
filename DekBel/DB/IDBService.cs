using System;
using System.Collections.Generic;
using System.Data;

namespace Dek.Bel.DB
{
    public interface IDBService
    {
        /*Some table names*/
        string TableCitationName { get; }

        string TableCategoryName { get; }
        string TableCitationCategoryName { get; }

        string TableStorageName { get; }
        string TableCitationStorageName { get; }

        bool CreateTable(object obj);
        bool CreateTable(Type type);

        DataTable Select(string query);
        List<T> Select<T>(string where) where T : new();
        bool ValueExists(string table, string column, string value);
        void Insert(string tablename, string columns, string values);
        void Update(string tablename, string where, string columnsValues);
        void InsertOrUpdate(object obj);
    }
}