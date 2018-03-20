using ApiScanner.Entities.Constants;
using System;
using System.Threading.Tasks;

namespace ApiScanner.Business.Managers
{
    public class AccountManager : IAccountManager
    {
        private readonly IConfigurationManager _configManager;

        public AccountManager(IConfigurationManager configManager)
        {
            _configManager = configManager;
        }

        public async Task<bool> IsAdmin(string accountName)
        {
            if (string.IsNullOrWhiteSpace(accountName))
                return false;
            var admins = await _configManager.GetConfigValueAsync(ConfigurationConst.Admins);

            var adminsList = admins.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            foreach(string admin in adminsList)
            {
                if (accountName.Equals(admin.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    return true;
            }
            return false;
        }
    }
}
