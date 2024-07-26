using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MusicTool.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class add_namenovn_song_type : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "NameNoDiacritics",
                table: "Song",
                type: "Varchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "NameNoDiacritics",
                table: "Song",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "Varchar(max)",
                oldNullable: true);
        }
    }
}
