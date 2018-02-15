using ApiScanner.Entities.Models;
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
    }
}
