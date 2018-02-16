using ApiScanner.Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiScanner.DataAccess.Repositories
{
    public class WidgetRepository : IWidgetRepository
    {
        private readonly CoreContext _dbContext;

        public WidgetRepository(CoreContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task CreateAsync(WidgetModel widget)
        {
            await _dbContext.AddAsync(widget);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<WidgetModel>> GetWidgetsAsync(Guid userId, bool includeLocation)
        {
            var query = _dbContext.Widgets.AsNoTracking().AsQueryable();
            if (includeLocation)
                query = query.Include(e => e.Location);

            return await query
                .Where(e => e.UserId == userId)
                .OrderBy(e => e.Name)
                .ToListAsync();
        }

        public async Task<WidgetModel> GetWidgetAsync(Guid widgetId, bool includeLocation, bool includeApis)
        {
            var query = _dbContext.Widgets.AsNoTracking().AsQueryable();
            if (includeLocation)
                query = query.Include(e => e.Location);
            if (includeApis)
                query = query.Include(e => e.ApiWidgets);

            return await query
                .FirstOrDefaultAsync(e => e.WidgetId == widgetId);
        }

        public async Task DeleteWidgetAsync(Guid widgetId)
        {
            var widget = await _dbContext.Widgets
                .FirstOrDefaultAsync(e => e.WidgetId == widgetId);
            if (widget != null)
            {
                _dbContext.Remove(widget);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<bool> UpdateWidgetAsync(WidgetModel widget)
        {
            var myWidget = await _dbContext.Widgets
                .FirstOrDefaultAsync(e => e.WidgetId == widget.WidgetId);
            if (myWidget == null)
                return false;

            myWidget.LocationId = widget.LocationId;
            myWidget.Name = widget.Name;
            myWidget.PublicRead = widget.PublicRead;
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task CreateApiWidget(Guid apiId, Guid widgetId)
        {
            await _dbContext.ApiWidgets.AddAsync(new ApiWidgetModel
            {
                ApiId = apiId,
                WidgetId = widgetId
            });
            await _dbContext.SaveChangesAsync();
        }

        public async Task CreateApiWidget(ApiWidgetModel apiWidget)
        {
            await CreateApiWidget(apiWidget.ApiId, apiWidget.WidgetId);
        }

        public async Task DeleteApiWidget(ApiWidgetModel apiWidget)
        {
            _dbContext.Remove(apiWidget);
            await _dbContext.SaveChangesAsync();
        }
    }
}
