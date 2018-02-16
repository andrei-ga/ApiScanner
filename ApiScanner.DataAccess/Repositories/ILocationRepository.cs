using ApiScanner.Entities.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiScanner.DataAccess.Repositories
{
    public interface ILocationRepository
    {
        /// <summary>
        /// Get all locations.
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<LocationModel>> GetLocationsAsync();

        /// <summary>
        /// Map an api with a location.
        /// </summary>
        /// <param name="apiId">Api id.</param>
        /// <param name="locId">Location id.</param>
        /// <returns></returns>
        Task CreateApiLocation(Guid apiId, Guid locId);

        /// <summary>
        /// Map an api with a location.
        /// </summary>
        /// <param name="loc">Api location model.</param>
        /// <returns></returns>
        Task CreateApiLocation(ApiLocationModel loc);

        /// <summary>
        /// Remove a map between an api and a location.
        /// </summary>
        /// <param name="loc">Api location model.</param>
        /// <returns></returns>
        Task DeleteApiLocation(ApiLocationModel loc);
    }
}
