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

        public new DbSet<ApplicationUser> Users { get; set; }
        public DbSet<ApiModel> Apis { get; set; }
        public DbSet<ConditionModel> Conditions { get; set; }
        public DbSet<LocationModel> Locations { get; set; }
        public DbSet<ApiLocationModel> ApiLocations { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(ConnectionString, b => b.MigrationsAssembly("ApiScanner.DataAccess"));
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ApplicationUser>(Configure);
            builder.Entity<ApiModel>(Configure);
            builder.Entity<LocationModel>(Configure);
            builder.Entity<ApiLocationModel>(Configure);
            base.OnModelCreating(builder);
        }

        /// <summary>
        /// Configure users.
        /// </summary>
        /// <param name="entity"></param>
        private static void Configure(EntityTypeBuilder<ApplicationUser> entity)
        {
            entity.HasKey(e => e.Id);
            entity.HasMany<ApiModel>(e => e.Apis).WithOne(e => e.User);
        }

        /// <summary>
        /// Configure apis.
        /// </summary>
        /// <param name="entity"></param>
        private static void Configure(EntityTypeBuilder<ApiModel> entity)
        {
            entity.HasKey(e => e.ApiId);
            entity.HasOne<ApplicationUser>(e => e.User).WithMany(e => e.Apis);
            entity.HasMany<ConditionModel>(e => e.Conditions).WithOne(e => e.Api);
            entity.HasMany<ApiLocationModel>(e => e.ApiLocations).WithOne(e => e.Api);
        }

        /// <summary>
        /// Configure conditions.
        /// </summary>
        /// <param name="entity"></param>
        private static void Configure(EntityTypeBuilder<ConditionModel> entity)
        {
            entity.HasKey(e => e.ConditionId);
            entity.HasOne<ApiModel>(e => e.Api).WithMany(e => e.Conditions);
        }

        /// <summary>
        /// Configure locations.
        /// </summary>
        /// <param name="entity"></param>
        private static void Configure(EntityTypeBuilder<LocationModel> entity)
        {
            entity.HasKey(e => e.LocationId);
        }

        /// <summary>
        /// Configure api locations.
        /// </summary>
        /// <param name="entity"></param>
        private static void Configure(EntityTypeBuilder<ApiLocationModel> entity)
        {
            entity.HasKey(e => new { e.ApiId, e.LocationId });
            entity.HasOne<ApiModel>(e => e.Api).WithMany(e => e.ApiLocations);
            entity.HasOne<LocationModel>(e => e.Location).WithMany(e => e.ApiLocations);
        }
    }
}
