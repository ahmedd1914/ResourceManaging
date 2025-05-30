using System.Data;
using Microsoft.Data.SqlClient;

namespace ResourceManaging.Repository
{
    public class ConnectionFactory
    {
        private static string _connectionString;

        public static void SetConnectionString(string connectionString)
        {
            _connectionString = connectionString;
        }

        public static async Task<SqlConnection> CreateConnection()
        {
            var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            return connection;
        }
    }
}
