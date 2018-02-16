using ApiScanner.Business.Identity;
using ApiScanner.DataAccess.Repositories;
using ApiScanner.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<IEnumerable<WidgetModel>> GetWidgetsAsync(bool includeLocation)
        {
            var account = await _accountSvc.AccountData();
            if (account == null)
                return new List<WidgetModel>();
            var results = await _widgetRepo.GetWidgetsAsync(account.Id, includeLocation);
            foreach (var widget in results)
                widget.User = null;
            return results;
        }

        public async Task<WidgetModel> GetWidgetAsync(Guid widgetId, bool includeLocation, bool includeApis)
        {
            var account = await _accountSvc.AccountData();
            if (account == null)
                return null;
            var widget = await _widgetRepo.GetWidgetAsync(widgetId, includeLocation, includeApis);
            if (widget == null || (!widget.PublicRead && widget.UserId != account.Id))
                return null;
            widget.User = null;
            return widget;
        }

        public async Task<bool> CanSeeWidgetAsync(Guid widgetId)
        {
            var account = await _accountSvc.AccountData();
            var widget = await _widgetRepo.GetWidgetAsync(widgetId, false, false);
            if (widget != null && (widget.PublicRead || (account?.Id == widget.UserId)))
                return true;
            return false;
        }

        public async Task<bool> DeleteWidgetAsync(Guid widgetId)
        {
            var account = await _accountSvc.AccountData();
            if (account == null)
                return false;
            var api = await _widgetRepo.GetWidgetAsync(widgetId, false, false);
            if (api == null || (account.Id != api.UserId))
                return false;
            await _widgetRepo.DeleteWidgetAsync(widgetId);
            return true;
        }

        public async Task<bool> UpdateWidgetAsync(WidgetModel widget)
        {
            var account = await _accountSvc.AccountData();
            if (account == null)
                return false;
            var myWidget = await _widgetRepo.GetWidgetAsync(widget.WidgetId, false, true);
            if (myWidget == null || (account.Id != myWidget.UserId))
                return false;

            // delete removed apis
            foreach (var api in myWidget.ApiWidgets.ToList())
            {
                if (widget.ApiWidgets.Count(e => e.ApiId == api.ApiId) == 0)
                    await _widgetRepo.DeleteApiWidget(api);
            }
            // create or update the rest
            foreach (var api in widget.ApiWidgets)
            {
                if (myWidget.ApiWidgets.Count(e => e.ApiId == api.ApiId) == 0)
                    await _widgetRepo.CreateApiWidget(api);
            }

            await _widgetRepo.UpdateWidgetAsync(widget);
            return true;
        }
    }
}
