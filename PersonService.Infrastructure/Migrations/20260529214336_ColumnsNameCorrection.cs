using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PersonService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ColumnsNameCorrection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NationalCode_Value",
                table: "Persons",
                newName: "NationalCode");

            migrationBuilder.RenameColumn(
                name: "LastName_Value",
                table: "Persons",
                newName: "LastName");

            migrationBuilder.RenameColumn(
                name: "FirstName_Value",
                table: "Persons",
                newName: "FirstName");

            migrationBuilder.RenameColumn(
                name: "BirthDate_Value",
                table: "Persons",
                newName: "BirthDate");

            migrationBuilder.RenameIndex(
                name: "IX_Persons_NationalCode_Value",
                table: "Persons",
                newName: "IX_Persons_NationalCode");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NationalCode",
                table: "Persons",
                newName: "NationalCode_Value");

            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "Persons",
                newName: "LastName_Value");

            migrationBuilder.RenameColumn(
                name: "FirstName",
                table: "Persons",
                newName: "FirstName_Value");

            migrationBuilder.RenameColumn(
                name: "BirthDate",
                table: "Persons",
                newName: "BirthDate_Value");

            migrationBuilder.RenameIndex(
                name: "IX_Persons_NationalCode",
                table: "Persons",
                newName: "IX_Persons_NationalCode_Value");
        }
    }
}
