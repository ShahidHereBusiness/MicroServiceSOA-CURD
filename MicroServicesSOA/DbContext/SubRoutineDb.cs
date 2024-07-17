using System.Data;
using System.Data.SQLite;
using System.Security.Cryptography;
using System.Text;

namespace MicroServicesSOA.DbContext
{
    internal static class SubRoutineDb
    {
        internal static bool LocalizeDb(string? connectionString, string? appName = "tblMineTable", string? appNameSuffix = "19700101")
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    // Perform database operations CREATE Table
                    using (SQLiteCommand command = new SQLiteCommand($"CREATE TABLE IF NOT EXISTS {appName}{appNameSuffix} (TuppleId INTEGER PRIMARY KEY,AppCode TEXT NOT NULL, AppLogs TEXT NOT NULL, TuppleReceived DATETIME DEFAULT CURRENT_TIMESTAMP);", connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                if (ex.Message.ToString().Length > 0)
                    return false;
            }
            return false;
        }
        internal static bool DiminishDb(string connectionString, string? appCode, string appLogs, string? appName = "tblMineTable", string appNameSuffix = "19700101")
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    // Perform database operations INSERT Table
                    string insertSql = $"INSERT INTO {appName}{appNameSuffix} (AppCode,AppLogs) VALUES (@code, @log);";
                    using (SQLiteCommand command = new SQLiteCommand(insertSql, connection))
                    {
                        command.Parameters.AddWithValue("@code", appCode);
                        command.Parameters.AddWithValue("@log", appLogs);
                        if (command.ExecuteNonQuery() > 0)
                            return true;
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.ToString().Length > 0)
                    return false;
            }
            return false;
        }
        internal static DataTable? InsightDb(string connectionString, string? appName = "tblMineTable", string appNameSuffix = "19700101")
        {
            DataTable AppInstance = new DataTable();
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    // Perform database operations SELECT Table
                    string selectSql = $"SELECT * FROM {appName}{appNameSuffix};";

                    using (SQLiteCommand command = new SQLiteCommand(selectSql, connection))
                    {
                        AppInstance = new DataTable();
                        AppInstance.Load(command.ExecuteReader());
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.ToString().Length > 0)
                    return AppInstance;
            }
            return AppInstance;
        }
        internal static string MD5Db(string? watchWord, bool securityStamp = false)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(watchWord ?? "");
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to a hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("x2"));
                }
                if (securityStamp)
                    return $"{DateTime.Now.ToString("MMssyyyyHHddmmfff")}{sb.ToString()}{new Random().Next()}";
                else
                    return sb.ToString();
            }
        }                       
    }
}