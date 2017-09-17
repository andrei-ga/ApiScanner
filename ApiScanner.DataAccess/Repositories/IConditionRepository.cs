using ApiScanner.Entities.Models;
using System;
using System.Threading.Tasks;

namespace ApiScanner.DataAccess.Repositories
{
    public interface IConditionRepository
    {
        /// <summary>
        /// Create condition.
        /// </summary>
        /// <param name="cond">Condition model.</param>
        /// <returns></returns>
        Task CreateConditionAsync(ConditionModel cond);

        /// <summary>
        /// Update condition.
        /// </summary>
        /// <param name="cond">Condition model.</param>
        /// <returns></returns>
        Task<bool> UpdateConditionAsync(ConditionModel cond);

        /// <summary>
        /// Delete condition by id.
        /// </summary>
        /// <param name="condId">Condition id.</param>
        /// <returns></returns>
        Task DeleteAsync(Guid condId);
    }
}
