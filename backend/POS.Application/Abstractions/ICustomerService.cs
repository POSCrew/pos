using POS.Core.Sales;

namespace POS.Application.Abstractions;

public interface ICustomerService
{
    Task<Customer> Create(Customer customer);
    Task<Customer> Update(Customer customer);
    Task<Customer?> GetByID(int id);
    Task<List<Customer>> GetAll(int? page, int? pageSize);
    Task Remove(int id);
    Task<int> GetCount();
}