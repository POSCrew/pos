using Microsoft.EntityFrameworkCore;
using POS.Application.Abstractions;
using POS.Application.Abstractions.Data;
using POS.Core.Inventory;

namespace POS.Application.Inventory;

public sealed class ItemService : IItemService
{
    private readonly ITransactionManager _transactionManager;
    private readonly IRepository<Item> _repository;

    public ItemService(IRepository<Item> repository, ITransactionManager transactionManager)
    {
        _repository = repository;
        _transactionManager = transactionManager;
    }

    public async Task<Item> Create(Item item)
    {
        ArgumentNullException.ThrowIfNull(item);

        item.ID = 0;

        item.Title = item.Title?.Trim() ?? string.Empty;
        item.Serial = item.Serial?.Trim() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(item.Title)) throw new ArgumentException($"{nameof(Item)}.{nameof(Item.Title)} is required");

        using var transaction = await _transactionManager.BeginTransactionAsync();

        if (_repository.Set.Any(i => i.Title == item.Title))
            throw new POSException("another item with this title already exists");

        if (item.Serial.Length > 0 && _repository.Set.Any(i => i.Serial == item.Serial))
            throw new POSException("another item with this serial already exists");

        _repository.Add(item);
        await _repository.SaveChangesAsync();
        await transaction.CommitAsync();
        return item;
    }

    public Task<Item?> GetByID(int id)
    {
        return _repository.Set.Where(i => i.ID == id).FirstOrDefaultAsync();
    }

    public Task<List<Item>> GetAll(int? page, int? pageSize)
    {
        if (page is null || pageSize is null || page < 0 || pageSize < 1)
            return _repository.Set.ToListAsync();

        return _repository.Set.Skip(page.Value * pageSize.Value).Take(pageSize.Value).ToListAsync();
    }

    public Task<int> GetCount()
    {
        return _repository.Set.CountAsync();
    }

    public Task Remove(int id)
    {
        _repository.Remove(id);
        return _repository.SaveChangesAsync();
    }
}
