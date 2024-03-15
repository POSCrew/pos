using Microsoft.EntityFrameworkCore;

namespace POS.Infrastructure;

public sealed class POSDBContext : DbContext
{
    public POSDBContext(DbContextOptions<POSDBContext> options)
        : base (options)
    {
    }
}