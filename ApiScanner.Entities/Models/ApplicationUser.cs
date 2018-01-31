using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ApiScanner.Entities.Models
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        /// <summary>
        /// Full name of user.
        /// </summary>
        [MaxLength(150)]
        public string FullName { get; set; }

        /// <summary>
        /// True if user has subscribed to newsletter.
        /// </summary>
        public bool Subscribe { get; set; }

        /// <summary>
        /// User defined apis.
        /// </summary>
        public ICollection<ApiModel> Apis { get; set; }
    }
}
