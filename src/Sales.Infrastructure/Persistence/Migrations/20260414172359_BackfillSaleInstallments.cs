using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sales.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class BackfillSaleInstallments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                UPDATE "Sales"
                SET "Installments" = 1,
                    "InstallmentAmount" = "Total"
                WHERE "Installments" <= 0
                   OR "InstallmentAmount" <= 0;
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
