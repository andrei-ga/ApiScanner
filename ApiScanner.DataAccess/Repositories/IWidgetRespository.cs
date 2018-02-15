using ApiScanner.Entities.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiScanner.DataAccess.Repositories
{
    public interface IWidgetRepository
    {
        /// <summary>
        /// Create widget.
        /// </summary>
        /// <param name="widget">Widget model.</param>
        /// <returns></returns>
        Task CreateAsync(WidgetModel widget);

        /// <summary>
        /// Get list of widgets.
        /// </summary>
        /// <param name="userId">User id.</param>
        /// <param name="includeLocation">Include widget location if true.</param>
        /// <returns></returns>
        Task<IEnumerable<WidgetModel>> GetWidgetsAsync(Guid userId, bool includeLocation);
    }
}
