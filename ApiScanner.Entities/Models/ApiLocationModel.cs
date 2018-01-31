using System;

namespace ApiScanner.Entities.Models
{
    public class ApiLocationModel
    {
        /// <summary>
        /// Id of api.
        /// </summary>
        public Guid ApiId { get; set; }

        /// <summary>
        /// Id of location.
        /// </summary>
        public Guid LocationId { get; set; }

        /// <summary>
        /// Api.
        /// </summary>
        public ApiModel Api { get; set; }

        /// <summary>
        /// Location.
        /// </summary>
        public LocationModel Location { get; set; }
    }
}
