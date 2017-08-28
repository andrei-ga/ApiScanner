using System;
using System.ComponentModel.DataAnnotations;

namespace ApiScanner.Entities.Models
{
    public class ApiModel
    {
        /// <summary>
        /// Id of api.
        /// </summary>
        [Key]
        public Guid ApiId { get; set; }

        /// <summary>
        /// Id of user who created the api.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Name of api.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Absolute url of api.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Http request method type.
        /// </summary>
        public HttpMethodType Method { get; set; }

        /// <summary>
        /// Headers of request.
        /// </summary>
        public string Headers { get; set; }

        /// <summary>
        /// Body of request.
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// Interval to scan the api.
        /// </summary>
        public ApiInterval Interval { get; set; }

        /// <summary>
        /// Only scan if true.
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// Anyone can see if true.
        /// </summary>
        public bool PublicRead { get; set; }

        /// <summary>
        /// Anyone can modify if true.
        /// </summary>
        public bool PublicWrite { get; set; }

        /// <summary>
        /// User who created the api.
        /// </summary>
        public virtual ApplicationUser User { get; set; }
    }
}
