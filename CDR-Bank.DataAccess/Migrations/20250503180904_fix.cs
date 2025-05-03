using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CDR_Bank.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class fix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_BankAccounts_UserId_IsMain",
                table: "BankAccounts");

            migrationBuilder.CreateIndex(
                name: "IX_BankAccounts_IsMain",
                table: "BankAccounts",
                column: "IsMain");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_BankAccounts_IsMain",
                table: "BankAccounts");

            migrationBuilder.CreateIndex(
                name: "IX_BankAccounts_UserId_IsMain",
                table: "BankAccounts",
                columns: new[] { "UserId", "IsMain" },
                unique: true);
        }
    }
}
