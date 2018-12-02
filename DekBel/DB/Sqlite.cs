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

namespace Dek.Bel.DB
{
    [Export(typeof(IDBService))]
    public class Sqlite : IDBService, IDisposable
    {
        private readonly IUserSettingsService m_UserSettingsService;
        SQLiteConnection m_dbConnection;

        // Table names
        public string TableBookName => "Book";
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
                "`Id` TEXT NOT NULL, " +
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
                "`Id` TEXT NOT NULL, "+
                "`Citation1` TEXT NOT NULL, " +
                "`Citation2` TEXT NOT NULL, " +
                "`CreatedDate` TEXT NOT NULL, " +
                "`EditedDate` TEXT NOT NULL",
                "`Id`"))
            {
                CreateIndex(TableCitationName, "Code");
            }

            // Categories
            // 

            if (CreateTable(TableCategoryName,
            "`Id` TEXT NOT NULL, " +
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
                "`Id` TEXT NOT NULL, " +
                "`Hash` TEXT NOT NULL, " +
                "`SourceFileName` TEXT NOT NULL, " +
                "`SourceFilePath` TEXT NOT NULL, " +
                "`StorageFileName` TEXT NOT NULL UNIQUE, "+
                "`Author` TEXT, "+
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
                //well yeah

            }
            catch (Exception ex)
            {
                //well yeah

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
                //well yeah

            }
            catch (Exception ex)
            {
                //well yeah
            }
            finally
            {
                m_dbConnection.Close();
            }

            return dt.Rows.Count > 0;
        }


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
                    // acount table not exist, create table and insert 
                    if (columnDesc.EndsWith(","))
                        throw new Exception("Create: Ending ,, stupido!");

                    command.CommandText = $"CREATE TABLE `{tablename}` ({columnDesc}, PRIMARY KEY(`{primaryKey}`)) ";
                    command.ExecuteNonQuery();
                }
            }
            catch (SQLiteException sqlex)
            {
                //well yeah

            }
            catch (Exception ex)
            {
                //well yeah

            }
            finally
            {
                m_dbConnection.Close();
            }
            return true;

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
                //well yeah

            }
            catch (Exception ex)
            {
                //well yeah

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
                    command.ExecuteNonQuery();
                }
            }
            catch (SQLiteException sqlex)
            {
                //well yeah

            }
            catch (Exception ex)
            {
                //well yeah

            }
            finally
            {
                m_dbConnection.Close();
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
                //well yeah

            }
            catch (Exception ex)
            {
                //well yeah

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
