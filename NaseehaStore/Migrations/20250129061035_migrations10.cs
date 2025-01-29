using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NaseehaStore.Migrations
{
    /// <inheritdoc />
    public partial class migrations10 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Admins",
                keyColumn: "Id",
                keyValue: 1,
                column: "Email",
                value: "admin@naseeha.com");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Admins",
                keyColumn: "Id",
                keyValue: 1,
                column: "Email",
                value: "admin@gmail.com");
        }
    }
}
