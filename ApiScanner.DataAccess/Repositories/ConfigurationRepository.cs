using ApiScanner.Entities.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
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

        public async Task<IEnumerable<ConfigurationModel>> GetConfigsAsync()
        {
            return await _dbContext.Configurations
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task SaveConfigsAsync(IEnumerable<ConfigurationModel> configs)
        {
            _dbContext.Configurations.UpdateRange(configs);
            await _dbContext.SaveChangesAsync();
        }
    }
}
