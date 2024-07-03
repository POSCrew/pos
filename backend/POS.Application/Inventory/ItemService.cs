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

        Validate(item);

        using var transaction = await _transactionManager.BeginTransactionAsync();

        ValidateInTransaction(item);

        _repository.Add(item);
        await _repository.SaveChangesAsync();
        await transaction.CommitAsync();
        return item;
    }

    public async Task<Item> Update(Item item)
    {
        ArgumentNullException.ThrowIfNull(item);

        Validate(item);

        using var transaction = await _transactionManager.BeginTransactionAsync();
        ValidateInTransaction(item);

        _repository.Update(item);
        await _repository.SaveChangesAsync();
        await transaction.CommitAsync();
        return item;
    }

    private void Validate(Item item)
    {
        item.Title = item.Title?.Trim() ?? string.Empty;
        item.Serial = item.Serial?.Trim() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(item.Title))
            throw new POSException("item.title is required");
    }

    private void ValidateInTransaction(Item item)
    {
        if (_repository.Set.Any(i => i.Title == item.Title && (i.ID != item.ID || item.ID == 0)))
            throw new POSException("another item with this title already exists");

        if (item.Serial.Length > 0 && _repository.Set.Any(i => i.Serial == item.Serial && (i.ID != item.ID || item.ID == 0)))
            throw new POSException("another item with this serial already exists");
    }

    public Task<Item?> GetByID(int id, bool tracking = false)
    {
        var items = _repository.Set.Where(i => i.ID == id);
        if (!tracking)
            items = items.AsNoTracking();

        return items.FirstOrDefaultAsync();
    }

    public Task<List<Item>> GetAll(int? page, int? pageSize)
    {
        if (page is null || pageSize is null || page < 0 || pageSize < 1)
            return _repository.Set.AsNoTracking().ToListAsync();

        return _repository.Set.Skip(page.Value * pageSize.Value).Take(pageSize.Value).AsNoTracking().ToListAsync();
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
