using ApiScanner.DataAccess.Repositories;
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
    }
}
