using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using ApiScanner.Entities.Models;
using ApiScanner.Business.Utility;
using ApiScanner.Entities.DTOs;
using ApiScanner.Entities;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ApiScanner.Web.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RegexUtilities _regexUtilities;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _regexUtilities = new RegexUtilities();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Register([FromBody] UserDTO user)
        {
            if (!_regexUtilities.IsValidEmail(user.Email))
                return BadRequest(Json(new string[1] { BadRequestType.InvalidEmail.ToString() }));

            var newUser = new ApplicationUser
            {
                UserName = user.Email,
                Email = user.Email,
                Subscribe = user.Subscribe ?? false
            };

            var userCreationResult = await _userManager.CreateAsync(newUser, user.Password);
            if (!userCreationResult.Succeeded)
                return BadRequest(Json(userCreationResult.Errors.Select(e => e.Code)));

            return Ok(true);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Login([FromBody] UserDTO user)
        {
            var dbUser = await _userManager.FindByEmailAsync(user.Email);
            if (dbUser == null)
                return BadRequest(Json(BadRequestType.UserOrPassIncorrect.ToString()));
            
            if (!dbUser.EmailConfirmed && _userManager.Options.SignIn.RequireConfirmedEmail)
                return BadRequest(Json(BadRequestType.EmailNotConfirmed.ToString()));

            var passwordSignInResult = await _signInManager.PasswordSignInAsync(dbUser, user.Password, isPersistent: user.RememberLogin ?? false, lockoutOnFailure: false);
            if (!passwordSignInResult.Succeeded)
                return BadRequest(Json(BadRequestType.UserOrPassIncorrect.ToString()));

            return Ok(new UserDTO()
            {
                Email = dbUser.Email,
                Subscribe = dbUser.Subscribe
            });
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
            var user = await _userManager.FindByNameAsync(name);

            return Ok(new UserDTO()
            {
                Email = user.Email,
                Subscribe = user.Subscribe
            });
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok(true);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> ResetPassword([FromBody] UserDTO user)
        {
            var dbUser = await _userManager.Users.FirstOrDefaultAsync(u => u.Id.Equals(user.Id));
            if (dbUser == null)
                return BadRequest(Json(new string[1] { BadRequestType.UserNotFound.ToString() }));

            if (user.Password != user.PasswordRepeat)
                return BadRequest(Json(new string[1] { BadRequestType.PasswordMissmatch.ToString() }));

            var resetPasswordResult = await _userManager.ResetPasswordAsync(dbUser, user.ResetToken, user.Password);
            if (!resetPasswordResult.Succeeded)
                return BadRequest(Json(resetPasswordResult.Errors.Select(e => e.Code)));

            return Ok(true);
        }
    }
}
