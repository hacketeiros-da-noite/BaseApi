using Microsoft.Extensions.Configuration;
using Npgsql;

namespace BlankApiModel.Dao.DbConnections
{
    public class PostgreeBaseDao : BaseDao
    {
        protected readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public PostgreeBaseDao(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DataAccessPostgreSqlProvider");
        }

        public override bool IsValidConnection()
        {
            return Connection != null && (Connection is NpgsqlConnection);
        }

        public override void SetConnection()
        {
            Connection = new NpgsqlConnection(_connectionString);
        }
    }
}
