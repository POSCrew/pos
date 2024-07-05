using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace POS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixForeignKeyNamesInAveragePurchasePrice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SaleInvoiceItem",
                table: "InvoiceAveragePurchasePrice",
                newName: "SaleInvoiceItemID");

            migrationBuilder.RenameColumn(
                name: "PurchaseInvoiceItem",
                table: "InvoiceAveragePurchasePrice",
                newName: "PurchaseInvoiceItemID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SaleInvoiceItemID",
                table: "InvoiceAveragePurchasePrice",
                newName: "SaleInvoiceItem");

            migrationBuilder.RenameColumn(
                name: "PurchaseInvoiceItemID",
                table: "InvoiceAveragePurchasePrice",
                newName: "PurchaseInvoiceItem");
        }
    }
}
