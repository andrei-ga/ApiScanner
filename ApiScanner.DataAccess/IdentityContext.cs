using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ApiScanner.Entities.Models;

namespace ApiScanner.DataAccess
{
    public class IdentityContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public static string ConnectionString { get; set; }

        public IdentityContext(DbContextOptions<IdentityContext> options) : base(options) { }

        public IdentityContext() { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(ConnectionString, b => b.MigrationsAssembly("ApiScanner.DataAccess"));
        }
    }
}
