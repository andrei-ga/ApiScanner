using ApiScanner.Entities.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiScanner.Business.Managers
{
    public interface IWidgetManager
    {
        /// <summary>
        /// Create widget. Returns false if not logged in.
        /// </summary>
        /// <param name="widget">Widget model.</param>
        /// <returns></returns>
        Task<bool> CreateAsync(WidgetModel widget);

        /// <summary>
        /// Get widgets.
        /// </summary>
        /// <param name="includeLocation">Include widget location if true.</param>
        /// <returns></returns>
        Task<IEnumerable<WidgetModel>> GetWidgetsAsync(bool includeLocation);
    }
}
