using ApiScanner.Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiScanner.DataAccess.Repositories
{
    public class LocationRepository : ILocationRepository
    {
        private readonly CoreContext _dbContext;

        public LocationRepository(CoreContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<LocationModel>>GetLocationsAsync()
        {
            return await _dbContext.Locations
                .OrderBy(e => e.Name).ToListAsync();
        }

        public async Task CreateApiLocation(Guid apiId, Guid locId)
        {
            await _dbContext.ApiLocations.AddAsync(new ApiLocationModel
            {
                ApiId = apiId,
                LocationId = locId
            });
            await _dbContext.SaveChangesAsync();
        }

        public async Task CreateApiLocation(ApiLocationModel loc)
        {
            await CreateApiLocation(loc.ApiId, loc.LocationId);
        }

        public async Task DeleteApiLocation(ApiLocationModel loc)
        {
            _dbContext.Remove(loc);
            await _dbContext.SaveChangesAsync();
        }
    }
}
