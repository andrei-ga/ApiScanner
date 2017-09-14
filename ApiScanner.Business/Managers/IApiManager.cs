using ApiScanner.Entities.Models;
using System;
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
        Task<IEnumerable<ApiModel>> GetApisAsync(bool includeConditions, bool includeLocations);

        /// <summary>
        /// Get api by id.
        /// </summary>
        /// <param name="apiId">Api id.</param>
        /// <param name="includeConditions">Include api conditions if true.</param>
        /// <param name="includeLocations">Include api locations if true.</param>
        /// <returns></returns>
        Task<ApiModel> GetApiAsync(Guid apiId, bool includeConditions, bool includeLocations);

        /// <summary>
        /// Check if current user can see specific api.
        /// </summary>
        /// <param name="apiId">Api id.</param>
        /// <returns></returns>
        Task<bool> CanSeeApi(Guid apiId);
    }
}
