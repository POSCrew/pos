using POS.Application.Abstractions.Data;
using POS.Core;

namespace POS.Infrastructure;

public sealed class Repository<T> : IRepository<T>
    where T : class, new()
{
    private readonly POSDbContext _context;

    public Repository(POSDbContext context)
    {
        _context = context;
    }

    public IQueryable<T> Set
        => _context.Set<T>();

    public void Add(T entity)
    {
        _context.Add(entity);
    }

    public void Remove(int id)
    {
        if (!typeof(T).IsAssignableTo(typeof(BaseEntity)))
            throw new NotImplementedException();

        BaseEntity entity = (BaseEntity)(object)new T();
        entity.ID = id;
        _context.Remove(entity);
    }

    public void Update(T entity)
    {
        _context.Update(entity);
    }

    public Task SaveChangesAsync()
    {
        return _context.SaveChangesAsync();
    }
}