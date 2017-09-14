using ApiScanner.Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiScanner.DataAccess.Repositories
{
    public class ApiRepository : IApiRepository
    {
        private readonly CoreContext _dbContext;

        public ApiRepository(CoreContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> CreateAsync(ApiModel api)
        {
            _dbContext.Add(api);
            await _dbContext.SaveChangesAsync();
            return true;
        }
        
        public async Task<IEnumerable<ApiModel>> GetApisAsync(Guid userId, bool includeConditions, bool includeLocations)
        {
            var query = _dbContext.Apis.AsQueryable();
            if (includeConditions)
                query = query.Include(e => e.Conditions);
            if (includeLocations)
                query = query.Include(e => e.ApiLocations);

            return await query
                .Where(e => e.UserId == userId)
                .ToListAsync();
        }

        public async Task<ApiModel> GetApiAsync(Guid apiId, bool includeConditions, bool includeLocations)
        {
            var query = _dbContext.Apis.AsQueryable();
            if (includeConditions)
                query = query.Include(e => e.Conditions);
            if (includeLocations)
                query = query.Include(e => e.ApiLocations);

            return await query
                .FirstOrDefaultAsync(e => e.ApiId == apiId);
        }
    }
}
