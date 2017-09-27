using ApiScanner.Entities.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiScanner.Business.Managers
{
    public interface ILocationManager
    {
        /// <summary>
        /// Get all locations.
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<LocationModel>> GetLocationsAsync();
    }
}
