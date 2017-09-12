using ApiScanner.Entities.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiScanner.DataAccess.Repositories
{
    public interface IApiRepository
    {
        Task<bool> CreateAsync(ApiModel api);

        Task<IEnumerable<ApiModel>> GetApisAsync(Guid userId);
    }
}
