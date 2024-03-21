using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace POS.Infrastructure;

public sealed class POSDbContext : IdentityDbContext<IdentityUser>
{
    public POSDbContext(DbContextOptions<POSDbContext> options)
        : base (options)
    {
    }
}