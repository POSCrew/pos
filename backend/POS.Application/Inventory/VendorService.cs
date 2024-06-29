using Microsoft.EntityFrameworkCore;
using POS.Application.Abstractions;
using POS.Application.Abstractions.Data;
using POS.Core.Inventory;

namespace POS.Application.Inventory;

public sealed class VendorService : IVendorService
{
    private readonly ITransactionManager _transactionManager;
    private readonly IRepository<Vendor> _repository;

    public VendorService(IRepository<Vendor> repository, ITransactionManager transactionManager)
    {
        _repository = repository;
        _transactionManager = transactionManager;
    }

    public async Task<Vendor> Create(Vendor vendor)
    {
        ArgumentNullException.ThrowIfNull(vendor);

        vendor.ID = 0;

        Validate(vendor);

        using var transaction = await _transactionManager.BeginTransactionAsync();

        ValidateInTransaction(vendor);

        _repository.Add(vendor);
        await _repository.SaveChangesAsync();
        await transaction.CommitAsync();
        return vendor;
    }

    public async Task<Vendor> Update(Vendor vendor)
    {
        ArgumentNullException.ThrowIfNull(vendor);

        Validate(vendor);

        using var transaction = await _transactionManager.BeginTransactionAsync();
        ValidateInTransaction(vendor);

        _repository.Update(vendor);
        await _repository.SaveChangesAsync();
        await transaction.CommitAsync();
        return vendor;
    }

    private void Validate(Vendor vendor)
    {
        if(vendor.ID == -1)
            throw new POSException("cannot edit default vendor");

        vendor.Code = vendor.Code?.Trim() ?? string.Empty;
        vendor.FirstName = vendor.FirstName?.Trim() ?? string.Empty;
        vendor.LastName = vendor.LastName?.Trim() ?? string.Empty;
        vendor.Address = vendor.Address?.Trim() ?? string.Empty;
        vendor.PhoneNumber = vendor.PhoneNumber?.Trim() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(vendor.Code))
            throw new POSException("vendor.vode is required");

        if (string.IsNullOrWhiteSpace(vendor.FirstName))
            throw new POSException("vendor.firstname is required");
    }

    private void ValidateInTransaction(Vendor vendor)
    {
        if (_repository.Set.Any(i => (i.ID != vendor.ID || vendor.ID == 0) && i.Code == vendor.Code))
            throw new POSException("another vendor with this code already exists");
    }

    public Task<Vendor?> GetByID(int id)
    {
        return _repository.Set.Where(i => i.ID == id).AsNoTracking().FirstOrDefaultAsync();
    }

    public Task<List<Vendor>> GetAll(int? page, int? pageSize)
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
        if(id == -1)
            throw new POSException("cannot edit default vendor");

        _repository.Remove(id);
        return _repository.SaveChangesAsync();
    }
}
