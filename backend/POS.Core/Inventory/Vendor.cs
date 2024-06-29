using POS.Core.General;

namespace POS.Core.Inventory;

public sealed class Vendor : Party
{
    public static Vendor DefaultVendor = new Vendor
    {
        ID = -1,
        Code = "0",
        FirstName = "تامین کننده پیش فرض"
    };
}