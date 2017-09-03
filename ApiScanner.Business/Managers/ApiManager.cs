using ApiScanner.Business.Identity;
using ApiScanner.DataAccess.Repositories;
using ApiScanner.Entities.Models;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace ApiScanner.Business.Managers
{
    [Authorize]
    public class ApiManager : IApiManager
    {
        private readonly IApiRepository _apiRepo;
        private readonly IAccountService _accountSvc;

        public ApiManager(IApiRepository apiRepo, IAccountService accountSvc)
        {
            _apiRepo = apiRepo;
            _accountSvc = accountSvc;
        }
        
        public async Task<bool> CreateAsync(ApiModel api)
        {
            var account = await _accountSvc.AccountData();
            api.User = account;
            return await _apiRepo.CreateAsync(api);
        }
    }
}
