using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CDR_Bank.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Edit_ContartInfo_add_Names : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Email",
                table: "ContactInfos",
                newName: "MiddleName");

            migrationBuilder.AddColumn<DateTime>(
                name: "BirthDate",
                table: "ContactInfos",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "ContactInfos",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "ContactInfos",
                type: "longtext",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BirthDate",
                table: "ContactInfos");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "ContactInfos");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "ContactInfos");

            migrationBuilder.RenameColumn(
                name: "MiddleName",
                table: "ContactInfos",
                newName: "Email");
        }
    }
}
