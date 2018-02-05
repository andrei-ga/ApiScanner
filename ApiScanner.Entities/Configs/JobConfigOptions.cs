using System;

namespace ApiScanner.Entities.Configs
{
    public class JobConfigOptions
    {
        /// <summary>
        /// Location id of job instance. Should be unique per instance.
        /// </summary>
        public Guid LocationId { get; set; }

        /// <summary>
        /// An api url to initialize the http client. Should be a fast endpoint.
        /// </summary>
        public string TestApi { get; set; }
    }
}
