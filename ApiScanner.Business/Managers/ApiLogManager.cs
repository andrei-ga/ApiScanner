using ApiScanner.DataAccess.Repositories;
using ApiScanner.Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiScanner.Business.Managers
{
    public class ApiLogManager : IApiLogManager
    {
        private readonly IApiLogRepository _apiLogRepo;
        private readonly IApiManager _apiManager;
        private readonly IWidgetManager _widgetManager;

        public ApiLogManager(IApiLogRepository apiLogRepo, IApiManager apiManager, IWidgetManager widgetManager)
        {
            _apiLogRepo = apiLogRepo;
            _apiManager = apiManager;
            _widgetManager = widgetManager;
        }

        public async Task<IEnumerable<ApiLogDTO>> GetApiLogsAsync(Guid id, DateTime? dateFrom)
        {
            if (!await _apiManager.CanSeeApiAsync(id))
                return null;
            return await _apiLogRepo.GetApiLogsAsync(id, false, false, dateFrom);
        }

        public async Task<IEnumerable<ApiLogDTO>> GetApiLogsByWidgetAsync(Guid widgetId, DateTime? dateFrom)
        {
            if (!await _widgetManager.CanSeeWidgetAsync(widgetId))
                return null;
            return await _apiLogRepo.GetApiLogsByWidgetAsync(widgetId, false, false, dateFrom);
        }
    }
}
