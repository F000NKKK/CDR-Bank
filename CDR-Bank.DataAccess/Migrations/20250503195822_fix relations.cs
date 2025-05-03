using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CDR_Bank.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class fixrelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_BankAccounts_TelephoneNumber",
                table: "BankAccounts");

            migrationBuilder.DropColumn(
                name: "TelephoneNumber",
                table: "BankAccounts");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TelephoneNumber",
                table: "BankAccounts",
                type: "varchar(255)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_BankAccounts_TelephoneNumber",
                table: "BankAccounts",
                column: "TelephoneNumber");
        }
    }
}
