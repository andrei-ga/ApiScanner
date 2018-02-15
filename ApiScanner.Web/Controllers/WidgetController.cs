using ApiScanner.Business.Managers;
using ApiScanner.Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ApiScanner.Web.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class WidgetController : Controller
    {
        private readonly IWidgetManager _widgetManager;

        public WidgetController(IWidgetManager widgetManager)
        {
            _widgetManager = widgetManager;
        }

        /// <summary>
        /// Create new api.
        /// </summary>
        /// <param name="api">Api model.</param>
        /// <returns></returns>
        [HttpPost("")]
        public async Task<IActionResult> Create([FromBody] WidgetModel widget)
        {
            var result = await _widgetManager.CreateAsync(widget);
            return Ok(result);
        }
    }
}
