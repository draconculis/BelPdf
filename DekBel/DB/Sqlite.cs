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
        public string TableCitationName => "Citation";
        public string TableCategoryName => "Category";
        public string TableCitationCategoryName => "CitationCategory";
        public string TableFileName => "File";
        public string TableCitationFileName => "CitationFile";

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
            // Categories
            if (CreateTable(TableCategoryName, "Code TEXT PRIMARY KEY, Name TEXT NOT NULL, Description TEXT"))
            {
                CreateIndex(TableCategoryName, "Code");
                CreateIndex(TableCategoryName, "Name");
                Insert(TableCategoryName, $"{ColCode},{ColName},{ColDescription}", "'CATA','Category A','A first category.'");
                Insert(TableCategoryName, $"{ColCode},{ColName},{ColDescription}", "'CATB','Category B','A second category.'");
                Insert(TableCategoryName, $"{ColCode},{ColName},{ColDescription}", "'CATC','Category C','A third category.'");
                Insert(TableCategoryName, $"{ColCode},{ColName},{ColDescription}", "'CATC','Category C','A third category.'");
                Insert(TableCategoryName, $"{ColCode},{ColName},{ColDescription}", "'CATD','Category D','A fourth category.'");
            }

            // Files
            if (CreateTable(TableFileName, 
                "Hash TEXT PRIMARY KEY, " +
                "SrcPath TEXT NOT NULL, " +
                "StoName TEXT NOT NULL"
                ))
            {
                CreateIndex(TableFileName, "SrcPath");
                CreateIndex(TableFileName, "Hash");
                CreateIndex(TableFileName, "StorageName");
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


        public bool CreateTable(string tablename, string columnDesc)
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

                    // acount table not exist, create table and insert 
                    command.CommandText = $"CREATE TABLE {tablename} ({columnDesc})";
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
