using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MusicTool.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class n_n_SongAndSinger : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Song_Singer_SingerId",
                table: "Song");

            migrationBuilder.DropIndex(
                name: "IX_Song_SingerId",
                table: "Song");

            migrationBuilder.DropColumn(
                name: "SingerId",
                table: "Song");

            migrationBuilder.CreateTable(
                name: "Song_Singer",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SongId = table.Column<int>(type: "int", nullable: true),
                    SingerId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Song_Singer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Song_Singer_Singer_SingerId",
                        column: x => x.SingerId,
                        principalTable: "Singer",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Song_Singer_Song_SongId",
                        column: x => x.SongId,
                        principalTable: "Song",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Song_Singer_SingerId",
                table: "Song_Singer",
                column: "SingerId");

            migrationBuilder.CreateIndex(
                name: "IX_Song_Singer_SongId",
                table: "Song_Singer",
                column: "SongId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Song_Singer");

            migrationBuilder.AddColumn<int>(
                name: "SingerId",
                table: "Song",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Song_SingerId",
                table: "Song",
                column: "SingerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Song_Singer_SingerId",
                table: "Song",
                column: "SingerId",
                principalTable: "Singer",
                principalColumn: "Id");
        }
    }
}
