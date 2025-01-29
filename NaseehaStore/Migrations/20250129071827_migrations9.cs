using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NaseehaStore.Migrations
{
    /// <inheritdoc />
    public partial class migrations9 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsDelivered",
                table: "Orders",
                newName: "IsPrepared");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsPrepared",
                table: "Orders",
                newName: "IsDelivered");
        }
    }
}
