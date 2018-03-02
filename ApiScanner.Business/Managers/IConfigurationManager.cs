using System.Threading.Tasks;

namespace ApiScanner.Business.Managers
{
    public interface IConfigurationManager
    {
        /// <summary>
        /// Get config value by name.
        /// </summary>
        /// <param name="name">Config name.</param>
        /// <returns></returns>
        Task<string> GetConfigValueAsync(string name);
    }
}
