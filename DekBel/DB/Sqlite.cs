using Dek.Bel.Cls;
using Dek.Bel.Models;
using Dek.Bel.UserSettings;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
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
        public Sqlite(IUserSettingsService userSettingsService)
        {
            m_UserSettingsService = userSettingsService;
            Init();
        }

        public void Init()
        {
            if(!File.Exists(m_UserSettingsService.DBPath))
                SQLiteConnection.CreateFile(m_UserSettingsService.DBPath);

            m_dbConnection = new SQLiteConnection($"Data Source={m_UserSettingsService.DBPath};Version=3;New=False;");
            CreateTables();
        }

        public void CreateTables()
        {
            // Book
            // CREATE TABLE "Book" ( `Id` TEXT NOT NULL, `Title` TEXT NOT NULL, `Author` TEXT, `PublishDate` TEXT, `Edition` TEXT, `Editors` TEXT, `EditionPublishDate` TEXT, `ISBN` TEXT, `Comment` TEXT, PRIMARY KEY(`BookId`) )
            if (CreateTable(TableBookName,
                "`Id` TEXT, " +
                "`Title` TEXT NOT NULL, " +
                "`Author` TEXT, " +
                "`PublishDate` TEXT, " +
                "`Edition` TEXT, " +
                "`Editors` TEXT, " +
                "`EditionPublishDate` TEXT, " +
                "`ISBN` TEXT, " +
                "`Comment` TEXT",
                "`Id`"))
            {
                CreateIndex(TableBookName, "ISBN");
            }

            // Citation
            // CREATE TABLE `Citation` ( `Id` TEXT NOT NULL, `Citation1` TEXT NOT NULL, `Citation2` TEXT NOT NULL, `CreatedDate` TEXT NOT NULL, `EditedDate` TEXT NOT NULL, PRIMARY KEY(`CitationId`) )
            if (CreateTable(TableCitationName,
                "`Id` TEXT, " +
                "`Citation1` TEXT NOT NULL, " +
                "`Citation2` TEXT NOT NULL, " +
                "`CreatedDate` TEXT NOT NULL, " +
                "`EditedDate` TEXT NOT NULL",
                "`Id`"))
            {
                CreateIndex(TableCitationName, "Code");
            }

            //if (CreateTable(TableRawCitationName,
            //    "`Id` TEXT, " +
            //    "`Fragment` TEXT NOT NULL, " +
            //    "`PageStart` INT NOT NULL, " +
            //    "`PageStop` INT NOT NULL, " +
            //    "`GlyphStart` INT NOT NULL, " +
            //    "`GlyphStop` INT NOT NULL, " +
            //    "`Rectangles` TEXT NOT NULL, " +
            //    "`Date` TEXT NOT NULL ",
            //    "`Id`"))
            //{
            //    CreateIndex(TableCitationName, "Code");
            //}

            CreateTable(typeof(RawCitation));

            // Categories
            // 

            if (CreateTable(TableCategoryName,
            "`Id` TEXT, " +
            "`Code` TEXT, " +
            "`Name` TEXT NOT NULL, " +
            "`Description`  TEXT",
            "`Id`"))
            {
                CreateIndex(TableCategoryName, "Code");
                CreateIndex(TableCategoryName, "Name");
                Insert(TableCategoryName, $"{ColCode},{ColName},{ColDescription}", "'CATA','Category A','A first category.'");
                Insert(TableCategoryName, $"{ColCode},{ColName},{ColDescription}", "'CATB','Category B','A second category.'");
                Insert(TableCategoryName, $"{ColCode},{ColName},{ColDescription}", "'CATC','Category C','A third category.'");
                Insert(TableCategoryName, $"{ColCode},{ColName},{ColDescription}", "'CATC','Category C','A third category.'");
                Insert(TableCategoryName, $"{ColCode},{ColName},{ColDescription}", "'CATD','Category D','A fourth category.'");
            }

            // Storage
            //CREATE TABLE "Storage"( `Id` TEXT NOT NULL, `Hash` TEXT NOT NULL, `SourceFileName` TEXT NOT NULL, `SourceFilePath` TEXT NOT NULL, `StorageFileName` TEXT NOT NULL UNIQUE, `Author` TEXT, `Date` TEXT, `Comment` TEXT, PRIMARY KEY(`Id`))
            if (CreateTable(TableStorageName,
                "`Id` TEXT, " +
                "`Hash` TEXT NOT NULL, " +
                "`SourceFileName` TEXT NOT NULL, " + // "file.pdf"
                "`SourceFilePath` TEXT NOT NULL, " + // "c:\some\place\file.pdf"
                "`StorageFileName` TEXT NOT NULL UNIQUE, " +
                "`BookId` TEXT, "+
                "`Date` TEXT, `Comment` TEXT",
                "`Id`"))
            {
                CreateIndex(TableStorageName, "Hash");
                CreateIndex(TableStorageName, "SourceFileName");
                CreateIndex(TableStorageName, "StorageFileName");
            }

            // CitationStorage
            // CREATE TABLE `CitationStorage` ( `CitationId` TEXT NOT NULL, `StorageId` TEXT NOT NULL, PRIMARY KEY(`StorageId`) )
            if (CreateTable(TableCitationStorageName,
                "`CitationId` TEXT NOT NULL, " +
                "`StorageId` TEXT NOT NULL",
                "`CitationId`, `StorageId`"
                ))
            {
                CreateIndex(TableCitationStorageName, "CitationId");
                CreateIndex(TableCitationStorageName, "StorageId");
            }

            // CitationCategory

            if (CreateTable(TableCitationCategoryName,
                "`CitationId` TEXT NOT NULL, " +
                "`CategoryId` TEXT NOT NULL, "+
                "`Weight` INTEGER NOT NULL",
                "`CitationId`, `CategoryId`"
                ))
            {
                CreateIndex(TableCitationCategoryName, "CitationId");
                CreateIndex(TableCitationCategoryName, "CategoryId");
            }

            // CitationBook
            // CREATE TABLE `CitationBook` ( `BookId` TEXT NOT NULL, `CitationId` TEXT NOT NULL, PRIMARY KEY(`BookId`,`CitationId`) )
            if (CreateTable(TableCitationBookName,
                "`CitationId` TEXT NOT NULL, " +
                "`BookId` TEXT NOT NULL",
                "`CitationId`, `BookId`"
                ))
            {
                CreateIndex(TableCitationBookName, "CitationId");
                CreateIndex(TableCitationBookName, "BookId");
            }
        }


        public DataTable Select(string query)
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

        public List<T> Select<T>(string where) where T : new()
        {
            T obj = new T();
            List<T> result = new List<T>();
            string tableName = obj.GetType().Name;

            string sqlQuery = $"SELECT * FROM {tableName} WHERE {where}";
            DataTable dt = Select(sqlQuery);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                obj = new T();
                foreach (var prop in obj.GetType().GetProperties())
                {
                    string name = prop.Name;
                    object val = dt.Rows[i][name];
                    bool isNullable = Nullable.GetUnderlyingType(prop.GetType()) != null;
                    bool isEnum = prop.GetType().IsEnum;
                    Type type = prop.GetType();
                    string typeName = type.Name;
                    if (isEnum)
                        typeName = "enum";

                    switch (typeName)
                    {
                        case nameof(DateTime):
                            prop.SetValue(obj, ((string)val).ToSaneDateTime());
                            break;
                        case "enum":
                            prop.SetValue(obj, Enum.Parse(type, ((string)val)));
                            break;
                        default:
                            prop.SetValue(obj, val);
                            break;
                    }

                }
            }

            return result;
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
                        throw new Exception("Create: No ending ',', stupido!");
                    if(!primaryKey.StartsWith("`") || !primaryKey.EndsWith("`"))
                        throw new Exception("Create: No `backticks` in primary key desc, stupido!");

                    command.CommandText = $"CREATE TABLE `{tablename}` ({columnDesc}, PRIMARY KEY({primaryKey})) ";
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
                string typeName = prop.GetType().Name;
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
                    case "sbyte":
                    case "short":
                    case "int":
                    case "long":
                    case "byte":
                    case "ushort":
                    case "uint":
                    case "ulong":
                    case "bool":
                        sqlType = "INTEGER";
                        break;
                    case "float":
                    case "double":
                    case "decimal":
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
            if (keyFound)
            {
                Update(tableName, updateValues, updateWhereValues);
            }
            else
            {
                Insert(tableName, names, values);
            }
        }


        public void Update(string tablename, string where, string columnValues)
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

        public void Dispose()
        {
            if(m_dbConnection != null && m_dbConnection.State != System.Data.ConnectionState.Closed)
                m_dbConnection.Close();
        }
    }
}
