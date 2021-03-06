﻿using Dek.Bel.Cls;
using Dek.Bel.Models;
using Dek.Bel.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dek.Bel.DB
{
    [Export(typeof(IDBService))]
    public class Sqlite : IDBService, IDisposable
    {
        private readonly IUserSettingsService m_UserSettingsService;
        SQLiteConnection m_dbConnection;

        // Table names
        public string TableBookName => "Book";
        public string TableRawCitationName => "RawCitation";
        public string TableCitationName => "Citation";
        public string TableCategoryName => "Category";
        public string TableCitationCategoryName => "CitationCategory";
        public string TableStorageName => "Storage";
        public string TableCitationStorageName => "CitationFile";
        public string TableCitationBookName => "CitationBook";

        private const string ColCode = "Code";
        private const string ColName = "Name";
        private const string ColDescription = "Description";

        [ImportingConstructor]
        public Sqlite(IUserSettingsService userSettingsService, IDBCreator creator)
        {
            m_UserSettingsService = userSettingsService;
            InitLocal(creator);
        }

        public void InitLocal(IDBCreator creator)
        {
            if(!File.Exists(m_UserSettingsService.DBPath))
                SQLiteConnection.CreateFile(m_UserSettingsService.DBPath);

            m_dbConnection = new SQLiteConnection($"Data Source={m_UserSettingsService.DBPath};Version=3;New=False;");
            creator.Create(this);
        }

        public List<T> Select<T>(string where = null) where T : new()
        {
            T obj = new T();
            List<T> result = new List<T>();
            string tableName = obj.GetType().Name;

            string sqlQuery = $"SELECT * FROM {tableName}";
            if (!string.IsNullOrWhiteSpace(where))
                sqlQuery += $" WHERE {where}";

            DataTable dt = SelectBySql(sqlQuery);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                obj = new T();
                foreach (var prop in obj.GetType().GetProperties())
                {
                    LoadProperty(obj, prop, dt.Rows[i]);
                    //string name = prop.Name;
                    //object val = dt.Rows[i][name];
                    //bool isNullable = Nullable.GetUnderlyingType(prop.GetType()) != null;
                    //bool isEnum = prop.GetType().IsEnum;
                    //Type type = prop.PropertyType;
                    //string typeName = type.Name;
                    //if (isEnum)
                    //    typeName = "enum";

                    //switch (typeName)
                    //{
                    //    case "int":
                    //        prop.SetValue(obj, (int)(Int64)val);
                    //        break;
                    //    case "uint":
                    //        prop.SetValue(obj, (uint)(UInt64)val);
                    //        break;
                    //    case nameof(Int32):
                    //        prop.SetValue(obj, (Int32)(Int64)val);
                    //        break;
                    //    case nameof(UInt32):
                    //        prop.SetValue(obj, (UInt32)(UInt64)val);
                    //        break;
                    //    case "long":
                    //        prop.SetValue(obj, (long)(Int64)val);
                    //        break;
                    //    case nameof(Int64):
                    //        prop.SetValue(obj, (Int64)val);
                    //        break;
                    //    case "ulong":
                    //        prop.SetValue(obj, (ulong)(Int64)val);
                    //        break;
                    //    case nameof(UInt64):
                    //        prop.SetValue(obj, (UInt64)(Int64)val);
                    //        break;
                    //    case nameof(Decimal):
                    //        prop.SetValue(obj, (Decimal)val);
                    //        break;
                    //    case nameof(DateTime):
                    //        prop.SetValue(obj, ((string)val).ToSaneDateTime());
                    //        break;
                    //    case "enum":
                    //        prop.SetValue(obj, Enum.Parse(type, ((string)val)));
                    //        break;
                    //    case nameof(Id):
                    //        prop.SetValue(obj, Id.NewId((string)val));
                    //        break;
                    //    default:
                    //        prop.SetValue(obj, val);
                    //        break;
                    //}
                }
                result.Add(obj);
            }

            return result;
        }

        public T SelectById<T>(Id id) where T : new()
        {
            T obj = new T();
            string tableName = obj.GetType().Name;

            string sqlQuery = $"SELECT * FROM `{tableName}` WHERE `Id`='{id}'";

            DataTable dt = SelectBySql(sqlQuery);
            if (dt.Rows.Count != 1)
                return default(T);

            LoadObject(obj, dt.Rows[0]);

            return obj;
        }

        /// <summary>
        /// Select with one extension table. Requires naming of ext ref in ext table to be "MainTableId" if main table is "MainTable".
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<(T Main, T1 Ext1)> Select<T, T1>(string where) where T : new() where T1 : new()
        {
            T main = new T();
            List<T> result = new List<T>();
            string mainTableName = main.GetType().Name;

            string columnSql = "";
            string mainTableId = "Id";
            foreach (var prop in main.GetType().GetProperties())
            {
                if (prop.GetCustomAttributes(typeof(KeyAttribute), true).FirstOrDefault() != null)
                    mainTableId = prop.Name;

                if (columnSql.Length > 0)
                    columnSql += ", ";
                columnSql += $"{prop.Name}";
            }

            List<Type> extTypes = new List<Type> { typeof(T1) };
            string fromSql = $"{mainTableName}";
            foreach (Type type in extTypes)
            {

                T1 ext = new T1();
                string extTableName = ext.GetType().Name;
                string extTableId = $"{mainTableName}{mainTableId}";

                foreach (var prop in type.GetType().GetProperties())
                {
                    if (prop.GetCustomAttributes(typeof(KeyAttribute), true).FirstOrDefault() != null)
                        extTableId = prop.Name;

                    string refKey = ((RefKeyAttribute)prop.GetCustomAttributes(typeof(RefKeyAttribute), true).FirstOrDefault())?.RefKey;
                    if (refKey != null)
                        extTableId = refKey;

                    if (refKey != null || prop.Name == extTableId)
                        continue;

                    columnSql += ", ";
                    columnSql += $"{extTableName}.{prop.Name} as '{extTableName}.{prop.Name}'";
                }
                fromSql += $" left join {extTableName}.{extTableId} on {mainTableName}.{mainTableId}";
            }

            string sqlQuery = $"SELECT {columnSql} FROM {fromSql} WHERE {where}";
            DataTable dt = SelectBySql(sqlQuery);

            var res = new List<(T Main, T1 Ext1)>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow row = dt.Rows[i];
                main = new T();
                LoadObject(main, row);
                T1 ext1 = new T1();
                LoadObject(ext1, row, true);

                res.Add((main, ext1));
            }

            return res;
        }

        /// <summary>
        /// Loads one object propery with the correct value from the provided DataRow.
        /// Property name matches column name. If prefixTableName then columnName is 'tableName.propertyName'.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="prop"></param>
        /// <param name="row"></param>
        private void LoadProperty(object obj, PropertyInfo prop, DataRow row, bool prefixTableName = false)
        {
            string name = (prefixTableName ? $"{obj.GetType().Name}." : "") + prop.Name;
            object val;
            try
            {
                val = row[name];
            }
            catch
            {
                return;
            }

            bool isNullable = Nullable.GetUnderlyingType(prop.GetType()) != null;
            bool isEnum = prop.GetType().IsEnum;
            Type type = prop.PropertyType;
            string typeName = type.Name;
            if (isEnum)
                typeName = "enum";

            switch (typeName)
            {
                case "int":
                    prop.SetValue(obj, (int)(Int64)val);
                    break;
                case "uint":
                    prop.SetValue(obj, (uint)(UInt64)val);
                    break;
                case nameof(Int32):
                    prop.SetValue(obj, (Int32)(Int64)val);
                    break;
                case nameof(UInt32):
                    prop.SetValue(obj, (UInt32)(UInt64)val);
                    break;
                case "long":
                    prop.SetValue(obj, (long)(Int64)val);
                    break;
                case nameof(Int64):
                    prop.SetValue(obj, (Int64)val);
                    break;
                case "ulong":
                    prop.SetValue(obj, (ulong)(Int64)val);
                    break;
                case nameof(UInt64):
                    prop.SetValue(obj, (UInt64)(Int64)val);
                    break;
                case nameof(Decimal):
                    prop.SetValue(obj, (Decimal)val);
                    break;
                case nameof(DateTime):
                    prop.SetValue(obj, ((string)val).ToSaneDateTime());
                    break;
                case "enum":
                    prop.SetValue(obj, Enum.Parse(type, ((string)val)));
                    break;
                case nameof(Id):
                    prop.SetValue(obj, Id.NewId((string)val));
                    break;
                default:
                    prop.SetValue(obj, val);
                    break;
            }
        }


        /// <summary>
        /// Load an object from a DataRow. Column name to load from matches property name in object.
        /// </summary>
        /// <param name="obj">Object to load into.</param>
        /// <param name="row">A datarow with columns that matches property names in obj.</param>
        /// <param name="prefixTableName">Name of a column in dataRow is either "ColumnName" or "TableName.ColumnName" depending on param prefixTableName.</param>
        private void LoadObject(object obj, DataRow row, bool prefixTableName = false)
        {
            foreach (var prop in obj.GetType().GetProperties())
            {
                LoadProperty(obj, prop, row, prefixTableName);

                //string name = (prefixTableName ? $"{obj.GetType().Name}." : "") + prop.Name;
                //object val = null;
                //try
                //{
                //    val = row[name];
                //}
                //catch
                //{
                //    continue;
                //}
                //bool isNullable = Nullable.GetUnderlyingType(prop.GetType()) != null;
                //bool isEnum = prop.GetType().IsEnum;
                //Type type = prop.GetType();
                //string typeName = type.Name;
                //if (isEnum)
                //    typeName = "enum";

                //switch (typeName)
                //{
                //    case nameof(DateTime):
                //        prop.SetValue(obj, ((string)val).ToSaneDateTime());
                //        break;
                //    case "enum":
                //        prop.SetValue(obj, Enum.Parse(type, ((string)val)));
                //        break;
                //    default:
                //        prop.SetValue(obj, val);
                //        break;
                //}

            }

        }

        public DataTable SelectBySql(string query)
        {
            DataTable dt = new DataTable();

            try
            {
                SQLiteDataAdapter ad;
                SQLiteCommand cmd;
                m_dbConnection.Open();  //Initiate connection to the db
                cmd = m_dbConnection.CreateCommand();
                cmd.CommandText = query;  //set the passed query
                ad = new SQLiteDataAdapter(cmd);
                ad.Fill(dt); //fill the datasource
            }
            catch (SQLiteException sqlex)
            {
                Console.WriteLine($"{sqlex}");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex}");

            }
            finally
            {
                m_dbConnection.Close();
            }
            return dt;
        }


        public bool ValueExists(string table, string column, string value)
        {
            DataTable dt = new DataTable();

            try
            {
                SQLiteDataAdapter ad;
                SQLiteCommand cmd;
                m_dbConnection.Open();  //Initiate connection to the db
                cmd = m_dbConnection.CreateCommand();
                cmd.CommandText = $"select {column} from {table} where {column} = {value}";  //set the passed query
                ad = new SQLiteDataAdapter(cmd);
                ad.Fill(dt); //fill the datasource
            }
            catch (SQLiteException sqlex)
            {
                Console.WriteLine($"{sqlex}");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex}");
            }
            finally
            {
                m_dbConnection.Close();
            }

            return dt.Rows.Count > 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tablename"></param>
        /// <param name="columnDesc">Ex: "`Id` TEXT, `Title` TEXT NOT NULL"</param>
        /// <param name="primaryKey"></param>
        /// <returns></returns>
        public bool CreateTable(string tablename, string columnDesc, string primaryKey)
        {
            try
            {
                using (SQLiteCommand command = m_dbConnection.CreateCommand())
                {
                    m_dbConnection.Open();
                    command.CommandText = $"SELECT name FROM sqlite_master WHERE name='{tablename}'";
                    var name = command.ExecuteScalar();
                    if (name != null && name.ToString() == $"{tablename}")
                        return false;

                    //"CREATE TABLE `Storage` ( `Hash` TEXT NOT NULL, `SourceFileName` TEXT NOT NULL, `SourceFilePath` TEXT NOT NULL, `StorageFileName` TEXT NOT NULL UNIQUE, PRIMARY KEY(`Hash`) )"
                    // table not exist, create table and insert 
                    if (columnDesc.EndsWith(","))
                        throw new Exception($"Create {tablename}: No ending ',', stupido!");

                    bool primaryDescEmpty = string.IsNullOrWhiteSpace(primaryKey);

                    if (primaryDescEmpty)
                    {
                        // Accept no primary key
                        command.CommandText = $"CREATE TABLE `{tablename}` ({columnDesc})";
                    }
                    else
                    {
                        if (!primaryKey.StartsWith("`") || !primaryKey.EndsWith("`"))
                            throw new Exception($"Create {tablename}: No `backticks` in primary key desc, stupido!");

                        command.CommandText = $"CREATE TABLE `{tablename}` ({columnDesc}, PRIMARY KEY({primaryKey})) ";
                    }

                    MessageBox.Show(command.CommandText);
                    command.ExecuteNonQuery();
                }
            }
            catch (SQLiteException sqlex)
            {
                MessageBox.Show(sqlex.ToString(), "Sql exception");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Exception");

            }
            finally
            {
                m_dbConnection.Close();
            }
            return true;

        }

        /// <summary>
        /// Checks if a table exists
        /// </summary>
        /// <param name="tablename"></param>
        /// <param name="columnDesc">Ex: "`Id` TEXT, `Title` TEXT NOT NULL"</param>
        /// <param name="primaryKey"></param>
        /// <returns></returns>
        public bool TableExists(string tableName)
        {
            try
            {
                using (SQLiteCommand command = m_dbConnection.CreateCommand())
                {
                    m_dbConnection.Open();
                    command.CommandText = $"SELECT name FROM sqlite_master WHERE name='{tableName}'";
                    var name = command.ExecuteScalar();
                    if (name != null && name.ToString() == $"{tableName}")
                        return true;
                }
            }
            catch (SQLiteException sqlex)
            {
                MessageBox.Show(sqlex.ToString(), "Sql exception");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Exception");
            }
            finally
            {
                m_dbConnection.Close();
            }

            return false;
        }


        public bool CreateTable(Type modelType)
        {
            object obj = Activator.CreateInstance(modelType);
            return CreateTable(obj);
        }

        /// <summary>
        /// Creates a table from a model object
        /// </summary>
        /// <param name="obj">Model</param>
        /// <returns>true if created, false if error or already exists</returns>
        public bool CreateTable(object obj)
        {
            if (obj == null)
                return false;

            string tableName = obj.GetType().Name;
            if (TableExists(tableName))
                return false;

            string colDefs = "";
            string keyDefs = "";
            foreach (var prop in obj.GetType().GetProperties())
            {
                string typeName = prop.PropertyType.Name;
                object keyAttributes = prop.GetCustomAttributes(typeof(KeyAttribute), true).FirstOrDefault();
                bool isKey = prop.Name.ToLower() == "id" || keyAttributes != null;
                string sqlType;
                string value = prop.GetValue(obj, null)?.ToString() ?? "NULL";
                bool isNullable = Nullable.GetUnderlyingType(prop.GetType()) != null || typeName.ToLower() == "string";

                string name = prop.Name;
                if (prop.GetType().IsEnum)
                    typeName = "enum";

                // Enums default to TEXT - NOT their const value.
                // Datetime defaults to TEST
                switch (typeName)
                {
                    case "byte":
                    case "Byte":
                    case "sbyte":
                    case "SByte":
                    case "short":
                    case "int":
                    case "long":
                    case "ushort":
                    case "uint":
                    case "ulong":
                    case "bool":
                    case "Boolean":
                    case "Int32":
                    case "UInt32":
                    case "Int16":
                    case "UInt16":
                    case "Int64":
                    case "UInt64":
                    case "Single":
                        sqlType = "INTEGER";
                        break;
                    case "float":
                    case "Float":
                    case "double":
                    case "Double":
                    case "decimal":
                    case "Decimal":
                        sqlType = "REAL";
                        break;
                    default:
                        sqlType = "TEXT";
                        break;
                }

                colDefs += (colDefs.Length > 0 ? ", " : "") + $"`{prop.Name}` {sqlType}{(isNullable ? "" : " NOT NULL")}";

                if (isKey)
                {
                    keyDefs += (keyDefs.Length > 0 ? ", " : "") + $"`{prop.Name}`";
                }
            }

            return CreateTable(tableName, colDefs, keyDefs);
        }

        public void ExecuteNonQuery(string cmd)
        {
            try
            {
                using (SQLiteCommand command = m_dbConnection.CreateCommand())
                {
                    m_dbConnection.Open();
                    command.CommandText = cmd;
                    command.ExecuteNonQuery();
                }
            }
            catch (SQLiteException sqlex)
            {
                Console.WriteLine($"{sqlex}");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex}");

            }
            finally
            {
                m_dbConnection.Close();
            }

        }

        public void CreateIndex(string table, string column)
        {
            string createindexstatement = $"CREATE UNIQUE INDEX `{table}_{column}` ON `{table}` (`{column}`)";
            ExecuteNonQuery(createindexstatement);
        }


        public void Insert(string tablename, string columns, string values)
        {
            try
            {
                using (SQLiteCommand command = m_dbConnection.CreateCommand())
                {
                    m_dbConnection.Open();
                    command.CommandText = $"INSERT INTO {tablename} ({columns}) VALUES ({values})";
                    MessageBox.Show(command.CommandText);
                    command.ExecuteNonQuery();
                }
            }
            catch (SQLiteException sqlex)
            {
                Console.WriteLine($"{sqlex}");
                MessageBox.Show($"{sqlex}");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex}");
                MessageBox.Show($"{ex}");
            }
            finally
            {
                m_dbConnection.Close();
            }

        }


        public void InsertOrUpdate(object obj)
        {
            // TODO: Handle Enum

            if (obj == null)
                return;

            string names = "", values = "", updateValues = "", updateWhereValues = "";
            //List<string> ids = new List<string>(), keys = new List<string>();
            bool keyFound = false;
            foreach (var prop in obj.GetType().GetProperties())
            {
                names += (names.Length > 0 ? ", " : "") + $"`{prop.Name}`";
                values += (values.Length > 0 ? ", " : "") + $"'{prop.GetValue(obj, null)}'";

                object attributes = prop.GetCustomAttributes(typeof(KeyAttribute), true).FirstOrDefault();
                if (prop.Name.ToLower() == "id" || attributes != null)
                {
                    keyFound = true;
                    //ids.Add(prop.Name);
                    //keys.Add(prop.GetValue(obj, null).ToString());
                    updateWhereValues += (updateWhereValues.Length > 0 ? " AND " : "") + $"`{prop.Name}` = '{prop.GetValue(obj, null)}'";
                }
                else
                {
                    // Don't add keys to update string
                    updateValues += (updateValues.Length > 0 ? ", " : "") + $"`{prop.Name}` = '{prop.GetValue(obj, null)}'";
                }
            }

            string tableName = obj.GetType().Name;
            if (IdExists(obj))
            {
                Update(tableName, updateValues, updateWhereValues);
            }
            else
            {
                Insert(tableName, names, values);
            }
        }

        public void Update(string tablename, string columnValues, string where)
        {
            try
            {
                using (SQLiteCommand command = m_dbConnection.CreateCommand())
                {
                    m_dbConnection.Open();
                    command.CommandText = $"UPDATE {tablename} SET {columnValues} WHERE {where}";
                    command.ExecuteNonQuery();
                }
            }
            catch (SQLiteException sqlex)
            {
                Console.WriteLine($"{sqlex}");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex}");

            }
            finally
            {
                m_dbConnection.Close();
            }

        }

        public bool IdExists(object obj)
        {
            string tableName = obj.GetType().Name;

            string whereKeyValues = "";
            foreach (var prop in obj.GetType().GetProperties())
            {
                object attributes = prop.GetCustomAttributes(typeof(KeyAttribute), true).FirstOrDefault();
                if (prop.Name.ToLower() == "id" || attributes != null)
                {
                    whereKeyValues += (whereKeyValues.Length > 0 ? " AND " : "") + $"`{prop.Name}` = '{prop.GetValue(obj, null)}'";
                }
            }

            try
                {
                using (SQLiteCommand command = m_dbConnection.CreateCommand())
                {
                    m_dbConnection.Open();
                    command.CommandText = $"SELECT * FROM {tableName} WHERE {whereKeyValues}";
                    bool idExists = command.ExecuteScalar() != null;
                    return idExists;
                }
            }
            catch (SQLiteException sqlex)
            {
                MessageBox.Show($"{sqlex}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex}");
            }
            finally
            {
                m_dbConnection.Close();
            }

            return true;
        }

        public void ClearTable<T>() where T : new()
        {
            ClearTable(new T());
        }

        public void ClearTable(object model)
        {
            if (model == null)
                return;

            try
            {
                using (SQLiteCommand command = m_dbConnection.CreateCommand())
                {
                    string tableName = model.GetType().Name;

                    m_dbConnection.Open();
                    command.CommandText = $"DELETE FROM '{tableName}'";
                    command.ExecuteNonQuery();
                }
            }
            catch (SQLiteException sqlex)
            {
                MessageBox.Show(sqlex.ToString(), "Sql exception");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Exception");
            }
            finally
            {
                m_dbConnection.Close();
            }
        }

        public void Dispose()
        {
            if(m_dbConnection != null && m_dbConnection.State != System.Data.ConnectionState.Closed)
                m_dbConnection.Close();
        }
    }
}
