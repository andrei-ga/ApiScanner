using ApiScanner.Entities.DTOs;
using ApiScanner.Entities.Models;
using System;
using System.Collections.Generic;
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

        /// <summary>
        /// Get api logs by specific api id.
        /// </summary>
        /// <param name="id">Api id.</param>
        /// <param name="includeContent">Include content property if true.</param>
        /// <param name="includeHeaders">Include headers property if true.</param>
        /// <param name="includeFails">Include api logs with fail status if true.</param>
        /// <param name="dateFrom">Date from when to get api logs. Will return all if null.</param>
        /// <returns></returns>
        Task<IEnumerable<ApiLogDTO>> GetApiLogsAsync(Guid id, bool includeContent, bool includeHeaders, bool includeFails, DateTime? dateFrom);

        /// <summary>
        /// Get api logs by specific widget id.
        /// </summary>
        /// <param name="widgetId">Widget id.</param>
        /// <param name="includeContent">Include content property if true.</param>
        /// <param name="includeHeaders">Include headers property if true.</param>
        /// <param name="includeFails">Include api logs with fail status if true.</param>
        /// <param name="dateFrom">Date from when to get api logs. Will return all if null.</param>
        /// <returns></returns>
        Task<IEnumerable<ApiLogDTO>> GetApiLogsByWidgetAsync(Guid widgetId, bool includeContent, bool includeHeaders, bool includeFails, DateTime? dateFrom);
    }
}
