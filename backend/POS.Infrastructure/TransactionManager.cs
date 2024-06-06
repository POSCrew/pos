using Microsoft.EntityFrameworkCore.Storage;
using POS.Application.Abstractions.Data;

namespace POS.Infrastructure;

public sealed class TransactionManager : ITransactionManager
{
    private readonly POSDbContext _dbContext;

    public TransactionManager(POSDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IDBTransaction> BeginTransactionAsync()
    {
        return new DBTransaction(await _dbContext.Database.BeginTransactionAsync());
    }
}

public sealed class DBTransaction : IDBTransaction
{
    private readonly IDbContextTransaction _transaction;

    public DBTransaction(IDbContextTransaction transaction)
    {
        _transaction = transaction;
    }

    public Task CommitAsync()
    {
        return _transaction.CommitAsync();
    }

    public void Dispose()
    {
        _transaction.Dispose();
    }
}
