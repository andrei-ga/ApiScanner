using ApiScanner.Entities.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace ApiScanner.DataAccess
{
    public class CoreContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public CoreContext(DbContextOptions<CoreContext> options) : base(options) { }

        public CoreContext() { }

        public static string ConnectionString { get; set; }

        public DbSet<ApiModel> Apis { get; set; }
        public new DbSet<ApplicationUser> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(ConnectionString, b => b.MigrationsAssembly("ApiScanner.DataAccess"));
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ApiModel>(Configure);
            builder.Entity<ApplicationUser>(Configure);
            base.OnModelCreating(builder);
        }

        /// <summary>
        /// Configure apis
        /// </summary>
        /// <param name="entity"></param>
        private static void Configure(EntityTypeBuilder<ApiModel> entity)
        {
            entity.HasKey(e => e.ApiId);
            entity.HasOne<ApplicationUser>(e => e.User).WithMany(e => e.Apis);
        }

        /// <summary>
        /// Configure users
        /// </summary>
        /// <param name="entity"></param>
        private static void Configure(EntityTypeBuilder<ApplicationUser> entity)
        {
            entity.HasKey(e => e.Id);
            entity.HasMany<ApiModel>(e => e.Apis).WithOne(e => e.User);
        }
    }
}
