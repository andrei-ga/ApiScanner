using ApiScanner.Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiScanner.DataAccess.Repositories
{
    public class ApiLogRepository : IApiLogRepository
    {
        private readonly CoreContext _dbContext;

        public ApiLogRepository(CoreContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task LogAsync(ApiLogModel apiLog)
        {
            await _dbContext.AddAsync(apiLog);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<ApiLogModel>> GetApiLogsAsync(Guid id, DateTime? dateFrom)
        {
            var query = _dbContext.ApiLogs.AsNoTracking()
                .Include(e => e.Location)
                .AsQueryable();
            if (dateFrom != null)
                query = query.Where(e => e.LogDate > dateFrom);

            return await query
                .Where(e => e.ApiId == id)
                .OrderBy(e => e.LogDate)
                .ToListAsync();
        }
    }
}
