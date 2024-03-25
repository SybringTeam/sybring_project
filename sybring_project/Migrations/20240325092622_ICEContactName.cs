using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace sybring_project.Migrations
{
    /// <inheritdoc />
    public partial class ICEContactName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ICEContactName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ICEContactName",
                table: "AspNetUsers");
        }
    }
}
