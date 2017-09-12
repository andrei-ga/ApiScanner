using ApiScanner.Business.Identity;
using ApiScanner.DataAccess.Repositories;
using ApiScanner.Entities.Models;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
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
            if (account == null)
                return false;
            api.User = account;
            return await _apiRepo.CreateAsync(api);
        }

        public async Task<IEnumerable<ApiModel>> GetApisAsync()
        {
            var account = await _accountSvc.AccountData();
            if (account == null)
                return new List<ApiModel>();
            return await _apiRepo.GetApisAsync(account.Id);
        }
    }
}
