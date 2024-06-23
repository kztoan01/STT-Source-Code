using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace data.Migrations
{
    /// <inheritdoc />
    public partial class updateMusicEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "26110c47-6d53-4233-96f7-5b3beab9ba3f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "eb3f0343-5b6e-4d30-9da2-390cc58b4982");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ecfacaff-e3b9-4a1b-8242-75b3aa9f0eb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "albumId",
                table: "Musics",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AlterColumn<Guid>(
                name: "albumId",
                table: "Musics",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "26110c47-6d53-4233-96f7-5b3beab9ba3f", null, "Admin", "ADMIN" },
                    { "eb3f0343-5b6e-4d30-9da2-390cc58b4982", null, "Artist", "ARTIST" },
                    { "ecfacaff-e3b9-4a1b-8242-75b3aa9f0eb4", null, "User", "USER" }
                });
        }
    }
}
