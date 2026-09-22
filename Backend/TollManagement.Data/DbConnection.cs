using MySql.Data.MySqlClient;
using System;

namespace TollManagement.Data
{
    public class DbConnection
    {
        private readonly string connectionString;

        public DbConnection()
        {
            connectionString =
                System.Configuration.ConfigurationManager
                    .ConnectionStrings["TollManagementConnection"]
                    .ConnectionString;
        }
        public MySqlConnection GetConnection()
        {
            return new MySqlConnection(connectionString);
        }
        public bool TestConnection()
        {
            try
            {
                using (MySqlConnection connection = GetConnection())
                {
                    connection.Open();
                    return connection.State == System.Data.ConnectionState.Open;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}