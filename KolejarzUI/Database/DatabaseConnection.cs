using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kolejarz
{
    public class DatabaseConnection
    {
        public static string databaseConnectionString = "";

        public static OdbcConnection mySQLConnection;

        protected OdbcCommand cmd = new OdbcCommand { Connection = mySQLConnection };

        private string PrepareConnectionString()
        {
            databaseConnectionString = "DRIVER={MySQL ODBC 9.0 Unicode Driver}; " + // lub DRIVER={MySQL ODBC 8.0 Unicode Driver
                 "SERVER=localhost; " +
                 "DATABASE=kolejarz; " +
                 "UID=root; " +
                 "PASSWORD=";
            Console.WriteLine(databaseConnectionString);
            return databaseConnectionString;
        }
        public void ConnectToDB()
        {
            if (mySQLConnection != null)
            {
                mySQLConnection.Close();
            }

            string connStr = PrepareConnectionString();

            mySQLConnection = new OdbcConnection(connStr);

            mySQLConnection.Open();
            Console.WriteLine("Connection Information:");
            Console.WriteLine("\tConnection String:" + mySQLConnection.ConnectionString);
            Console.WriteLine("\tConnection Timeout:" + mySQLConnection.ConnectionTimeout);
            Console.WriteLine("\tDatabase:" + mySQLConnection.Database);
            Console.WriteLine("\tDataSource:" + mySQLConnection.DataSource);
            Console.WriteLine("\tDriver:" + mySQLConnection.Driver);
            Console.WriteLine("\tServerVersion:" + mySQLConnection.ServerVersion);
        }
        public void CloseDBConnection()
        {
            if (mySQLConnection != null)
            {
                mySQLConnection.Close();
            }
        }
    }
}
