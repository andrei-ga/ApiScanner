using ApiScanner.Business.Managers;
using ApiScanner.Entities.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ApiScanner.Web.Controllers
{
    [Route("api/[controller]")]
    public class ApiController : Controller
    {
        private readonly IApiManager _apiManager;

        public ApiController(IApiManager apiManager)
        {
            _apiManager = apiManager;
        }

        /// <summary>
        /// Register a new account.
        /// </summary>
        /// <param name="user">User model.</param>
        /// <returns></returns>
        [HttpPost("")]
        public async Task<IActionResult> Create([FromBody] ApiModel api)
        {
            var result = await _apiManager.CreateAsync(api);
            return Ok(result);
        }

        /// <summary>
        /// Get list of apis created by current user.
        /// </summary>
        /// <returns></returns>
        [HttpGet("")]
        public async Task<IActionResult> Get()
        {
            var results = await _apiManager.GetApisAsync();
            return Ok(results);
        }
    }
}
