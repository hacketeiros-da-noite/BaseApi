using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlankApiModel.Dao
{
    /// <summary>
    /// Interface to make database connections
    /// </summary>
    public interface IBaseDao
    {
        /// <summary>
        /// Get all items in database of type of <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T">Must by a <see cref="class"/></typeparam>
        /// <returns>Return a list of all get in database cast in type of <typeparamref name="T"/></returns>
        Task<IEnumerable<T>> GetAll<T>() where T : class;

        /// <summary>
        /// Get one item in database of type of <typeparamref name="T"/> that have the <paramref name="id"/>
        /// </summary>
        /// <typeparam name="T">Must by a <see cref="class"/></typeparam>
        /// <returns>Return a object of founded item in database cast to <typeparamref name="T"/></returns>
        Task<T> Get<T>(int id) where T : class;

        /// <summary>
        /// Insert one or any items in database
        /// Can insert a list of object of <typeparamref name="T"/>
        /// <typeparamref name="T"/> must contains <see cref="System.ComponentModel.DataAnnotations"/>
        /// The key param must contains just <see cref="System.ComponentModel.DataAnnotations.KeyAttribute"/>
        /// </summary>
        /// <typeparam name="T">Must by a <see cref="class"/></typeparam>
        /// <returns>Return the id or ids of inserted objects</returns>
        Task<object> Insert<T>(T obj) where T : class;

        /// <summary>
        /// Update the item in database
        /// <typeparamref name="T"/> must contains <see cref="System.ComponentModel.DataAnnotations"/>
        /// The key param must contains just <see cref="System.ComponentModel.DataAnnotations.KeyAttribute"/>
        /// </summary>
        /// <typeparam name="T">Must by a <see cref="class"/></typeparam>
        /// <returns>Return the id or ids of inserted objects</returns>
        Task<bool> Update<T>(T obj) where T : class;
    }
}
