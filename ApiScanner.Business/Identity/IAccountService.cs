using ApiScanner.Entities.DTOs;
using ApiScanner.Entities.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiScanner.Business.Identity
{
    public interface IAccountService
    {
        /// <summary>
        /// Register a windows user.
        /// </summary>
        /// <param name="user">User data.</param>
        /// <returns></returns>
        Task<(bool Success, IEnumerable<string> Errors)> RegisterWindows(UserDTO user);

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
        /// Return whether current user is logged in or not.
        /// </summary>
        /// <returns></returns>
        bool LoggedIn();

        /// <summary>
        /// Get account data for the logged in user.
        /// </summary>
        /// <returns></returns>
        Task<ApplicationUser> AccountData();

        /// <summary>
        /// Get account data for specific user. Returns null if not found.
        /// </summary>
        /// <param name="userName">User name.</param>
        /// <returns></returns>
        Task<ApplicationUser> AccountData(string userName);

        /// <summary>
        /// Reset password for a user.
        /// </summary>
        /// <param name="user">User data.</param>
        /// <returns></returns>
        Task<(bool Success, IEnumerable<string> Errors)> ResetPassword(UserDTO user);
    }
}
