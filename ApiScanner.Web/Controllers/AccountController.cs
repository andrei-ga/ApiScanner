using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ApiScanner.Entities.DTOs;
using ApiScanner.Business.Identity;
using Microsoft.AspNetCore.Authorization;
using ApiScanner.Entities.Configs;
using Microsoft.Extensions.Options;
using ApiScanner.Entities.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ApiScanner.Web.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly AuthenticationModesOptions _authModesOptions;

        public AccountController(IAccountService accountservice, IOptions<AuthenticationModesOptions> authModesOptions)
        {
            _accountService = accountservice;
            _authModesOptions = authModesOptions.Value;
        }

        /// <summary>
        /// Register a new account.
        /// </summary>
        /// <param name="user">User model.</param>
        /// <returns></returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserDTO user)
        {
            var (success, errors) = await _accountService.Register(user);
            if (success)
                return Ok(true);
            return BadRequest(Json(errors));
        }

        /// <summary>
        /// Login the specified user.
        /// </summary>
        /// <param name="user">User model.</param>
        /// <returns></returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserDTO user)
        {
            var result = await _accountService.Login(user);
            if (result.Success)
                return Ok(new UserDTO
                {
                    UserName = result.User.UserName,
                    Email = result.User.Email,
                    Subscribe = result.User.Subscribe
                });
            return BadRequest(Json(result.Error));
        }

        /// <summary>
        /// Checks whether current user is logged in or not.
        /// </summary>
        /// <returns></returns>
        [HttpGet("logged")]
        public IActionResult LoggedIn()
        {
            return Ok(_accountService.LoggedIn());
        }

        /// <summary>
        /// Get account data of current user. Returns 204 if not logged in.
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet("data")]
        public async Task<IActionResult> AccountData()
        {
            bool windowsLogin = true;
            ApplicationUser user = null;
            if (_authModesOptions.Windows)
            {
                // if windows authentication is enabled
                var winUser = User.Identity;
                if (winUser != null)
                {
                    string accountName = winUser.Name?.Substring(winUser.Name.IndexOf('\\') + 1);
                    if (!string.IsNullOrWhiteSpace(accountName))
                    {
                        user = await _accountService.AccountData(accountName);
                        if (user == null)
                        {
                            // TODO: get email from LDAP when .net core 2.1 ships
                            var newUser = new UserDTO
                            {
                                Email = accountName + "@apiscanner.com",
                                UserName = accountName
                            };
                            await _accountService.RegisterWindows(newUser);
                            user = await _accountService.AccountData(accountName);
                        }
                    }
                }
            }

            if (user == null && _authModesOptions.Basic)
            {
                windowsLogin = false;
                user = await _accountService.AccountData();
            }

            if (user == null)
                return NoContent();

            return Ok(new UserDTO
            {
                UserName = user.UserName,
                Email = user.Email,
                Subscribe = user.Subscribe,
                Id = user.Id,
                WindowsLogin = windowsLogin
            });
        }

        /// <summary>
        /// Loggs out the current user.
        /// </summary>
        /// <returns></returns>
        [HttpPost("logout")]
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
        [HttpPost("reset")]
        public async Task<IActionResult> ResetPassword([FromBody] UserDTO user)
        {
            var (success, errors) = await _accountService.ResetPassword(user);
            if (success)
                return Ok(true);
            return BadRequest(Json(errors));
        }
    }
}
