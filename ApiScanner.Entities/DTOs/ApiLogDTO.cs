using System;

namespace ApiScanner.Entities.DTOs
{
    public class ApiLogDTO
    {
        /// <summary>
        /// Location name.
        /// </summary>
        public string LocationName { get; set; }

        /// <summary>
        /// Api name.
        /// </summary>
        public string ApiName { get; set; }

        /// <summary>
        /// Status code of the response.
        /// </summary>
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
    }
}
