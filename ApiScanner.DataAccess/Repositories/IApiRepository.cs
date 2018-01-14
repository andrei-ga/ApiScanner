using ApiScanner.Entities.Enums;
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
        Task CreateAsync(ApiModel api);

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

        /// <summary>
        /// Get enabled api list by location id.
        /// </summary>
        /// <param name="locationId">Location id.</param>
        /// <returns></returns>
        Task<IEnumerable<ApiModel>> GetEnabledApisAsync(Guid locationId, ApiInterval interval);

        /// <summary>
        /// Delete api by id.
        /// </summary>
        /// <param name="apiId">Api id.</param>
        /// <returns></returns>
        Task DeleteApiAsync(Guid apiId);

        /// <summary>
        /// Update api by id. Returns false if not found.
        /// </summary>
        /// <param name="api">Api id.</param>
        /// <returns></returns>
        Task<bool> UpdateApiAsync(ApiModel api);
    }
}
