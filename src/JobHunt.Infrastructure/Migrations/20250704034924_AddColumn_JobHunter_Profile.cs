using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobHunt.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddColumn_JobHunter_Profile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MatchingRequirements",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "Awards",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "AboutMe",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                maxLength: 5000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(512)",
                oldMaxLength: 512,
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Achievement",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AchievementName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    JobHunterId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Achievement", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Achievement_AspNetUsers_JobHunterId",
                        column: x => x.JobHunterId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MatchingRequirement",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Requirement = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JobId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MatchingRequirement", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MatchingRequirement_Jobs_JobId",
                        column: x => x.JobId,
                        principalTable: "Jobs",
                        principalColumn: "JobId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Achievement_JobHunterId",
                table: "Achievement",
                column: "JobHunterId");

            migrationBuilder.CreateIndex(
                name: "IX_MatchingRequirement_JobId",
                table: "MatchingRequirement",
                column: "JobId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Achievement");

            migrationBuilder.DropTable(
                name: "MatchingRequirement");

            migrationBuilder.AddColumn<string>(
                name: "MatchingRequirements",
                table: "Jobs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AboutMe",
                table: "AspNetUsers",
                type: "nvarchar(512)",
                maxLength: 512,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldMaxLength: 5000,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Awards",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
