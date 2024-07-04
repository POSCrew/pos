using POS.Core.General;

namespace POS.Application.Abstractions;

public interface IGeneralService
{
    public bool IsStoreInitialized { get; }
    Task<bool> GetIsStoreInitializedFromDb();
    Task<Store> InitializeStore(Store store);
    Task<Store> GetStoreInfo();
}