using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NaseehaStore.Migrations
{
    /// <inheritdoc />
    public partial class migrsations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsExportedToExcel",
                table: "Orders",
                type: "bit",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Admins",
                keyColumn: "Id",
                keyValue: 1,
                column: "Email",
                value: "admin@gmail.com");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsExportedToExcel",
                table: "Orders");

            migrationBuilder.UpdateData(
                table: "Admins",
                keyColumn: "Id",
                keyValue: 1,
                column: "Email",
                value: "admin@naseeha.com");
        }
    }
}
