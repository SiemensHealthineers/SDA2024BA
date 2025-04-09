using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NeuroMedia.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class DiagnosesChangedFromStringToEnum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            
            migrationBuilder.AddColumn<int>(
                name: "TempDiagnosis",
                table: "Patients",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            
            migrationBuilder.Sql(
                @"UPDATE ""Patients""
                  SET ""TempDiagnosis"" = CASE
                      WHEN ""Diagnosis"" = 'Cervical Dystonia' THEN 0
                      WHEN ""Diagnosis"" = 'Other' THEN 1
                  END;");

            
            migrationBuilder.DropColumn(
                name: "Diagnosis",
                table: "Patients");

            
            migrationBuilder.RenameColumn(
                name: "TempDiagnosis",
                table: "Patients",
                newName: "Diagnosis");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            
            migrationBuilder.AddColumn<string>(
                name: "Diagnosis",
                table: "Patients",
                type: "text",
                nullable: true);

           
            migrationBuilder.Sql(
                @"UPDATE ""Patients""
                  SET ""Diagnosis"" = CASE
                      WHEN ""Diagnosis"" = 0 THEN 'Cervical Dystonia'
                      WHEN ""Diagnosis"" = 1 THEN 'Other'
                  END;");

            
            migrationBuilder.DropColumn(
                name: "Diagnosis",
                table: "Patients");

           
            migrationBuilder.RenameColumn(
                name: "Diagnosis",
                table: "Patients",
                newName: "Diagnosis");
        }
    }
}
