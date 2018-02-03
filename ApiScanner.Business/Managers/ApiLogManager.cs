using ApiScanner.DataAccess.Repositories;
using ApiScanner.Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<IEnumerable<ApiLogDTO>> GetApiLogsAsync(Guid id, DateTime? dateFrom)
        {
            return (await _apiLogRepo.GetApiLogsAsync(id, dateFrom)).Select(e =>
                new ApiLogDTO
                {
                    LocationName = e.Location.Name,
                    Content = e.Content,
                    Headers = e.Headers,
                    LogDate = e.LogDate,
                    ResponseTime = e.ResponseTime,
                    StatusCode = e.StatusCode,
                    Success = e.Success
                });
        }
    }
}
