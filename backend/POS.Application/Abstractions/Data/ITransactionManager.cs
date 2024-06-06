namespace POS.Application.Abstractions.Data;

public interface IDBTransaction : IDisposable
{
    Task CommitAsync();
}

public interface ITransactionManager
{
    Task<IDBTransaction> BeginTransactionAsync();
}