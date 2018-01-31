using ApiScanner.DataAccess.Repositories;
using ApiScanner.Entities.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiScanner.Business.Managers
{
    public class ApiLogManager : IApiLogManager
    {
        private readonly IApiLogRepository _apiLogRepo;

        public ApiLogManager(IApiLogRepository apiLogRepo)
        {
            _apiLogRepo = apiLogRepo;
        }

        public async Task<IEnumerable<ApiLogModel>> GetApiLogsAsync(Guid id, DateTime? dateFrom)
        {
            return await _apiLogRepo.GetApiLogsAsync(id, dateFrom);
        }
    }
}
