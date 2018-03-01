using System.ComponentModel.DataAnnotations;

namespace ApiScanner.Entities.Models
{
    public class ConfigurationModel
    {
        /// <summary>
        /// Name of config.
        /// </summary>
        [Key]
        [MaxLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// Value of config.
        /// </summary>
        [MaxLength(250)]
        public string Value { get; set; }
    }
}
