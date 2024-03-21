using POS.Application.Data;

namespace POS.Infrastructure;

public sealed class Repository<T> : IRepository<T>
    where T : class, new()
{
    private readonly POSDbContext _context;

    public Repository(POSDbContext context)
    {
        _context = context;
    }

    // TODO: don't make it public
    public IQueryable<T> Set
        => _context.Set<T>();
}
