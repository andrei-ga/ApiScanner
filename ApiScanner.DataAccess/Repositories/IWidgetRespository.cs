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

        /// <summary>
        /// Get widget by id.
        /// </summary>
        /// <param name="widgetId">Widget id.</param>
        /// <param name="includeLocation">Include widget location if true.</param>
        /// <param name="includeApis">Include apis if true.</param>
        /// <returns></returns>
        Task<WidgetModel> GetWidgetAsync(Guid widgetId, bool includeLocation, bool includeApis);

        /// <summary>
        /// Delete specific widget.
        /// </summary>
        /// <param name="widgetId">Widget id.</param>
        /// <returns></returns>
        Task DeleteWidgetAsync(Guid widgetId);

        /// <summary>
        /// Update specific widget.
        /// </summary>
        /// <param name="widget">Widget model.</param>
        /// <returns></returns>
        Task<bool> UpdateWidgetAsync(WidgetModel widget);

        /// <summary>
        /// Create api widget.
        /// </summary>
        /// <param name="apiId">Api id.</param>
        /// <param name="widgetId">Widget id.</param>
        /// <returns></returns>
        Task CreateApiWidget(Guid apiId, Guid widgetId);

        /// <summary>
        /// Create api widget.
        /// </summary>
        /// <param name="apiWidget">Api widget model.</param>
        /// <returns></returns>
        Task CreateApiWidget(ApiWidgetModel apiWidget);

        /// <summary>
        /// Delete api widget.
        /// </summary>
        /// <param name="apiWidget">Api widget model.</param>
        /// <returns></returns>
        Task DeleteApiWidget(ApiWidgetModel apiWidget);
    }
}
