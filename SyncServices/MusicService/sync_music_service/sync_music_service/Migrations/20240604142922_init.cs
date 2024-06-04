using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace sync_music_service.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Artists",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    userId = table.Column<int>(type: "int", nullable: false),
                    artistDescription = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Artists", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Genres",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    genreName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    genreDescription = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genres", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Playlists",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    playlistName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    playlistDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    playlistPicture = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    createdDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    userId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Playlists", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Albums",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    albumTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    albumDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    releaseDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    artistId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Albums", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Albums_Artists_artistId",
                        column: x => x.artistId,
                        principalTable: "Artists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Musics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    musicTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    musicUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    musicPicture = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    musicPlays = table.Column<int>(type: "int", nullable: false),
                    musicDuration = table.Column<double>(type: "float", nullable: false),
                    releaseDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    albumId = table.Column<int>(type: "int", nullable: false),
                    artistId = table.Column<int>(type: "int", nullable: false),
                    genreId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Musics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Musics_Albums_albumId",
                        column: x => x.albumId,
                        principalTable: "Albums",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Musics_Artists_artistId",
                        column: x => x.artistId,
                        principalTable: "Artists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Musics_Genres_genreId",
                        column: x => x.genreId,
                        principalTable: "Genres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PlaylistMusics",
                columns: table => new
                {
                    playlistId = table.Column<int>(type: "int", nullable: false),
                    musicId = table.Column<int>(type: "int", nullable: false),
                    addedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    position = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlaylistMusics", x => new { x.playlistId, x.musicId });
                    table.ForeignKey(
                        name: "FK_PlaylistMusics_Musics_musicId",
                        column: x => x.musicId,
                        principalTable: "Musics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlaylistMusics_Playlists_playlistId",
                        column: x => x.playlistId,
                        principalTable: "Playlists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Albums_artistId",
                table: "Albums",
                column: "artistId");

            migrationBuilder.CreateIndex(
                name: "IX_Musics_albumId",
                table: "Musics",
                column: "albumId");

            migrationBuilder.CreateIndex(
                name: "IX_Musics_artistId",
                table: "Musics",
                column: "artistId");

            migrationBuilder.CreateIndex(
                name: "IX_Musics_genreId",
                table: "Musics",
                column: "genreId");

            migrationBuilder.CreateIndex(
                name: "IX_PlaylistMusics_musicId",
                table: "PlaylistMusics",
                column: "musicId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlaylistMusics");

            migrationBuilder.DropTable(
                name: "Musics");

            migrationBuilder.DropTable(
                name: "Playlists");

            migrationBuilder.DropTable(
                name: "Albums");

            migrationBuilder.DropTable(
                name: "Genres");

            migrationBuilder.DropTable(
                name: "Artists");
        }
    }
}
