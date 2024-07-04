using POS.Core.Inventory;

namespace POS.Application.Abstractions;

public interface IVendorService
{
    Task<Vendor> Create(Vendor vendor);
    Task<Vendor> Update(Vendor vendor);
    Task<Vendor?> GetByID(int id, bool tracking = false);
    Task<List<Vendor>> GetAll(int? page, int? pageSize);
    Task Remove(int id);
    Task<int> GetCount();
}