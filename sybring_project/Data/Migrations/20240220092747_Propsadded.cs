using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace sybring_project.Data.Migrations
{
    /// <inheritdoc />
    public partial class Propsadded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageLink",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "UserIncome",
                table: "AspNetUsers",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageLink",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UserIncome",
                table: "AspNetUsers");
        }
    }
}
