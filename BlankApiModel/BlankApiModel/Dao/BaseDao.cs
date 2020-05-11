using Dapper.Contrib.Extensions;
using BlankApiModel.Extension;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
namespace BlankApiModel.Dao
{
    public abstract class BaseDao : IBaseDao
    {
        public abstract void SetConnection();

        protected IDbConnection Connection { get; set; }

        public abstract bool IsValidConnection();

        private void ExceptionIfIsInvalidConnection()
        {
            if (!IsValidConnection())
                throw new DataException("Insert a valid connection");
        }

        private void MakeConnection()
        {
            SetConnection();
            ExceptionIfIsInvalidConnection();
        }

        public async Task<IEnumerable<T>> GetAll<T>() where T : class
        {
            MakeConnection();

            return await Connection.GetAllAsync<T>();
        }

        public async Task<T> Get<T>(int id) where T : class
        {
            MakeConnection();

            return await Connection.GetAsync<T>(id);
        }

        public async Task<object> Insert<T>(T obj) where T : class
        {
            MakeConnection();

            return await Connection.InsertObject(obj);
        }

        public async Task<bool> Update<T>(T obj) where T : class
        {
            MakeConnection();

            return await Connection.UpdatObject(obj);
        }
    }
}
