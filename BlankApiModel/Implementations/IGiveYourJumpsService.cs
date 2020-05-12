using BlankApiModel.Model.Entities;
using System.Threading.Tasks;

namespace BlankApiModel.Implementations
{
    public interface IGiveYourJumpsService
    {
        /// <summary>
        /// Get all database data by <see cref="GiveYourJumpsModel"/> entity.
        /// </summary>
        Task<object> GetAllAsync();

        /// <summary>
        /// Insert new item in database by <see cref="GiveYourJumpsModel"/> entity.
        /// </summary>
        Task<object> InsertAsync(GiveYourJumpsModel giveYourJumps);
    }
}
