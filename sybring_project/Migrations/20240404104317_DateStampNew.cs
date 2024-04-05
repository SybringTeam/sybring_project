using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace sybring_project.Migrations
{
    /// <inheritdoc />
    public partial class DateStampNew : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DataStamp",
                table: "Billings",
                newName: "DateStamp");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DateStamp",
                table: "Billings",
                newName: "DataStamp");
        }
    }
}
