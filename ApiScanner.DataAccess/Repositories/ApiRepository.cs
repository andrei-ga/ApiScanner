using ApiScanner.Entities.Enums;
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

        public async Task CreateAsync(ApiModel api)
        {
            await _dbContext.AddAsync(api);
            await _dbContext.SaveChangesAsync();
        }
        
        public async Task<IEnumerable<ApiModel>> GetApisAsync(Guid userId, bool includeConditions, bool includeLocations)
        {
            var query = _dbContext.Apis.AsNoTracking().AsQueryable();
            if (includeConditions)
                query = query.Include(e => e.Conditions);
            if (includeLocations)
                query = query.Include(e => e.ApiLocations);

            return await query
                .Where(e => e.UserId == userId)
                .OrderBy(e => e.Name)
                .ToListAsync();
        }

        public async Task<ApiModel> GetApiAsync(Guid apiId, bool includeConditions, bool includeLocations)
        {
            var query = _dbContext.Apis.AsNoTracking().AsQueryable();
            if (includeConditions)
                query = query.Include(e => e.Conditions);
            if (includeLocations)
                query = query.Include(e => e.ApiLocations);

            return await query
                .FirstOrDefaultAsync(e => e.ApiId == apiId);
        }

        public async Task<IEnumerable<ApiModel>> GetEnabledApisAsync(Guid locationId, ApiInterval interval)
        {
            return await _dbContext.Apis.AsNoTracking()
                .Include(e => e.Conditions)
                .Where(e => e.ApiLocations.Any(l => l.LocationId == locationId) && e.Enabled && e.Interval == interval)
                .ToListAsync();
        }

        public async Task DeleteApiAsync(Guid apiId)
        {
            var api = await _dbContext.Apis
                .FirstOrDefaultAsync(e => e.ApiId == apiId);
            if (api != null)
            {
                _dbContext.Remove(api);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<bool> UpdateApiAsync(ApiModel api)
        {
            var myApi = await _dbContext.Apis
                .FirstOrDefaultAsync(e => e.ApiId == api.ApiId);
            if (myApi == null)
                return false;

            myApi.Body = api.Body;
            myApi.Enabled = api.Enabled;
            myApi.Headers = api.Headers;
            myApi.Interval = api.Interval;
            myApi.Method = api.Method;
            myApi.Name = api.Name;
            myApi.PublicRead = api.PublicRead;
            myApi.PublicWrite = api.PublicWrite;
            myApi.Url = api.Url;
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
