using System;
using System.ComponentModel.DataAnnotations;

namespace ApiScanner.Entities.Models
{
    public class ApiLogModel
    {
        /// <summary>
        /// Api log id.
        /// </summary>
        [Key]
        public Guid ApiLogId { get; set; }

        /// <summary>
        /// Api id.
        /// </summary>
        [Required]
        public Guid ApiId { get; set; }

        /// <summary>
        /// Status code of the response.
        /// </summary>
        [Required]
        public int StatusCode { get; set; }

        /// <summary>
        /// Response headers.
        /// </summary>
        public string Headers { get; set; }

        /// <summary>
        /// Response content (body).
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Response time in milliseconds.
        /// </summary>
        public long ResponseTime { get; set; }

        /// <summary>
        /// Date and time of the log in UTC.
        /// </summary>
        public DateTime LogDate { get; set; }

        /// <summary>
        /// True if conditions passed at logging moment. Else false.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Coresponding api.
        /// </summary>
        public virtual ApiModel Api { get; set; }
    }
}
