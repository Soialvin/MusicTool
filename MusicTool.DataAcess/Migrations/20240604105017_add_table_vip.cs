using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MusicTool.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class add_table_vip : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VipExpires",
                table: "Account");

            migrationBuilder.CreateTable(
                name: "VIP",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VIPRegistrationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    VIPExpirationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AccountId = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VIP", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VIP_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_VIP_AccountId",
                table: "VIP",
                column: "AccountId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VIP");

            migrationBuilder.AddColumn<DateTime>(
                name: "VipExpires",
                table: "Account",
                type: "datetime2",
                nullable: true);
        }
    }
}
