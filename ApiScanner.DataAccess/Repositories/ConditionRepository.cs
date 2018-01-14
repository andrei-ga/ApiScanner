using ApiScanner.Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace ApiScanner.DataAccess.Repositories
{
    public class ConditionRepository : IConditionRepository
    {
        private readonly CoreContext _dbContext;

        public ConditionRepository(CoreContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task CreateConditionAsync(ConditionModel cond)
        {
            await _dbContext.AddAsync(cond);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> UpdateConditionAsync(ConditionModel cond)
        {
            var myCond = await _dbContext.Conditions
                .FirstOrDefaultAsync(e => e.ConditionId == cond.ConditionId);
            if (myCond == null)
                return false;

            myCond.CompareType = cond.CompareType;
            myCond.CompareValue = cond.CompareValue;
            myCond.MatchType = cond.MatchType;
            myCond.MatchVar = cond.MatchVar;
            myCond.ShouldPass = cond.ShouldPass;
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task DeleteAsync(Guid condId)
        {
            var myCond = await _dbContext.Conditions
                .FirstOrDefaultAsync(e => e.ConditionId == condId);
            if (myCond != null)
            {
                _dbContext.Remove(myCond);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
