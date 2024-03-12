using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace sybring_project.Data.Migrations
{
    /// <inheritdoc />
    public partial class NewTimeProps : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectTimeHistory_Projects_ProjectIdId",
                table: "ProjectTimeHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectTimeHistory_TimeHistories_TimeIdId",
                table: "ProjectTimeHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_TimeHistoryUser_AspNetUsers_UsersId",
                table: "TimeHistoryUser");

            migrationBuilder.DropForeignKey(
                name: "FK_TimeHistoryUser_TimeHistories_TimeIdId",
                table: "TimeHistoryUser");

            migrationBuilder.RenameColumn(
                name: "UsersId",
                table: "TimeHistoryUser",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "TimeIdId",
                table: "TimeHistoryUser",
                newName: "TimeId");

            migrationBuilder.RenameIndex(
                name: "IX_TimeHistoryUser_UsersId",
                table: "TimeHistoryUser",
                newName: "IX_TimeHistoryUser_UserId");

            migrationBuilder.RenameColumn(
                name: "TimeIdId",
                table: "ProjectTimeHistory",
                newName: "TimeId");

            migrationBuilder.RenameColumn(
                name: "ProjectIdId",
                table: "ProjectTimeHistory",
                newName: "ProjectId");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectTimeHistory_TimeIdId",
                table: "ProjectTimeHistory",
                newName: "IX_ProjectTimeHistory_TimeId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectTimeHistory_Projects_ProjectId",
                table: "ProjectTimeHistory",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectTimeHistory_TimeHistories_TimeId",
                table: "ProjectTimeHistory",
                column: "TimeId",
                principalTable: "TimeHistories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TimeHistoryUser_AspNetUsers_UserId",
                table: "TimeHistoryUser",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TimeHistoryUser_TimeHistories_TimeId",
                table: "TimeHistoryUser",
                column: "TimeId",
                principalTable: "TimeHistories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectTimeHistory_Projects_ProjectId",
                table: "ProjectTimeHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectTimeHistory_TimeHistories_TimeId",
                table: "ProjectTimeHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_TimeHistoryUser_AspNetUsers_UserId",
                table: "TimeHistoryUser");

            migrationBuilder.DropForeignKey(
                name: "FK_TimeHistoryUser_TimeHistories_TimeId",
                table: "TimeHistoryUser");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "TimeHistoryUser",
                newName: "UsersId");

            migrationBuilder.RenameColumn(
                name: "TimeId",
                table: "TimeHistoryUser",
                newName: "TimeIdId");

            migrationBuilder.RenameIndex(
                name: "IX_TimeHistoryUser_UserId",
                table: "TimeHistoryUser",
                newName: "IX_TimeHistoryUser_UsersId");

            migrationBuilder.RenameColumn(
                name: "TimeId",
                table: "ProjectTimeHistory",
                newName: "TimeIdId");

            migrationBuilder.RenameColumn(
                name: "ProjectId",
                table: "ProjectTimeHistory",
                newName: "ProjectIdId");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectTimeHistory_TimeId",
                table: "ProjectTimeHistory",
                newName: "IX_ProjectTimeHistory_TimeIdId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectTimeHistory_Projects_ProjectIdId",
                table: "ProjectTimeHistory",
                column: "ProjectIdId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectTimeHistory_TimeHistories_TimeIdId",
                table: "ProjectTimeHistory",
                column: "TimeIdId",
                principalTable: "TimeHistories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TimeHistoryUser_AspNetUsers_UsersId",
                table: "TimeHistoryUser",
                column: "UsersId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TimeHistoryUser_TimeHistories_TimeIdId",
                table: "TimeHistoryUser",
                column: "TimeIdId",
                principalTable: "TimeHistories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
