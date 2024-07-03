using POS.Core.Inventory;

namespace POS.Application.Abstractions.Data;

public interface IRepository<T>
    where T : class, new()
{
    void Add(T entity);
    void Update(T entity);
    void Remove(int id);
    Task SaveChangesAsync();
    void ChangeStateToUnchanged(T vendor);
    IQueryable<T> Set { get; }
}