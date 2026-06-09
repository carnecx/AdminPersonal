using MySqlConnector;
using Dapper;

namespace AdminPersonal.Repository
{
    public static class ConnectionProvider
    {
        static ConnectionProvider()
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;
        }

        public static string ConnectionString { get; set; } = "";

        public static MySqlConnection GetConnection()
        {
            return new MySqlConnection(ConnectionString);
        }
    }
}
