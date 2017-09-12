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
        
        public async Task<IEnumerable<ApiModel>> GetApisAsync(Guid userId)
        {
            return await _dbContext.Apis
                .Where(e => e.UserId == userId)
                .ToListAsync();
        }
    }
}
