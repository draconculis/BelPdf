using System;
using System.Collections.Generic;
using System.Data;

namespace Dek.Bel.DB
{
    public interface IDBService
    {
        bool CreateTable(object obj);
        bool CreateTable(Type type);

        DataTable SelectBySql(string query);
        List<T> Select<T>(string where = null) where T : new();
        T SelectById<T>(Id id) where T : new();
        bool ValueExists(string table, string column, string value);
        void Insert(string tablename, string columns, string values);
        void Update(string tablename, string where, string columnsValues);
        void InsertOrUpdate(object obj);
        void ClearTable<T>() where T : new();
        void ClearTable(object model);
    }
}