using POS.Core.Inventory;

namespace POS.Application.Abstractions;

public interface IPurchaseInvoiceService
{
    Task<PurchaseInvoice> Create(CreatePurchaseInvoiceRequest invoice, int userId);
    Task<PurchaseInvoice> Update(UpdatePurchaseInvoiceRequest invoice);
    Task<PurchaseInvoice?> GetByID(int id);
    Task<List<PurchaseInvoice>> GetAll(int? page, int? pageSize);
    Task Remove(int id);
    Task<int> GetCount();
}