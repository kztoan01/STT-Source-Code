using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace data.Migrations
{
    /// <inheritdoc />
    public partial class MusicHistory3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4977e3a9-c92b-45d4-a2b1-895c4a322e19");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b5cd9802-5223-4ba5-ba8d-6aefc4a372ff");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ea4d485d-43ac-4c00-b200-b077c6e9f3da");

            migrationBuilder.CreateTable(
                name: "MusicHistory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MusicId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ListenTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MusicHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MusicHistory_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MusicHistory_Musics_MusicId",
                        column: x => x.MusicId,
                        principalTable: "Musics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0cde3ea4-069e-411f-b916-615664497daa", null, "User", "USER" },
                    { "496aecd8-7fc6-4941-9669-3748c2fd25a1", null, "Artist", "ARTIST" },
                    { "ffedc1fd-9a1f-464f-a059-e27b62101c69", null, "Admin", "ADMIN" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_MusicHistory_MusicId",
                table: "MusicHistory",
                column: "MusicId");

            migrationBuilder.CreateIndex(
                name: "IX_MusicHistory_UserId",
                table: "MusicHistory",
                column: "UserId");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropTable(
                name: "MusicHistory");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0cde3ea4-069e-411f-b916-615664497daa");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "496aecd8-7fc6-4941-9669-3748c2fd25a1");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ffedc1fd-9a1f-464f-a059-e27b62101c69");

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
    }
}
