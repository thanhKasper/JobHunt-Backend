using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobHunt.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "JobFilters",
                columns: table => new
                {
                    JobFilterId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FilterTitle = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Occupation = table.Column<int>(type: "int", nullable: false),
                    Level = table.Column<int>(type: "int", nullable: true),
                    YearsOfExperience = table.Column<int>(type: "int", nullable: true),
                    TechnicalKnowledge = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SoftSkills = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tools = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Languages = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobFilters", x => x.JobFilterId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JobFilters");
        }
    }
}
