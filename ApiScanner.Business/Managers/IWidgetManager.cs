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

        /// <summary>
        /// Get widget by id.
        /// </summary>
        /// <param name="widgetId">Widget id.</param>
        /// <param name="includeLocation">Include widget location if true.</param>
        /// <param name="includeApis">Include apis if true.</param>
        /// <returns></returns>
        Task<WidgetModel> GetWidgetAsync(Guid widgetId, bool includeLocation, bool includeApis);

        /// <summary>
        /// Check if current user can see specific widget.
        /// </summary>
        /// <param name="widgetId">Widget id.</param>
        /// <returns></returns>
        Task<bool> CanSeeWidgetAsync(Guid widgetId);

        /// <summary>
        /// Delete widget.
        /// </summary>
        /// <param name="widgetId">Widget id.</param>
        /// <returns></returns>
        Task<bool> DeleteWidgetAsync(Guid widgetId);

        /// <summary>
        /// Update widget.
        /// </summary>
        /// <param name="widget">Widget model.</param>
        /// <returns></returns>
        Task<bool> UpdateWidgetAsync(WidgetModel widget);
    }
}
