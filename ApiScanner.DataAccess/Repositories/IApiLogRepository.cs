using ApiScanner.Entities.Models;
using System.Threading.Tasks;

namespace ApiScanner.DataAccess.Repositories
{
    public interface IApiLogRepository
    {
        /// <summary>
        /// Log an api call response.
        /// </summary>
        /// <param name="apiLog"></param>
        /// <returns></returns>
        Task LogAsync(ApiLogModel apiLog);
    }
}
