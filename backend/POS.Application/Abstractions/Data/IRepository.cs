namespace POS.Application.Abstractions.Data;

public interface IRepository<T>
    where T : class, new()
{
    IQueryable<T> Set { get; }
    void Add(T entity);
    void Update(T entity);
    void Remove(int id);
    Task SaveChangesAsync();
    void ChangeStateToUnchanged(T vendor);
    Task<List<Tdto>> ExecuteRawSql<Tdto>(FormattableString sql);
}