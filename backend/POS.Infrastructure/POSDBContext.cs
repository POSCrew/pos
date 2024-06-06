using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using POS.Core.General;
using POS.Core.Sales;
using POS.Core.Inventory;

namespace POS.Infrastructure;

public sealed class POSDbContext : IdentityDbContext<POSUser>
{
    public POSDbContext(DbContextOptions<POSDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<IdentityRole>().HasData(
            new IdentityRole("Admin") { NormalizedName = "ADMIN", Id = "abe9231c-f123-4db4-8eaf-411c138f4abf" },
            new IdentityRole("Seller") { NormalizedName = "SELLER", Id = "6de4b080-5523-4396-8b89-19ba8c6350a3"}
        );

        builder.Entity<Customer>().HasData(Customer.DefaultCustomer);
        builder.Entity<Vendor>().HasData(Vendor.DefaultVendor);

        builder.Entity<SaleInvoice>().Property(s => s.TotalPrice).HasColumnType("Decimal(19, 4)");
        builder.Entity<SaleInvoiceItem>().Property(s => s.Fee).HasColumnType("Decimal(19, 4)");
        builder.Entity<SaleInvoiceItem>().Property(s => s.Quantity).HasColumnType("Decimal(19, 4)");
        builder.Entity<SaleInvoiceItem>().Property(s => s.Price).HasColumnType("Decimal(19, 4)");

        builder.Entity<PurchaseInvoice>().Property(s => s.TotalPrice).HasColumnType("Decimal(19, 4)");
        builder.Entity<PurchaseInvoiceItem>().Property(s => s.Fee).HasColumnType("Decimal(19, 4)");
        builder.Entity<PurchaseInvoiceItem>().Property(s => s.Quantity).HasColumnType("Decimal(19, 4)");
        builder.Entity<PurchaseInvoiceItem>().Property(s => s.Price).HasColumnType("Decimal(19, 4)");
        
        builder.Entity<InvoiceAveragePurchasePrice>().Property(s => s.AveragePurchasePrice).HasColumnType("Decimal(19, 4)");

        builder.Entity<Item>().Property(s => s.SalePrice).HasColumnType("Decimal(19, 4)");
    }

    public DbSet<Store> Stores { get; set; }
    public DbSet<SaleInvoice> SaleInvoices { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<PurchaseInvoice> PurchaseInvoices { get; set; }
    public DbSet<Vendor> Vendors { get; set; }
    public DbSet<Pricing> Pricings { get; set; }
    public DbSet<Item> Items { get; set; }
}