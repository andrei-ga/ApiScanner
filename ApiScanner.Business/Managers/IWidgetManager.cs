using ApiScanner.Entities.Models;
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
    }
}
