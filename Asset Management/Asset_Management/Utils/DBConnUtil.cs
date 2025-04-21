using System;
using System.Data.SqlClient;
using System.Configuration;

namespace Asset_Management.Utils
{
    public static class DBConnUtil
    {
        public static SqlConnection GetConnection()
        {
            try
            {
                string connectionString = "Data Source=JARVIS-LAPTOP;Initial Catalog=AssetDB;Integrated Security=True";
                SqlConnection conn = new SqlConnection(connectionString);
                return conn;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while connecting to DB: " + ex.Message);
                return null;
            }
        }

    }
}
