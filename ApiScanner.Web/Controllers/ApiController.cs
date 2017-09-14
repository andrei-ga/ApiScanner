using ApiScanner.Business.Managers;
using ApiScanner.Entities.Models;
using Microsoft.AspNetCore.Mvc;
using System;
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
            var results = await _apiManager.GetApisAsync(false, false);
            return Ok(results);
        }

        /// <summary>
        /// Get api by id.
        /// </summary>
        /// <param name="id">Api id.</param>
        /// <returns></returns>
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetApi(Guid id)
        {
            var result = await _apiManager.GetApiAsync(id, true, true);
            if (result == null)
                return NoContent();
            return Ok(result);
        }

        /// <summary>
        /// Check if current user can see specific api.
        /// </summary>
        /// <param name="id">Api id.</param>
        /// <returns></returns>
        [HttpGet("{id:guid}/access")]
        public async Task<IActionResult> CanSeeApi(Guid id)
        {
            var result = await _apiManager.CanSeeApi(id);
            return Ok(result);
        }
    }
}
