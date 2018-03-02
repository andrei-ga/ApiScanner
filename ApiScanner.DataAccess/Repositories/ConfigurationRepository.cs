using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace ApiScanner.DataAccess.Repositories
{
    public class ConfigurationRepository : IConfigurationRepository
    {
        private readonly CoreContext _dbContext;

        public ConfigurationRepository(CoreContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<string> GetConfigValueAsync(string name)
        {
            return await _dbContext.Configurations
                .AsNoTracking()
                .Where(e => e.Name == name)
                .Select(e => e.Value)
                .FirstOrDefaultAsync();
        }
    }
}
