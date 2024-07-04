using POS.Application.Inventory;

namespace POS.Application.Abstractions;

public interface IInventoryReview
{
    Task<List<InventoryReviewItems>> GetItemSheetData(InventoryReviewFilter filter);
}