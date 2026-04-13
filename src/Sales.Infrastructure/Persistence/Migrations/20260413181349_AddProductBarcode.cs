using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sales.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddProductBarcode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Barcode",
                table: "Products",
                type: "TEXT",
                maxLength: 100,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_CompanyId_Barcode",
                table: "Products",
                columns: new[] { "CompanyId", "Barcode" },
                unique: true,
                filter: "\"Barcode\" IS NOT NULL AND \"Barcode\" <> ''");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Products_CompanyId_Barcode",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Barcode",
                table: "Products");
        }
    }
}
