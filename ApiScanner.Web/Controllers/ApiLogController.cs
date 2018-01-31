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
        /// <returns></returns>
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetApiLogs(Guid id, DateTime? dateFrom)
        {
            return Ok(await _apiLogManager.GetApiLogsAsync(id, dateFrom));
        }
    }
}
