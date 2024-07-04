using Microsoft.EntityFrameworkCore;
using POS.Application.Abstractions;
using POS.Application.Abstractions.Data;
using POS.Core.Sales;

namespace POS.Application.Sales;

public sealed class CustomerService : ICustomerService
{
    private readonly ITransactionManager _transactionManager;
    private readonly IRepository<Customer> _repository;

    public CustomerService(IRepository<Customer> repository, ITransactionManager transactionManager)
    {
        _repository = repository;
        _transactionManager = transactionManager;
    }

    public async Task<Customer> Create(Customer customer)
    {
        ArgumentNullException.ThrowIfNull(customer);

        customer.ID = 0;

        Validate(customer);

        using var transaction = await _transactionManager.BeginTransactionAsync();

        ValidateInTransaction(customer);

        _repository.Add(customer);
        await _repository.SaveChangesAsync();
        await transaction.CommitAsync();
        return customer;
    }

    public async Task<Customer> Update(Customer customer)
    {
        ArgumentNullException.ThrowIfNull(customer);

        Validate(customer);

        using var transaction = await _transactionManager.BeginTransactionAsync();
        ValidateInTransaction(customer);

        _repository.Update(customer);
        await _repository.SaveChangesAsync();
        await transaction.CommitAsync();
        return customer;
    }

    private void Validate(Customer customer)
    {
        if(customer.ID == -1)
            throw new POSException("cannot edit default customer");

        customer.Code = customer.Code?.Trim() ?? string.Empty;
        customer.FirstName = customer.FirstName?.Trim() ?? string.Empty;
        customer.LastName = customer.LastName?.Trim() ?? string.Empty;
        customer.Address = customer.Address?.Trim() ?? string.Empty;
        customer.PhoneNumber = customer.PhoneNumber?.Trim() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(customer.Code))
            throw new POSException("customer.vode is required");

        if (string.IsNullOrWhiteSpace(customer.FirstName))
            throw new POSException("customer.firstname is required");
    }

    private void ValidateInTransaction(Customer customer)
    {
        if (_repository.Set.Any(i => (i.ID != customer.ID || customer.ID == 0) && i.Code == customer.Code))
            throw new POSException("another customer with this code already exists");
    }

    public async Task<Customer?> GetByID(int id, bool tracking = false)
    {
        var dc = Customer.DefaultCustomer;
        if(id == dc.ID)
        {
            _repository.ChangeStateToUnchanged(dc);
            return dc;
        }

        var customers = _repository.Set.Where(i => i.ID == id);
        if(!tracking)
            customers = customers.AsNoTracking();
        var customer = await customers.FirstOrDefaultAsync();

        if(customer is not null)
            _repository.ChangeStateToUnchanged(customer);

        return customer;
    }

    public Task<List<Customer>> GetAll(int? page, int? pageSize)
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
            throw new POSException("cannot edit default customer");

        _repository.Remove(id);
        return _repository.SaveChangesAsync();
    }
}
