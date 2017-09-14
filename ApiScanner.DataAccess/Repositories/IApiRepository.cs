using ApiScanner.Entities.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiScanner.DataAccess.Repositories
{
    public interface IApiRepository
    {
        /// <summary>
        /// Create api.
        /// </summary>
        /// <param name="api">Api model.</param>
        /// <returns></returns>
        Task<bool> CreateAsync(ApiModel api);

        /// <summary>
        /// Get list of apis created by specific user.
        /// </summary>
        /// <param name="userId">User id.</param>
        /// <param name="includeConditions">Include api conditions if true.</param>
        /// <param name="includeLocations">Include api locations if true.</param>
        /// <returns></returns>
        Task<IEnumerable<ApiModel>> GetApisAsync(Guid userId, bool includeConditions, bool includeLocations);

        /// <summary>
        /// Get api by id.
        /// </summary>
        /// <param name="apiId">Api id.</param>
        /// <param name="includeConditions">Include api conditions if true.</param>
        /// <param name="includeLocations">Include api locations if true.</param>
        /// <returns></returns>
        Task<ApiModel> GetApiAsync(Guid apiId, bool includeConditions, bool includeLocations);
    }
}
