using POS.Core.Sales;

namespace POS.Application.Abstractions;

public interface ISaleInvoiceService
{
    Task<SaleInvoice> Create(CreateSaleInvoiceRequest invoice, string userId);
    Task<SaleInvoice> Update(UpdateSaleInvoiceRequest invoice);
    Task<SaleInvoice?> GetByID(int id);
    Task<List<SaleInvoice>> GetAll(int? page, int? pageSize);
    Task Remove(int id);
    Task<int> GetCount();
}