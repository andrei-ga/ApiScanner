using ApiScanner.Entities.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiScanner.Business.Managers
{
    public interface IApiManager
    {
        /// <summary>
        /// Create new api. Returns false if not logged in.
        /// </summary>
        /// <param name="api">Api model.</param>
        /// <returns></returns>
        Task<bool> CreateAsync(ApiModel api);

        /// <summary>
        /// Get list of apis created by current user. Returns empty list of not logged in.
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<ApiModel>> GetApisAsync();
    }
}
