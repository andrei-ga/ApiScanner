using ApiScanner.Business.Managers;
using ApiScanner.Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
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

        /// <summary>
        /// Get list of widgets created by current user.
        /// </summary>
        /// <returns></returns>
        [HttpGet("")]
        public async Task<IActionResult> Get()
        {
            var results = await _widgetManager.GetWidgetsAsync(true);
            return Ok(results);
        }

        /// <summary>
        /// Get widget by id.
        /// </summary>
        /// <param name="id">Widget id.</param>
        /// <returns></returns>
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetWidget(Guid id)
        {
            var result = await _widgetManager.GetWidgetAsync(id, true, true);
            if (result == null)
                return NoContent();
            return Ok(result);
        }

        /// <summary>
        /// Check if current user can see specific widget.
        /// </summary>
        /// <param name="id">Widget id.</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("{id:guid}/access")]
        public async Task<IActionResult> CanSeeWidget(Guid id)
        {
            var result = await _widgetManager.CanSeeWidgetAsync(id);
            return Ok(result);
        }

        /// <summary>
        /// Update widget by id.
        /// </summary>
        /// <param name="api">Widget id.</param>
        /// <returns></returns>
        [HttpPut("")]
        public async Task<IActionResult> UpdateApi([FromBody] WidgetModel widget)
        {
            var result = await _widgetManager.UpdateWidgetAsync(widget);
            if (!result)
                return Forbid();
            return NoContent();
        }

        /// <summary>
        /// Delete widget by id.
        /// </summary>
        /// <param name="id">Widget id.</param>
        /// <returns></returns>
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteWidget(Guid id)
        {
            var result = await _widgetManager.DeleteWidgetAsync(id);
            if (!result)
                return Forbid();
            return NoContent();
        }
    }
}
