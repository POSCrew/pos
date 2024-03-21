namespace POS.Application.Data;

public interface IRepository<T>
    where T : class, new()
{
    IQueryable<T> Set { get; }
}