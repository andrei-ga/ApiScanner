using ApiScanner.Entities.Models;
using System.Threading.Tasks;

namespace ApiScanner.DataAccess.Repositories
{
    public interface IApiRepository
    {
        Task<bool> CreateAsync(ApiModel api);
    }
}
