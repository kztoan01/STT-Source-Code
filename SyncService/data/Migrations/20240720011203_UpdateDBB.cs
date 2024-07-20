using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDBB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Followers_Artists_artistId",
                table: "Followers");

            migrationBuilder.DropForeignKey(
                name: "FK_Followers_AspNetUsers_userId",
                table: "Followers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Followers",
                table: "Followers");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6c85d0ac-7366-4a7e-bfcb-0b12d2b0f758");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a118c95a-0409-45f4-ade9-771108832048");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d14553b3-0059-4c5b-9380-4d1e7d9e8ccb");

            migrationBuilder.RenameTable(
                name: "Followers",
                newName: "Follower");

            migrationBuilder.RenameIndex(
                name: "IX_Followers_artistId",
                table: "Follower",
                newName: "IX_Follower_artistId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Follower",
                table: "Follower",
                columns: new[] { "userId", "artistId" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1ba1f330-9fb9-42dc-b034-2080fd3cf58e", null, "Artist", "ARTIST" },
                    { "50105ed8-a88e-42e5-a0ce-4606810dd78f", null, "User", "USER" },
                    { "ada9b9eb-4b10-4f67-ac8b-0e5cfea3d7cc", null, "Admin", "ADMIN" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Follower_Artists_artistId",
                table: "Follower",
                column: "artistId",
                principalTable: "Artists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Follower_AspNetUsers_userId",
                table: "Follower",
                column: "userId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Follower_Artists_artistId",
                table: "Follower");

            migrationBuilder.DropForeignKey(
                name: "FK_Follower_AspNetUsers_userId",
                table: "Follower");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Follower",
                table: "Follower");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1ba1f330-9fb9-42dc-b034-2080fd3cf58e");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "50105ed8-a88e-42e5-a0ce-4606810dd78f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ada9b9eb-4b10-4f67-ac8b-0e5cfea3d7cc");

            migrationBuilder.RenameTable(
                name: "Follower",
                newName: "Followers");

            migrationBuilder.RenameIndex(
                name: "IX_Follower_artistId",
                table: "Followers",
                newName: "IX_Followers_artistId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Followers",
                table: "Followers",
                columns: new[] { "userId", "artistId" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "6c85d0ac-7366-4a7e-bfcb-0b12d2b0f758", null, "Artist", "ARTIST" },
                    { "a118c95a-0409-45f4-ade9-771108832048", null, "User", "USER" },
                    { "d14553b3-0059-4c5b-9380-4d1e7d9e8ccb", null, "Admin", "ADMIN" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Followers_Artists_artistId",
                table: "Followers",
                column: "artistId",
                principalTable: "Artists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Followers_AspNetUsers_userId",
                table: "Followers",
                column: "userId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
