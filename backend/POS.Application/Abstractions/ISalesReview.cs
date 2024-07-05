using POS.Application.Sales;

namespace POS.Application.Abstractions;

public interface ISalesReview
{
    Task<List<SalesReviewProfit>> GetProfitSheetData(SalesReviewFilter filter);
}
