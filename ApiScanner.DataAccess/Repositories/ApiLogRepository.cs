using ApiScanner.Entities.DTOs;
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

        public async Task<IEnumerable<ApiLogDTO>> GetApiLogsAsync(Guid id, bool includeContent, bool includeHeaders, DateTime? dateFrom)
        {
            var query = _dbContext.ApiLogs.AsNoTracking()
                .Include(e => e.Location)
                .Include(e => e.Api)
                .AsQueryable();
            if (dateFrom != null)
                query = query.Where(e => e.LogDate > dateFrom);

            return await query
                .Where(e => e.ApiId == id && e.Success)
                .OrderBy(e => e.LogDate)
                .Select(e => new ApiLogDTO
                {
                    LocationName = e.Location.Name,
                    ApiName = e.Api.Name,
                    Content = includeContent ? e.Content : null,
                    Headers = includeHeaders ? e.Headers : null,
                    LogDate = e.LogDate,
                    ResponseTime = e.ResponseTime,
                    StatusCode = e.StatusCode,
                    Success = e.Success
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<ApiLogDTO>> GetApiLogsByWidgetAsync(Guid widgetId, bool includeContent, bool includeHeaders, DateTime? dateFrom)
        {
            var query = _dbContext.ApiLogs.AsNoTracking()
                .Include(e => e.Location)
                .Include(e => e.Api)
                .AsQueryable();
            if (dateFrom != null)
                query = query.Where(e => e.LogDate > dateFrom);

            var apiIds = await _dbContext.ApiWidgets.AsNoTracking()
                .Where(e => e.WidgetId == widgetId)
                .Select(e => e.ApiId)
                .ToListAsync();

            var locationId = await _dbContext.Widgets.AsNoTracking()
                .Where(e => e.WidgetId == widgetId)
                .Select(e => e.LocationId)
                .FirstOrDefaultAsync();
            if (locationId == null)
                return new List<ApiLogDTO>();

            return await query
                .Where(e => apiIds.Contains(e.ApiId) && e.LocationId == locationId && e.Success)
                .OrderBy(e => e.LogDate)
                .Select(e => new ApiLogDTO
                {
                    LocationName = e.Location.Name,
                    ApiName = e.Api.Name,
                    Content = includeContent ? e.Content : null,
                    Headers = includeHeaders ? e.Headers : null,
                    LogDate = e.LogDate,
                    ResponseTime = e.ResponseTime,
                    StatusCode = e.StatusCode,
                    Success = e.Success
                })
                .ToListAsync();
        }

    }
}
