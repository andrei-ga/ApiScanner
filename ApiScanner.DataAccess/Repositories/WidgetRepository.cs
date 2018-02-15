using ApiScanner.Entities.Models;
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
    }
}
