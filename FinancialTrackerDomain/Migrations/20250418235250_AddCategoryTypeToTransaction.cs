using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinancialTrackerDomain.Migrations
{
    /// <inheritdoc />
    public partial class AddCategoryTypeToTransaction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CategoryType",
                table: "Transactions",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "LimitAmount",
                table: "Budgets",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CategoryType",
                table: "Transactions");

            migrationBuilder.AlterColumn<decimal>(
                name: "LimitAmount",
                table: "Budgets",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);
        }
    }
}
