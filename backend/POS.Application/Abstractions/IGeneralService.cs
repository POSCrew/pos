using POS.Core.General;

namespace POS.Application.Abstractions;

public interface IGeneralService
{
    public static bool IsStoreInitialized;
    Task<bool> GetIsStoreInitializedFromDb();
    Task<Store> InitializeStore(Store store);
    Task<Store> GetStoreInfo();
}