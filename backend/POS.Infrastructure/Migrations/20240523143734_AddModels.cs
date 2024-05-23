using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace POS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SalePrice = table.Column<decimal>(type: "Decimal(19,4)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Pricings",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pricings", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Stores",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InitializationDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stores", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Vendors",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vendors", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SaleInvoices",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Number = table.Column<int>(type: "int", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "Decimal(19,4)", nullable: false),
                    CustomerID = table.Column<int>(type: "int", nullable: false),
                    CreatorID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SaleInvoices", x => x.ID);
                    table.ForeignKey(
                        name: "FK_SaleInvoices_Customers_CustomerID",
                        column: x => x.CustomerID,
                        principalTable: "Customers",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InvoiceAveragePurchasePrice",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SaleInvoiceItem = table.Column<int>(type: "int", nullable: true),
                    PurchaseInvoiceItem = table.Column<int>(type: "int", nullable: true),
                    AveragePurchasePrice = table.Column<decimal>(type: "Decimal(19,4)", nullable: false),
                    PricingID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceAveragePurchasePrice", x => x.ID);
                    table.ForeignKey(
                        name: "FK_InvoiceAveragePurchasePrice_Pricings_PricingID",
                        column: x => x.PricingID,
                        principalTable: "Pricings",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "PurchaseInvoices",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Number = table.Column<int>(type: "int", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "Decimal(19,4)", nullable: false),
                    VendorID = table.Column<int>(type: "int", nullable: false),
                    CreatorID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseInvoices", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PurchaseInvoices_Vendors_VendorID",
                        column: x => x.VendorID,
                        principalTable: "Vendors",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SaleInvoiceItem",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RowNumber = table.Column<int>(type: "int", nullable: false),
                    ItemID = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<decimal>(type: "Decimal(19,4)", nullable: false),
                    Fee = table.Column<decimal>(type: "Decimal(19,4)", nullable: false),
                    Price = table.Column<decimal>(type: "Decimal(19,4)", nullable: false),
                    SaleInvoiceID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SaleInvoiceItem", x => x.ID);
                    table.ForeignKey(
                        name: "FK_SaleInvoiceItem_Items_ItemID",
                        column: x => x.ItemID,
                        principalTable: "Items",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SaleInvoiceItem_SaleInvoices_SaleInvoiceID",
                        column: x => x.SaleInvoiceID,
                        principalTable: "SaleInvoices",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "PurchaseInvoiceItem",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RowNumber = table.Column<int>(type: "int", nullable: false),
                    ItemID = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<decimal>(type: "Decimal(19,4)", nullable: false),
                    Fee = table.Column<decimal>(type: "Decimal(19,4)", nullable: false),
                    Price = table.Column<decimal>(type: "Decimal(19,4)", nullable: false),
                    PurchaseInvoiceID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseInvoiceItem", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PurchaseInvoiceItem_Items_ItemID",
                        column: x => x.ItemID,
                        principalTable: "Items",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PurchaseInvoiceItem_PurchaseInvoices_PurchaseInvoiceID",
                        column: x => x.PurchaseInvoiceID,
                        principalTable: "PurchaseInvoices",
                        principalColumn: "ID");
                });

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "ID", "Address", "FirstName", "LastName", "PhoneNumber" },
                values: new object[] { -1, null, "مشتری پیش فرض", null, null });

            migrationBuilder.InsertData(
                table: "Vendors",
                columns: new[] { "ID", "Address", "FirstName", "LastName", "PhoneNumber" },
                values: new object[] { -1, null, "تامین کننده پیش فرض", null, null });

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceAveragePurchasePrice_PricingID",
                table: "InvoiceAveragePurchasePrice",
                column: "PricingID");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseInvoiceItem_ItemID",
                table: "PurchaseInvoiceItem",
                column: "ItemID");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseInvoiceItem_PurchaseInvoiceID",
                table: "PurchaseInvoiceItem",
                column: "PurchaseInvoiceID");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseInvoices_VendorID",
                table: "PurchaseInvoices",
                column: "VendorID");

            migrationBuilder.CreateIndex(
                name: "IX_SaleInvoiceItem_ItemID",
                table: "SaleInvoiceItem",
                column: "ItemID");

            migrationBuilder.CreateIndex(
                name: "IX_SaleInvoiceItem_SaleInvoiceID",
                table: "SaleInvoiceItem",
                column: "SaleInvoiceID");

            migrationBuilder.CreateIndex(
                name: "IX_SaleInvoices_CustomerID",
                table: "SaleInvoices",
                column: "CustomerID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InvoiceAveragePurchasePrice");

            migrationBuilder.DropTable(
                name: "PurchaseInvoiceItem");

            migrationBuilder.DropTable(
                name: "SaleInvoiceItem");

            migrationBuilder.DropTable(
                name: "Stores");

            migrationBuilder.DropTable(
                name: "Pricings");

            migrationBuilder.DropTable(
                name: "PurchaseInvoices");

            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.DropTable(
                name: "SaleInvoices");

            migrationBuilder.DropTable(
                name: "Vendors");

            migrationBuilder.DropTable(
                name: "Customers");
        }
    }
}
