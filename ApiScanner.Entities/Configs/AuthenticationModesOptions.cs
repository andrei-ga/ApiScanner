namespace ApiScanner.Entities.Configs
{
    public class AuthenticationModesOptions
    {
        /// <summary>
        /// Enable or not windows authentication.
        /// </summary>
        public bool Windows { get; set; }

        /// <summary>
        /// Enable or not basic authentication.
        /// </summary>
        public bool Basic { get; set; }
    }
}
