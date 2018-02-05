using ApiScanner.Business.Jobs;
using ApiScanner.Entities.Enums;
using System.Threading.Tasks;

namespace ApiScanner.Jobs.Managers
{
    public class ApiRunJob
    {
        private readonly IJobApiCompile _apiJobCompile;

        public ApiRunJob(IJobApiCompile jobApiCompile)
        {
            _apiJobCompile = jobApiCompile;
        }

        public async Task ExecuteJobAsync(ApiInterval interval)
        {
            await _apiJobCompile.ExecuteJobAsync(interval);
        }
    }
}
