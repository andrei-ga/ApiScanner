using ApiScanner.Business.Managers;
using ApiScanner.Entities.Models;
using ApiScanner.Web.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiScanner.Web.Controllers
{
    [Authorize]
    [IsAdmin]
    [Route("api/[controller]")]    
    public class ConfigurationController : Controller
    {
        private readonly IConfigurationManager _configManager;
        private readonly IAccountManager _accountManager;

        public ConfigurationController(IAccountManager accountManager, IConfigurationManager configManager)
        {
            _accountManager = accountManager;
            _configManager = configManager;
        }

        /// <summary>
        /// Get list of configuration keys and values.
        /// </summary>
        /// <returns></returns>
        [HttpGet("")]        
        public async Task<IActionResult> Get()
        {
            var results = await _configManager.GetConfigsAsync();
            return Ok(results);
        }

        /// <summary>
        /// Update configuration values.
        /// </summary>
        /// <returns></returns>
        [HttpPut("")]
        public async Task<IActionResult> Update([FromBody] IEnumerable<ConfigurationModel> configs)
        {
            await _configManager.SaveConfigsAsync(configs);
            return Ok(true);
        }
    }
}
