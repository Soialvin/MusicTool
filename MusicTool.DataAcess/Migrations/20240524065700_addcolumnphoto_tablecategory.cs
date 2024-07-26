﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MusicTool.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addcolumnphoto_tablecategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PhotoName",
                table: "Category",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhotoName",
                table: "Category");
        }
    }
}
