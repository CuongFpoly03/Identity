using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Topic2.Migrations
{
    /// <inheritdoc />
    public partial class AddIdentitytables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b1d10575-f7f2-4353-9efb-81cae4dbe457",
                columns: new[] { "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "b1d10575-f7f2-4353-9efb-81cae4dbe457", "Reader", "READER" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b1d10575-f7f2-4353-9efb-81cae4dbe457",
                columns: new[] { "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { null, "Admin", "ADMIN" });
        }
    }
}
