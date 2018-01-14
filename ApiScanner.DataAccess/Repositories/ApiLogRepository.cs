using ApiScanner.Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApiScanner.DataAccess.Repositories
{
    public class ApiLogRepository : IApiLogRepository
    {
        private readonly CoreContext _dbContext;

        public ApiLogRepository(CoreContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task LogAsync(ApiLogModel apiLog)
        {
            await _dbContext.AddAsync(apiLog);
            await _dbContext.SaveChangesAsync();
        }
    }
}
