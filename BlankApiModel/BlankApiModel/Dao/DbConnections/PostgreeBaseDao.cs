using Microsoft.Extensions.Configuration;
using Npgsql;

namespace BlankApiModel.Dao.DbConnections
{
    /// <summary>
    /// Class used to make connections in database
    /// This use the <see cref="IConfiguration"/> to get the connectionString
    /// </summary>
    public class PostgreeBaseDao : BaseDao
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public PostgreeBaseDao(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DataAccessPostgreSqlProvider");
        }

        /// <summary>
        /// Validate the connection created
        /// </summary>
        /// <returns>Return a bool if the connection is valid</returns>
        public override bool IsValidConnection()
        {
            return Connection != null && (Connection is NpgsqlConnection);
        }

        /// <summary>
        /// Create a connection with <see cref="Npgsql"/>
        /// </summary>
        public override void SetConnection()
        {
            Connection = new NpgsqlConnection(_connectionString);
        }
    }
}