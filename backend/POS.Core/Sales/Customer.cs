using POS.Core.General;

namespace POS.Core.Sales;

public sealed class Customer : Party
{
    public static Customer DefaultCustomer = new Customer
    {
        ID = -1,
        FirstName = "مشتری پیش فرض"
    };
}