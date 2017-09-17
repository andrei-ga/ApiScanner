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
            _dbContext.Add(api);
            await _dbContext.SaveChangesAsync();
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
