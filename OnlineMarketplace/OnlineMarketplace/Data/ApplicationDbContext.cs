using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OnlineMarketplace.Entities;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace OnlineMarketplace.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Category> Category { get; set; }

        public DbSet<Product> Product { get; set; }

        public DbSet<WishList> WishList { get; set; }

        public DbSet<OnlineMarketplace.Entities.File> File { get; set; }

        public DbSet<Sale> Sale { get; set; }

        public DbSet<Review> Review { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<ApplicationUser>().Ignore(c => c.AccessFailedCount)
                                             .Ignore(c => c.LockoutEnabled)
                                             .Ignore(c => c.LockoutEnd)
                                             .Ignore(c => c.TwoFactorEnabled)
                                             .Ignore(c => c.PhoneNumber)
                                             .Ignore(c => c.PhoneNumberConfirmed)
                                             .Ignore(c => c.ConcurrencyStamp)
                                             .Ignore(c => c.SecurityStamp)
                                             .Ignore(c => c.EmailConfirmed);

            builder.Entity<ApplicationUser>().Property(x => x.UserName).HasMaxLength(32);
            builder.Entity<ApplicationUser>().Property(x => x.NormalizedUserName).HasMaxLength(32);
            builder.Entity<ApplicationUser>().Property(x => x.Email).HasMaxLength(64);
            builder.Entity<ApplicationUser>().Property(x => x.NormalizedEmail).HasMaxLength(64);

            builder.Entity<IdentityRole>().Ignore(c => c.ConcurrencyStamp);

            builder.Entity<IdentityRole>().Property(x => x.Name).HasMaxLength(64);
            builder.Entity<IdentityRole>().Property(x => x.NormalizedName).HasMaxLength(64);

            builder.Entity<Product>().Property(x => x.NumberOfSales).HasDefaultValue(0);
            builder.Entity<Product>().Property(x => x.Is_Deleted).HasDefaultValue(false);
        }
    }
}