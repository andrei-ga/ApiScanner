using ApiScanner.Entities.Models;
using System.Threading.Tasks;

namespace ApiScanner.Business.Managers
{
    public interface IApiManager
    {
        /// <summary>
        /// Create new api.
        /// </summary>
        /// <param name="api">Api model.</param>
        /// <returns></returns>
        Task<bool> CreateAsync(ApiModel api);
    }
}
