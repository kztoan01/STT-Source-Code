using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace data.Migrations
{
    /// <inheritdoc />
    public partial class updatemusic : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0c7e78f2-62f3-4a67-b4a4-44dc7286a409");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "45bdae82-46bf-4203-9624-c03f6d8099cf");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "562eb858-f84c-44b2-b5e4-f2c847808b2a");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "43e2e300-96f9-4b51-8d32-410921bc6cb5", null, "Artist", "ARTIST" },
                    { "98e22b09-d91a-4e56-a001-d24d79095a1f", null, "Admin", "ADMIN" },
                    { "d95f722e-d0c8-4679-8bf0-0b4dc965b2e1", null, "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "43e2e300-96f9-4b51-8d32-410921bc6cb5");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "98e22b09-d91a-4e56-a001-d24d79095a1f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d95f722e-d0c8-4679-8bf0-0b4dc965b2e1");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0c7e78f2-62f3-4a67-b4a4-44dc7286a409", null, "Admin", "ADMIN" },
                    { "45bdae82-46bf-4203-9624-c03f6d8099cf", null, "Artist", "ARTIST" },
                    { "562eb858-f84c-44b2-b5e4-f2c847808b2a", null, "User", "USER" }
                });
        }
    }
}
