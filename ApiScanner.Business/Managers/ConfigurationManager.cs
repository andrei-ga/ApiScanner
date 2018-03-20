using ApiScanner.DataAccess.Repositories;
using ApiScanner.Entities.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiScanner.Business.Managers
{
    public class ConfigurationManager : IConfigurationManager
    {
        private readonly IConfigurationRepository _configRepo;

        public ConfigurationManager(IConfigurationRepository configRepo)
        {
            _configRepo = configRepo;
        }

        public async Task<string> GetConfigValueAsync(string name)
        {
            return await _configRepo.GetConfigValueAsync(name);
        }

        public async Task<IEnumerable<ConfigurationModel>> GetConfigsAsync()
        {
            return await _configRepo.GetConfigsAsync();
        }

        public async Task SaveConfigsAsync(IEnumerable<ConfigurationModel> configs)
        {
            await _configRepo.SaveConfigsAsync(configs);
        }
    }
}
