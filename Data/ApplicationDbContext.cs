// Data/ApplicationDbContext.cs
using HaneensCollection.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HaneensCollection.Data
{
    public class ApplicationDbContext : IdentityDbContext<Admin>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Add DbSet for HeroSection
        public DbSet<HeroSection> HeroSections { get; set; }

        public DbSet<UnstitchedSuit> UnstitchedSuits { get; set; }
        public DbSet<StitchedSuit> StitchedSuits { get; set; }

        public DbSet<InstagramPost> InstagramPosts { get; set; }

        public DbSet<FeaturedProduct> FeaturedProducts { get; set; }

        public DbSet<ProductImage> ProductImages { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configure HeroSection entity
            builder.Entity<HeroSection>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Subtitle).HasMaxLength(500);
                entity.Property(e => e.BannerImageBase64).IsRequired();
                entity.Property(e => e.ImageContentType).IsRequired().HasMaxLength(100);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");

                // Add index for frequently queried fields
                entity.HasIndex(e => e.CreatedAt);
            });
            builder.Entity<InstagramPost>(entity =>
            {
                entity.ToTable("InstagramPosts"); // Explicit table name
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ImageUrl).IsRequired();
                entity.Property(e => e.InstagramLink).IsRequired();
                entity.Property(e => e.ImageContentType).IsRequired();
            });

            // You can add other custom model configurations here
        }
    }
}