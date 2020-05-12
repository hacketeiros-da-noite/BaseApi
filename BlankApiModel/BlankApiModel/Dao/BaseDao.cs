using Dapper.Contrib.Extensions;
using BlankApiModel.Extension;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
namespace BlankApiModel.Dao
{
    /// <summary>
    /// Class used to make connections in database
    /// This use the <see cref="IDbConnection"/> to connect with the database
    /// </summary>
    public abstract class BaseDao : IBaseDao
    {
        /// <summary>
        /// Property of connection instance
        /// </summary>
        protected IDbConnection Connection { get; set; }

        /// <summary>
        /// Create a Db Connection and set to <see cref="Connection"/> 
        /// </summary>
        public abstract void SetConnection();

        /// <summary>
        /// Validate the created connection
        /// </summary>
        /// <returns>Return a bool if the connection is valid</returns>
        public abstract bool IsValidConnection();

        /// <summary>
        /// Validate if the connection is valid
        /// </summary>
        /// <exception cref="DataException">Throws it when <see cref="IsValidConnection"> returns false</exception>
        protected virtual void ExceptionIfIsInvalidConnection()
        {
            if (!IsValidConnection())
                throw new DataException("Insert a valid connection");
        }

        /// <summary>
        /// Create a connection with database and validate it
        /// </summary>
        private void MakeConnection()
        {
            SetConnection();
            ExceptionIfIsInvalidConnection();
        }

        /// <summary>
        /// Get all items in database of type of <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T">Must by a <see cref="class"/></typeparam>
        /// <returns>Return a list of all get in database cast in type of <typeparamref name="T"/></returns>
        public async Task<IEnumerable<T>> GetAll<T>() where T : class
        {
            MakeConnection();
            return await Connection.GetAllAsync<T>();
        }


        /// <summary>
        /// Get one item in database of type of <typeparamref name="T"/> that have the <paramref name="id"/>
        /// </summary>
        /// <typeparam name="T">Must be a <see cref="class"/></typeparam>
        /// <returns>Return a object of founded item in database cast to <typeparamref name="T"/></returns>
        public async Task<T> Get<T>(int id) where T : class
        {
            MakeConnection();
            return await Connection.GetAsync<T>(id);
        }

        /// <summary>
        /// Insert one or any items in database
        /// Can insert a list of object of <typeparamref name="T"/>
        /// <typeparamref name="T"/>Must contains <see cref="System.ComponentModel.DataAnnotations"/>
        /// The key param must contains just <see cref="System.ComponentModel.DataAnnotations.KeyAttribute"/>
        /// Use <see cref="DapperExtension.InsertObject{T}(IDbConnection, T)"/>
        /// </summary>
        /// <typeparam name="T">Must by a <see cref="class"/></typeparam>
        /// <returns>Return the id or ids of inserted objects</returns>
        public async Task<object> Insert<T>(T obj) where T : class
        {
            MakeConnection();
            return await Connection.InsertObject(obj);
        }

        /// <summary>
        /// Update the item in database
        /// <typeparamref name="T"/>Must contains <see cref="System.ComponentModel.DataAnnotations"/>
        /// The key param must contains just <see cref="System.ComponentModel.DataAnnotations.KeyAttribute"/>
        /// Use <see cref="DapperExtension.UpdatObject{T}(IDbConnection, T)"/>
        /// </summary>
        /// <typeparam name="T">Must by a <see cref="class"/></typeparam>
        /// <returns>Return the id or ids of inserted objects</returns>
        public async Task<bool> Update<T>(T obj) where T : class
        {
            MakeConnection();
            return await Connection.UpdatObject(obj);
        }
    }
}
