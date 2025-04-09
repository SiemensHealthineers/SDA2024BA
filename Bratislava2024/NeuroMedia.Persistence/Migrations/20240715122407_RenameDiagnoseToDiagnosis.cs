using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NeuroMedia.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RenameDiagnoseToDiagnosis : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
            name: "Diagnose",
            table: "Patients",
            newName: "Diagnosis");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
            name: "Diagnosis",
            table: "Patients",
            newName: "Diagnose");
        }
    }
}
