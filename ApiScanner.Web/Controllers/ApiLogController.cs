using ApiScanner.Business.Managers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace ApiScanner.Web.Controllers
{
    [Route("api/[controller]")]
    public class ApiLogController : Controller
    {
        private readonly IApiLogManager _apiLogManager;

        public ApiLogController(IApiLogManager apiLogManager)
        {
            _apiLogManager = apiLogManager;
        }

        /// <summary>
        /// Get api logs by specific api id.
        /// </summary>
        /// <param name="id">Api id</param>
        /// <param name="dateFrom">Date from when to get api logs. Will return all if null.</param>
        /// <param name="includeFails">Include api logs with fail status if true.</param>
        /// <returns></returns>
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetApiLogs(Guid id, DateTime? dateFrom, bool? includeFails)
        {
            var results = await _apiLogManager.GetApiLogsAsync(id, includeFails ?? false, dateFrom);
            if (results == null)
                return Forbid();
            return Ok(results);
        }

        /// <summary>
        /// Get api logs by specific widget id.
        /// </summary>
        /// <param name="widgetId">Widget id</param>
        /// <param name="dateFrom">Date from when to get api logs. Will return all if null.</param>
        /// <param name="includeFails">Include api logs with fail status if true.</param>
        /// <returns></returns>
        [HttpGet("widget/{widgetId:guid}")]
        public async Task<IActionResult> GetApiLogsByWidget(Guid widgetId, DateTime? dateFrom, bool? includeFails)
        {
            var results = await _apiLogManager.GetApiLogsByWidgetAsync(widgetId, includeFails ?? false, dateFrom);
            if (results == null)
                return Forbid();
            return Ok(results);
        }
    }
}
