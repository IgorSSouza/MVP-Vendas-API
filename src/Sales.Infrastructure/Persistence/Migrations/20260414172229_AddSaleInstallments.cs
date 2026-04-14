using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sales.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddSaleInstallments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "InstallmentAmount",
                table: "Sales",
                type: "TEXT",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "Installments",
                table: "Sales",
                type: "INTEGER",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.Sql("""
                UPDATE "Sales"
                SET "Installments" = 1,
                    "InstallmentAmount" = "Total"
                WHERE "Installments" = 0
                   OR "InstallmentAmount" = 0;
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InstallmentAmount",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "Installments",
                table: "Sales");
        }
    }
}
