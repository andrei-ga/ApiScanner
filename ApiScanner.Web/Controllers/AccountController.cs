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

        [HttpPost("[action]")]
        public async Task<IActionResult> Register([FromBody] UserDTO user)
        {
            var result = await _accountService.Register(user);
            if (result.Success)
                return Ok(true);
            return BadRequest(Json(result.Errors));
        }

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

        [HttpGet("[action]")]
        public IActionResult LoggedIn()
        {
            return Ok(User.Identity.IsAuthenticated);
        }

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

        [HttpPost("[action]")]
        public async Task<IActionResult> Logout()
        {
            await _accountService.Logout();
            return Ok(true);
        }

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
