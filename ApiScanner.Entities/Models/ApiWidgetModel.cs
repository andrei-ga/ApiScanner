using System;

namespace ApiScanner.Entities.Models
{
    public class ApiWidgetModel
    {
        /// <summary>
        /// Id of api.
        /// </summary>
        public Guid ApiId { get; set; }

        /// <summary>
        /// Id of widget.
        /// </summary>
        public Guid WidgetId { get; set; }

        /// <summary>
        /// Api.
        /// </summary>
        public ApiModel Api { get; set; }

        /// <summary>
        /// Widget.
        /// </summary>
        public WidgetModel Widget { get; set; }
    }
}
