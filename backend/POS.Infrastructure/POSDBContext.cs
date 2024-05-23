using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace POS.Infrastructure;

public sealed class POSDbContext : IdentityDbContext<IdentityUser>
{
    public POSDbContext(DbContextOptions<POSDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<IdentityRole>().HasData(
            new IdentityRole("Admin") { NormalizedName = "ADMIN" },
            new IdentityRole("Seller") { NormalizedName = "SELLER" }
        );
    }
}