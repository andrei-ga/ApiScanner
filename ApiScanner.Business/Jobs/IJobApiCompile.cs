using ApiScanner.Entities.Enums;
using System.Threading.Tasks;

namespace ApiScanner.Business.Jobs
{
    public interface IJobApiCompile
    {
        /// <summary>
        /// Executes job for sending http request to all api of current location.
        /// </summary>
        /// <param name="interval"></param>
        /// <returns></returns>
        Task ExecuteJobAsync(ApiInterval interval);
    }
}
