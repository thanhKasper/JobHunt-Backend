using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobHunt.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTable_JobFilter_ChangeMultiValueColumnToNewTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobFilters_JobLevels_LevelJobLevelId",
                table: "JobFilters");

            migrationBuilder.DropColumn(
                name: "Languages",
                table: "JobFilters");

            migrationBuilder.DropColumn(
                name: "SoftSkills",
                table: "JobFilters");

            migrationBuilder.DropColumn(
                name: "TechnicalKnowledge",
                table: "JobFilters");

            migrationBuilder.DropColumn(
                name: "Tools",
                table: "JobFilters");

            migrationBuilder.AlterColumn<int>(
                name: "LevelJobLevelId",
                table: "JobFilters",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Language",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CommunicationLanguage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JobFilterId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Language", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Language_JobFilters_JobFilterId",
                        column: x => x.JobFilterId,
                        principalTable: "JobFilters",
                        principalColumn: "JobFilterId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SoftSkill",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SoftSkillName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JobFilterId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SoftSkill", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SoftSkill_JobFilters_JobFilterId",
                        column: x => x.JobFilterId,
                        principalTable: "JobFilters",
                        principalColumn: "JobFilterId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SpecializedKnowledge",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Knowledge = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JobFilterId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpecializedKnowledge", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SpecializedKnowledge_JobFilters_JobFilterId",
                        column: x => x.JobFilterId,
                        principalTable: "JobFilters",
                        principalColumn: "JobFilterId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tool",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ToolName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JobFilterId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tool", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tool_JobFilters_JobFilterId",
                        column: x => x.JobFilterId,
                        principalTable: "JobFilters",
                        principalColumn: "JobFilterId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Language_JobFilterId",
                table: "Language",
                column: "JobFilterId");

            migrationBuilder.CreateIndex(
                name: "IX_SoftSkill_JobFilterId",
                table: "SoftSkill",
                column: "JobFilterId");

            migrationBuilder.CreateIndex(
                name: "IX_SpecializedKnowledge_JobFilterId",
                table: "SpecializedKnowledge",
                column: "JobFilterId");

            migrationBuilder.CreateIndex(
                name: "IX_Tool_JobFilterId",
                table: "Tool",
                column: "JobFilterId");

            migrationBuilder.AddForeignKey(
                name: "FK_JobFilters_JobLevels_LevelJobLevelId",
                table: "JobFilters",
                column: "LevelJobLevelId",
                principalTable: "JobLevels",
                principalColumn: "JobLevelId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobFilters_JobLevels_LevelJobLevelId",
                table: "JobFilters");

            migrationBuilder.DropTable(
                name: "Language");

            migrationBuilder.DropTable(
                name: "SoftSkill");

            migrationBuilder.DropTable(
                name: "SpecializedKnowledge");

            migrationBuilder.DropTable(
                name: "Tool");

            migrationBuilder.AlterColumn<int>(
                name: "LevelJobLevelId",
                table: "JobFilters",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "Languages",
                table: "JobFilters",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SoftSkills",
                table: "JobFilters",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TechnicalKnowledge",
                table: "JobFilters",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Tools",
                table: "JobFilters",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_JobFilters_JobLevels_LevelJobLevelId",
                table: "JobFilters",
                column: "LevelJobLevelId",
                principalTable: "JobLevels",
                principalColumn: "JobLevelId");
        }
    }
}
