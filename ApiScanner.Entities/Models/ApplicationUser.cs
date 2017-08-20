using Microsoft.AspNetCore.Identity;
using System;

namespace ApiScanner.Entities.Models
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string FullName { get; set; }
        public bool Subscribe { get; set; }
    }
}
