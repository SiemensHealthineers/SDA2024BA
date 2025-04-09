using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NeuroMedia.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ChangeSexFromStringToEnum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
               name: "SexTemp",
               table: "Patients",
               type: "integer",
               nullable: true);

            migrationBuilder.Sql(@"
                UPDATE ""Patients""
                SET ""SexTemp"" = CASE ""Sex""
                    WHEN 'Male' THEN 0
                    WHEN 'Female' THEN 1
                    ELSE NULL
                END
            ");

            migrationBuilder.DropColumn(
                name: "Sex",
                table: "Patients");

            migrationBuilder.RenameColumn(
                name: "SexTemp",
                table: "Patients",
                newName: "Sex");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SexTemp",
                table: "Patients",
                type: "text",
                nullable: true);

            migrationBuilder.Sql(@"
                UPDATE ""Patients""
                SET ""SexTemp"" = CASE ""Sex""
                    WHEN 0 THEN 'Male'
                    WHEN 1 THEN 'Female'
                    ELSE NULL
                END
            ");

            migrationBuilder.DropColumn(
                name: "Sex",
                table: "Patients");

            migrationBuilder.RenameColumn(
                name: "SexTemp",
                table: "Patients",
                newName: "Sex");
        }
    }
}
