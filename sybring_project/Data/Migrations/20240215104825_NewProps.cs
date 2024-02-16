using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace sybring_project.Data.Migrations
{
    /// <inheritdoc />
    public partial class NewProps : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BillingId",
                table: "Projects",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ProjectTimeHistory",
                columns: table => new
                {
                    ProjectIdId = table.Column<int>(type: "int", nullable: false),
                    TimeIdId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectTimeHistory", x => new { x.ProjectIdId, x.TimeIdId });
                    table.ForeignKey(
                        name: "FK_ProjectTimeHistory_Projects_ProjectIdId",
                        column: x => x.ProjectIdId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectTimeHistory_TimeHistories_TimeIdId",
                        column: x => x.TimeIdId,
                        principalTable: "TimeHistories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Projects_BillingId",
                table: "Projects",
                column: "BillingId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTimeHistory_TimeIdId",
                table: "ProjectTimeHistory",
                column: "TimeIdId");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Billings_BillingId",
                table: "Projects",
                column: "BillingId",
                principalTable: "Billings",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Billings_BillingId",
                table: "Projects");

            migrationBuilder.DropTable(
                name: "ProjectTimeHistory");

            migrationBuilder.DropIndex(
                name: "IX_Projects_BillingId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "BillingId",
                table: "Projects");
        }
    }
}
