using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MigrationTh : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.CreateTable(
                name: "OrderModel",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CustomerOrderRef = table.Column<int>(type: "int", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CustomersReference = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CustomerNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShippingAddress1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShippingZipCode = table.Column<int>(type: "int", nullable: false),
                    ShippingCity = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShippingCountry = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PriceWithoutTax = table.Column<double>(type: "float", nullable: false),
                    PriceWithTax = table.Column<double>(type: "float", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    SellerID = table.Column<int>(type: "int", nullable: false),
                    OfferID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrderStatus = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderModel", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderModel");

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CustomerNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomerOrderRef = table.Column<int>(type: "int", nullable: false),
                    CustomersReference = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OfferID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PriceWithTax = table.Column<double>(type: "float", nullable: false),
                    PriceWithoutTax = table.Column<double>(type: "float", nullable: false),
                    ProductId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    SellerID = table.Column<int>(type: "int", nullable: false),
                    ShippingAddress1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShippingCity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShippingCountry = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShippingZipCode = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                });
        }
    }
}
