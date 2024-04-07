﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace sybring_project.Migrations
{
    /// <inheritdoc />
    public partial class SelectedUserId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SelectedUserId",
                table: "Billings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SelectedUserId",
                table: "Billings");
        }
    }
}
