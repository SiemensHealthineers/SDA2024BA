using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NeuroMedia.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddFieldsToPatientsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfDiagnosis",
                table: "Patients",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "EmploymentStatus",
                table: "Patients",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "HighestEducation",
                table: "Patients",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Patients",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "PreviousBotulinumToxinApplication",
                table: "Patients",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "RapExamination",
                table: "Patients",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateOfDiagnosis",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "EmploymentStatus",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "HighestEducation",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "PreviousBotulinumToxinApplication",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "RapExamination",
                table: "Patients");
        }
    }
}
