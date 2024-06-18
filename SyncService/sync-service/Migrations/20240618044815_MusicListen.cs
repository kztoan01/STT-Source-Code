using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace sync_service.Migrations
{
    /// <inheritdoc />
    public partial class MusicListen : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "52a583df-e080-48ec-8c87-b14262651b6d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "58a54d9f-72d2-4fe8-8b96-c121b2e866d2");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f4ac1375-a1d4-445b-b9d2-db9f3401a221");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1af09706-0b06-4c62-806b-372640d275af", null, "Artist", "ARTIST" },
                    { "35a113ef-a19b-4ed7-8fe0-5f4674a3d4ef", null, "User", "USER" },
                    { "5c484637-53f6-4463-b36f-38e047d91b86", null, "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1af09706-0b06-4c62-806b-372640d275af");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "35a113ef-a19b-4ed7-8fe0-5f4674a3d4ef");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5c484637-53f6-4463-b36f-38e047d91b86");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "52a583df-e080-48ec-8c87-b14262651b6d", null, "Admin", "ADMIN" },
                    { "58a54d9f-72d2-4fe8-8b96-c121b2e866d2", null, "Artist", "ARTIST" },
                    { "f4ac1375-a1d4-445b-b9d2-db9f3401a221", null, "User", "USER" }
                });
        }
    }
}
