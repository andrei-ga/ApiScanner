using ApiScanner.Entities.Models;
using System.Threading.Tasks;

namespace ApiScanner.DataAccess.Repositories
{
    public class ApiRepository : IApiRepository
    {
        private readonly CoreContext _dbContext;

        public ApiRepository(CoreContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> CreateAsync(ApiModel api)
        {
            _dbContext.Add(api);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
