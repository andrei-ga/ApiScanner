using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ApiScanner.Entities.Models
{
    public class LocationModel
    {
        /// <summary>
        /// Id of location.
        /// </summary>
        [Key]
        public Guid LocationId { get; set; }

        /// <summary>
        /// Name of location.
        /// </summary>
        [MaxLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// Apis of location.
        /// </summary>
        public ICollection<ApiLocationModel> ApiLocations { get; set; }
    }
}
