using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ApiScanner.Entities.DTOs;
using ApiScanner.Business.Identity;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ApiScanner.Web.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountservice)
        {
            _accountService = accountservice;
        }

        /// <summary>
        /// Register a new account.
        /// </summary>
        /// <param name="user">User model.</param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public async Task<IActionResult> Register([FromBody] UserDTO user)
        {
            var result = await _accountService.Register(user);
            if (result.Success)
                return Ok(true);
            return BadRequest(Json(result.Errors));
        }

        /// <summary>
        /// Login the specified user.
        /// </summary>
        /// <param name="user">User model.</param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public async Task<IActionResult> Login([FromBody] UserDTO user)
        {
            var result = await _accountService.Login(user);
            if (result.Success)
                return Ok(new UserDTO()
                {
                    Email = result.User.Email,
                    Subscribe = result.User.Subscribe
                });
            return BadRequest(Json(result.Error));
        }

        /// <summary>
        /// Checks whether current user is logged in or not.
        /// </summary>
        /// <returns></returns>
        [HttpGet("[action]")]
        public IActionResult LoggedIn()
        {
            return Ok(User.Identity.IsAuthenticated);
        }

        /// <summary>
        /// Get account data of current user. Returns 204 if not logged in.
        /// </summary>
        /// <returns></returns>
        [HttpGet("[action]")]
        public async Task<IActionResult> AccountData()
        {
            if (!User.Identity.IsAuthenticated)
                return NoContent();

            var name = User.Identity.Name;
            var user = await _accountService.AccountData(name);
            if (user == null)
                return NoContent();

            return Ok(new UserDTO()
            {
                Email = user.Email,
                Subscribe = user.Subscribe
            });
        }

        /// <summary>
        /// Loggs out the current user.
        /// </summary>
        /// <returns></returns>
        [HttpPost("[action]")]
        public async Task<IActionResult> Logout()
        {
            await _accountService.Logout();
            return Ok(true);
        }

        /// <summary>
        /// Reset a user password.
        /// </summary>
        /// <param name="user">User model.</param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public async Task<IActionResult> ResetPassword([FromBody] UserDTO user)
        {
            var result = await _accountService.ResetPassword(user);
            if (result.Success)
                return Ok(true);
            return BadRequest(Json(result.Errors));
        }
    }
}
