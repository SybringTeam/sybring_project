using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace sybring_project.Data.Migrations
{
    /// <inheritdoc />
    public partial class TimeHistoryProps : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DateTime",
                table: "TimeHistories",
                newName: "Schedule");

            migrationBuilder.AddColumn<decimal>(
                name: "AnnualLeave",
                table: "TimeHistories",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "AttendanceTime",
                table: "TimeHistories",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Childcare",
                table: "TimeHistories",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "EndBreak",
                table: "TimeHistories",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "EndWork",
                table: "TimeHistories",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<decimal>(
                name: "FlexiTime",
                table: "TimeHistories",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "InconvenientHours",
                table: "TimeHistories",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "LeaveOfAbsence",
                table: "TimeHistories",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "MoreTime",
                table: "TimeHistories",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Overtime",
                table: "TimeHistories",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "SickLeave",
                table: "TimeHistories",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "StartBreak",
                table: "TimeHistories",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "StartWork",
                table: "TimeHistories",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<decimal>(
                name: "TotalWorkingHours",
                table: "TimeHistories",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "WorkingHours",
                table: "TimeHistories",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AnnualLeave",
                table: "TimeHistories");

            migrationBuilder.DropColumn(
                name: "AttendanceTime",
                table: "TimeHistories");

            migrationBuilder.DropColumn(
                name: "Childcare",
                table: "TimeHistories");

            migrationBuilder.DropColumn(
                name: "EndBreak",
                table: "TimeHistories");

            migrationBuilder.DropColumn(
                name: "EndWork",
                table: "TimeHistories");

            migrationBuilder.DropColumn(
                name: "FlexiTime",
                table: "TimeHistories");

            migrationBuilder.DropColumn(
                name: "InconvenientHours",
                table: "TimeHistories");

            migrationBuilder.DropColumn(
                name: "LeaveOfAbsence",
                table: "TimeHistories");

            migrationBuilder.DropColumn(
                name: "MoreTime",
                table: "TimeHistories");

            migrationBuilder.DropColumn(
                name: "Overtime",
                table: "TimeHistories");

            migrationBuilder.DropColumn(
                name: "SickLeave",
                table: "TimeHistories");

            migrationBuilder.DropColumn(
                name: "StartBreak",
                table: "TimeHistories");

            migrationBuilder.DropColumn(
                name: "StartWork",
                table: "TimeHistories");

            migrationBuilder.DropColumn(
                name: "TotalWorkingHours",
                table: "TimeHistories");

            migrationBuilder.DropColumn(
                name: "WorkingHours",
                table: "TimeHistories");

            migrationBuilder.RenameColumn(
                name: "Schedule",
                table: "TimeHistories",
                newName: "DateTime");
        }
    }
}
