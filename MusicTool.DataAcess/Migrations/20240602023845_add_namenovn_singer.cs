using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MusicTool.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class add_namenovn_singer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NameNoDiacritics",
                table: "Singer",
                type: "Varchar(50)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NameNoDiacritics",
                table: "Singer");
        }
    }
}
