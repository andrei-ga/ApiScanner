using ApiScanner.Entities.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiScanner.DataAccess.Repositories
{
    public interface IConfigurationRepository
    {
        /// <summary>
        /// Get config value by name.
        /// </summary>
        /// <param name="name">Config name.</param>
        /// <returns></returns>
        Task<string> GetConfigValueAsync(string name);

        /// <summary>
        /// Get list of configuration keys and values.
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<ConfigurationModel>> GetConfigsAsync();

        /// <summary>
        /// Update configuration values.
        /// </summary>
        /// <param name="configs">Configuration values.</param>
        /// <returns></returns>
        Task SaveConfigsAsync(IEnumerable<ConfigurationModel> configs);
    }
}
