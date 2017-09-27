using ApiScanner.Business.Managers;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ApiScanner.Web.Controllers
{
    [Route("api/[controller]")]
    public class LocationController : Controller
    {
        private readonly ILocationManager _locManager;

        public LocationController(ILocationManager locManager)
        {
            _locManager = locManager;
        }

        /// <summary>
        /// Get list of locations.
        /// </summary>
        /// <returns></returns>
        [HttpGet("")]
        public async Task<IActionResult> Get()
        {
            var results = await _locManager.GetLocationsAsync();
            return Ok(results);
        }
    }
}
