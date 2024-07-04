using POS.Core.General;

namespace POS.Core.Sales;

public sealed class Customer : Party
{
    public static Customer DefaultCustomer => new()
    {
        ID = -1,
        Code = "0",
        FirstName = "مشتری پیش فرض"
    };
}