using ApiScanner.DataAccess.Repositories;
using ApiScanner.Entities.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiScanner.Business.Managers
{
    public class LocationManager : ILocationManager
    {
        private readonly ILocationRepository _locRepo;

        public LocationManager(ILocationRepository locRepo)
        {
            _locRepo = locRepo;
        }

        public async Task<IEnumerable<LocationModel>> GetLocationsAsync()
        {
            return await _locRepo.GetLocationsAsync();
        }
    }
}
