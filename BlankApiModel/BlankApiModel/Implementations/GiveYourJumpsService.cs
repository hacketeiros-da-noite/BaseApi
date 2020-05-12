using BlankApiModel.Dao;
using BlankApiModel.Model.Entities;
using System;
using System.Threading.Tasks;

namespace BlankApiModel.Implementations
{
    /// <summary>
    /// Example of a service layer pattern implementation.
    /// </summary>
    public class GiveYourJumpsService
    {
        private readonly IBaseDao _connection;

        public GiveYourJumpsService(IBaseDao connection)
        {
            _connection = connection;
        }

        /// <summary>
        /// Get all database data by <see cref="GiveYourJumpsModel"/> entity.
        /// </summary>
        /// <returns></returns>
        public async Task<object> GetAllAsync()
        {
            var result = await _connection.GetAll<GiveYourJumpsModel>();
            return result;
        }

        /// <summary>
        /// Insert new item in database by <see cref="GiveYourJumpsModel"/> entity.
        /// </summary>
        /// <returns></returns>
        public async Task<object> InsertAsync(GiveYourJumpsModel giveYourJumps)
        {
            if (giveYourJumps == null)
                throw new ArgumentException("The parameter giveYourJumps can't be null");

            var insertedId = await _connection.Insert(giveYourJumps);
            return insertedId;
        }
    }

}
