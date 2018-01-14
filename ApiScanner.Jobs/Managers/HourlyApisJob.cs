using ApiScanner.DataAccess.Repositories;
using ApiScanner.Entities.Enums;
using ApiScanner.Entities.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ApiScanner.Jobs.Managers
{
    public class HourlyApisJob
    {
        private readonly IApiRepository _apiRepo;
        private readonly IApiLogRepository _apiLogRepo;
        private readonly IConfiguration _config;

        private Guid locationId;
        private static HttpClient client = new HttpClient();

        public HourlyApisJob(IApiRepository apiRepo, IApiLogRepository apiLogRepo, IConfiguration configuration)
        {
            _apiRepo = apiRepo;
            _apiLogRepo = apiLogRepo;
            _config = configuration;
            locationId = _config.GetValue<Guid>("LocationId");
        }

        public async Task ExecuteJobAsync(ApiInterval interval)
        {
            var apis = await _apiRepo.GetEnabledApisAsync(locationId, interval);
            foreach(var api in apis)
            {
                await RunApiAsync(api);
            }
        }

        private async Task RunApiAsync(ApiModel api)
        {
            var response = await client.GetAsync(api.Url);
            ApiLogModel log = new ApiLogModel();
            log.ApiId = api.ApiId;
            log.StatusCode = (int)response.StatusCode;
            log.Headers = "";
            log.Content = "";
            log.Success = true;
            await _apiLogRepo.LogAsync(log);
        }
    }
}
