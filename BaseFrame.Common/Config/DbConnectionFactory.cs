using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;

namespace BaseFrame.Common.Config
{
    public class DbConnectionFactory
    {
        private static readonly string connString;
        private static readonly string dbType;

        public static string DbConnString()
        {
            string conn = ConfigurationManager.ConnectionStrings["DBConnGuZhi"].ConnectionString;
            return conn;
        }

         static DbConnectionFactory()
        {
            var collection = ConfigurationManager.ConnectionStrings["DBConnGuZhi"];
            connString = collection.ConnectionString;
            dbType = collection.ProviderName.ToLower();
        }
        static IDbConnection CreateDbConnection()
        {
            IDbConnection conn = null;
            switch (dbType)
            {
                case "system.data.sqlclient":
                    conn = new SqlConnection(connString);
                    break;
                case "mysql":
                    //conn = new MySqlConnection(connString); ;
                    break;
                case "oracle":
                    // conn = new OracleConnection(connString);
                    break;
                case "db2":
                    conn = new OleDbConnection(connString);
                    break;
                default:
                    conn = new SqlConnection(connString);
                    break;

            }
            return conn;
        }
    }
}
