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
    }
}
