using POS.Core.Inventory;

namespace POS.Application.Abstractions;

public interface IItemService
{
    Task<Item> Create(Item item);
    Task<Item> Update(Item item);
    Task<Item?> GetByID(int id, bool tracking = false);
    Task<List<Item>> GetAll(int? page, int? pageSize);
    Task Remove(int id);
    Task<int> GetCount();
    Task ValidateItemQuantity();
}