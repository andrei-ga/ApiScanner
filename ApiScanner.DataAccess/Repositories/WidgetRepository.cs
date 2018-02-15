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
    }
}
