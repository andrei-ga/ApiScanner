using ApiScanner.Entities.DTOs;
using ApiScanner.Entities.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiScanner.Business.Identity
{
    public interface IAccountService
    {
        /// <summary>
        /// Register a new user.
        /// </summary>
        /// <param name="user">User data.</param>
        /// <returns></returns>
        Task<(bool Success, IEnumerable<string> Errors)> Register(UserDTO user);

        /// <summary>
        /// Login a specific user.
        /// </summary>
        /// <param name="user">User data.</param>
        /// <returns></returns>
        Task<(bool Success, ApplicationUser User, string Error)> Login(UserDTO user);

        /// <summary>
        /// Logout current user.
        /// </summary>
        /// <returns></returns>
        Task Logout();

        /// <summary>
        /// Get account data for specific user. Returns null if not found.
        /// </summary>
        /// <param name="account">User account name.</param>
        /// <returns></returns>
        Task<ApplicationUser> AccountData(string account);

        /// <summary>
        /// Reset password for a user.
        /// </summary>
        /// <param name="user">User data.</param>
        /// <returns></returns>
        Task<(bool Success, IEnumerable<string> Errors)> ResetPassword(UserDTO user);
    }
}
