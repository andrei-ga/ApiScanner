using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ApiScanner.Entities.Models
{
    public class WidgetModel
    {
        /// <summary>
        /// Id of widget.
        /// </summary>
        [Key]
        public Guid WidgetId { get; set; }

        /// <summary>
        /// Id of user who created the widget.
        /// </summary>
        [Required]
        public Guid UserId { get; set; }

        /// <summary>
        /// Id of location.
        /// </summary>
        [Required]
        public Guid LocationId { get; set; }

        /// <summary>
        /// Name of widget.
        /// </summary>
        [MaxLength(50)]
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Anyone can see if true.
        /// </summary>
        public bool PublicRead { get; set; }

        /// <summary>
        /// User who created the widget.
        /// </summary>
        public ApplicationUser User { get; set; }

        /// <summary>
        /// Location.
        /// </summary>
        public LocationModel Location { get; set; }

        /// <summary>
        /// Api widgets.
        /// </summary>
        public ICollection<ApiWidgetModel> ApiWidgets { get; set; }
    }
}
