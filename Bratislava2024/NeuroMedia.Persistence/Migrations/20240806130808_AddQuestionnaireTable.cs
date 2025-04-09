using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace NeuroMedia.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddQuestionnaireTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "VisitType",
                table: "Visits",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Questionnaires",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    VisitId = table.Column<int>(type: "integer", nullable: false),
                    QuestionnaireType = table.Column<int>(type: "integer", nullable: false),
                    BlobPath = table.Column<string>(type: "text", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questionnaires", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Questionnaires_Visits_VisitId",
                        column: x => x.VisitId,
                        principalTable: "Visits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Questionnaires_VisitId",
                table: "Questionnaires",
                column: "VisitId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Questionnaires");

            migrationBuilder.DropColumn(
                name: "VisitType",
                table: "Visits");
        }
    }
}
