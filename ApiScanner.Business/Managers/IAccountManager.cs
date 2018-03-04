using System.Threading.Tasks;

namespace ApiScanner.Business.Managers
{
    public interface IAccountManager
    {
        /// <summary>
        /// Check if the specified account is an admin.
        /// </summary>
        /// <param name="accountName">Account name.</param>
        /// <returns></returns>
        Task<bool> IsAdmin(string accountName);
    }
}
