using Microsoft.EntityFrameworkCore;
using POS.Application.Abstractions;
using POS.Application.Abstractions.Data;
using POS.Core.General;

namespace POS.Application.General;

public sealed class GeneralService : IGeneralService
{
    private static bool? isStoreInitialized = null;
    private static object lockObject = new object();
    private static Store? StoreInfo = null;

    private readonly ITransactionManager _transactionManager;
    private readonly IRepository<Store> _repository;

    public bool IsStoreInitialized
    {
        get
        {
            if(isStoreInitialized is null)
            {
                lock(lockObject)
                {
                    if(isStoreInitialized is null)
                        isStoreInitialized = _repository.Set.Any();
                }
            }
            return isStoreInitialized.Value;
        }
        private set
        {
            if(!value)
                throw new ArgumentException($"value cannot be false in {nameof(isStoreInitialized)}.set");
            
            lock(lockObject)
            {
                isStoreInitialized = value;
            }
        }
    }

    public GeneralService(IRepository<Store> repository, ITransactionManager transactionManager)
    {
        _repository = repository;
        _transactionManager = transactionManager;
    }

    public Task<bool> GetIsStoreInitializedFromDb()
    {
        return _repository.Set.AnyAsync();
    }

    public async Task<Store> InitializeStore(Store store)
    {
        if(IsStoreInitialized)
            throw new POSException("store is already initialized");

        store.ID = 0;
        
        store.Address = store.Address?.Trim() ?? string.Empty;
        store.Title = store.Title?.Trim() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(store.Title))
            throw new POSException("store.title is required");

        store.InitializationDate = store.InitializationDate.Date;
        if(store.InitializationDate > DateTime.Now)
            throw new POSException("store initialization date cannot be in the future");
        if(store.InitializationDate.Year < 2000)
            throw new POSException("store initialization year cannot be less then 2000");

        using var t = await _transactionManager.BeginTransactionAsync();
        
        if(await GetIsStoreInitializedFromDb())
            throw new POSException("store is already initialized");

        _repository.Add(store);
        await _repository.SaveChangesAsync();
        
        await t.CommitAsync();
        IsStoreInitialized = true;

        return store;
    }

    public async Task<Store> GetStoreInfo()
    {
        return StoreInfo ??= await _repository.Set.AsNoTracking().FirstAsync();
    }
}
