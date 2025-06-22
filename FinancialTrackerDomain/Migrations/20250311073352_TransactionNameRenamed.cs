using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinancialTrackerDomain.Migrations
{
    /// <inheritdoc />
    public partial class TransactionNameRenamed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TransactionDate",
                table: "Transactions",
                newName: "CreatedDate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                table: "Transactions",
                newName: "TransactionDate");
        }
    }
}
