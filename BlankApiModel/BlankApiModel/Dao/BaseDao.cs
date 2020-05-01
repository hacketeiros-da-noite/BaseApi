using BlankApiModel.Extension;
using Dapper.Contrib.Extensions;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlankApiModel.Dao
{
    /// <summary>
    /// Class used to make connections in database
    /// This use the <see cref="IConfiguration"/> to get the connectionString
    /// Make connections using <see cref="Npgsql"/>
    /// </summary>
    public class BaseDao : IBaseDao
    {
        protected IConfiguration _configuration;
        private string connectionString;
        public BaseDao(IConfiguration configuration)
        {
            _configuration = configuration;
            connectionString = _configuration.GetConnectionString("DataAccessPostgreSqlProvider");
        }

        /// <summary>
        /// Get all items in database of type of <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T">Must by a <see cref="class"/></typeparam>
        /// <returns>Return a list of all get in database cast in type of <typeparamref name="T"/></returns>
        public async Task<IEnumerable<T>> GetAll<T>() where T : class
        {
            using (var conection = new NpgsqlConnection(connectionString))
            {
                return await conection.GetAllAsync<T>();
            }
        }

        /// <summary>
        /// Get one item in database of type of <typeparamref name="T"/> that have the <paramref name="id"/>
        /// </summary>
        /// <typeparam name="T">Must by a <see cref="class"/></typeparam>
        /// <returns>Return a object of founded item in database cast to <typeparamref name="T"/></returns>
        public async Task<T> Get<T>(int id) where T : class
        {
            using (var conection = new NpgsqlConnection(connectionString))
            {
                return await conection.GetAsync<T>(id);
            }
        }

        /// <summary>
        /// Insert one or any items in database
        /// Can insert a list of object of <typeparamref name="T"/>
        /// <typeparamref name="T"/> must contains <see cref="System.ComponentModel.DataAnnotations"/>
        /// The key param must contains just <see cref="System.ComponentModel.DataAnnotations.KeyAttribute"/>
        /// Use <see cref="DapperExtension.InsertObject{T}(NpgsqlConnection, T)"/>
        /// </summary>
        /// <typeparam name="T">Must by a <see cref="class"/></typeparam>
        /// <returns>Return the id or ids of inserted objects</returns>
        public async Task<object> Insert<T>(T obj) where T : class
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                return await connection.InsertObject<T>(obj);
            }
        }

        /// <summary>
        /// Update the item in database
        /// <typeparamref name="T"/> must contains <see cref="System.ComponentModel.DataAnnotations"/>
        /// The key param must contains just <see cref="System.ComponentModel.DataAnnotations.KeyAttribute"/>
        /// Use <see cref="DapperExtension.UpdatObject{T}(NpgsqlConnection, T)"/>
        /// </summary>
        /// <typeparam name="T">Must by a <see cref="class"/></typeparam>
        /// <returns>Return the id or ids of inserted objects</returns>
        public async Task<bool> Update<T>(T obj) where T : class
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                return await connection.UpdatObject(obj);
            }
        }
    }
}
