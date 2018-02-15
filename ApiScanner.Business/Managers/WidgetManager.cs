using ApiScanner.Business.Identity;
using ApiScanner.DataAccess.Repositories;
using ApiScanner.Entities.Models;
using System.Threading.Tasks;

namespace ApiScanner.Business.Managers
{
    public class WidgetManager : IWidgetManager
    {
        private readonly IAccountService _accountSvc;
        private readonly IWidgetRepository _widgetRepo;

        public WidgetManager(IAccountService accountSvc, IWidgetRepository widgetRepo)
        {
            _accountSvc = accountSvc;
            _widgetRepo = widgetRepo;
        }

        public async Task<bool> CreateAsync(WidgetModel widget)
        {
            var account = await _accountSvc.AccountData();
            if (account == null)
                return false;
            widget.User = account;
            await _widgetRepo.CreateAsync(widget);
            return true;
        }
    }
}
