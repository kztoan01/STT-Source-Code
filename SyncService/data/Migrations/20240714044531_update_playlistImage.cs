using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace data.Migrations
{
    /// <inheritdoc />
    public partial class update_playlistImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0a31521c-d70d-42a1-a132-190ef2ab837f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "53d98016-8cf1-4ce7-8cf8-74ef626b55ef");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c265bb4d-0166-4617-9abf-4d7a88cb3d71");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Playlists");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "47cca955-9758-42b3-92bc-e02d30838400", null, "Admin", "ADMIN" },
                    { "89ee947b-8d53-4b77-926d-55b04fe4d336", null, "Artist", "ARTIST" },
                    { "df8a27ed-77db-4a1e-b57e-ca574468f0a8", null, "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "47cca955-9758-42b3-92bc-e02d30838400");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "89ee947b-8d53-4b77-926d-55b04fe4d336");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "df8a27ed-77db-4a1e-b57e-ca574468f0a8");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Playlists",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0a31521c-d70d-42a1-a132-190ef2ab837f", null, "User", "USER" },
                    { "53d98016-8cf1-4ce7-8cf8-74ef626b55ef", null, "Admin", "ADMIN" },
                    { "c265bb4d-0166-4617-9abf-4d7a88cb3d71", null, "Artist", "ARTIST" }
                });
        }
    }
}
