using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace POS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeDefaultVendorAndCustomerName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "ID",
                keyValue: -1,
                column: "FirstName",
                value: "Default Customer");

            migrationBuilder.UpdateData(
                table: "Vendors",
                keyColumn: "ID",
                keyValue: -1,
                column: "FirstName",
                value: "Default Vendor");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "ID",
                keyValue: -1,
                column: "FirstName",
                value: "مشتری پیش فرض");

            migrationBuilder.UpdateData(
                table: "Vendors",
                keyColumn: "ID",
                keyValue: -1,
                column: "FirstName",
                value: "تامین کننده پیش فرض");
        }
    }
}
