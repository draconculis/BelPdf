using System;
using System.Collections.Generic;
using System.Data;
using Dek.Bel.Cls;

namespace Dek.Bel.DB
{
    public interface IDBService
    {
        bool CreateTable(object obj);
        bool CreateTable(Type type);
        bool TableExists(string tableName);
        void AddColumn(string tableName, string columnDesc); // Ex: "`Id` TEXT, `Title` TEXT NOT NULL"

        DataTable SelectBySql(string query);
        DataTable SelectBySql<T>(string where) where T : new(); // Model gives all cols => SELECT *
        List<T> Select<T>(string where = null) where T : new();
        T SelectById<T>(Id id) where T : new();
        bool ValueExists(string table, string column, string value);
        void Insert(string tablename, string columns, string values);
        void Update(string tablename, string where, string columnsValues);
        void InsertOrUpdate(object obj);
        void DeleteAll<T>() where T : new();
        void DeleteAll(object model);
        void Delete<T>(T obj) where T : new();
        void Delete<T>(string where) where T : new();
        void Delete(object model, string where);

        // Some admin stuff
        void CloseDb(bool hardClose);
        void RenitializeDbConnection();
    }
}