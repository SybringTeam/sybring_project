using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace sybring_project.Migrations
{
    /// <inheritdoc />
    public partial class ImageLink : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "Billings");

            migrationBuilder.AddColumn<string>(
                name: "ImageLink",
                table: "Billings",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageLink",
                table: "Billings");

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Billings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
