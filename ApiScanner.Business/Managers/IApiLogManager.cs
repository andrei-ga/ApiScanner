using ApiScanner.Entities.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiScanner.Business.Managers
{
    public interface IApiLogManager
    {
        /// <summary>
        /// Get api logs by specific api id.
        /// </summary>
        /// <param name="id">Api id.</param>
        /// <param name="dateFrom">Date from when to get api logs. Will return all if null.</param>
        /// <returns></returns>
        Task<IEnumerable<ApiLogModel>> GetApiLogsAsync(Guid id, DateTime? dateFrom);
    }
}
