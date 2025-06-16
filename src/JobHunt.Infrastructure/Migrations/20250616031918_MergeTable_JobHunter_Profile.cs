using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobHunt.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MergeTable_JobHunter_Profile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Profiles");

            migrationBuilder.AddColumn<string>(
                name: "AboutMe",
                table: "JobHunters",
                type: "nvarchar(512)",
                maxLength: 512,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "JobHunters",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Awards",
                table: "JobHunters",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Education",
                table: "JobHunters",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Major",
                table: "JobHunters",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "JobHunters",
                type: "nvarchar(24)",
                maxLength: 24,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "University",
                table: "JobHunters",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WorkingEmail",
                table: "JobHunters",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AboutMe",
                table: "JobHunters");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "JobHunters");

            migrationBuilder.DropColumn(
                name: "Awards",
                table: "JobHunters");

            migrationBuilder.DropColumn(
                name: "Education",
                table: "JobHunters");

            migrationBuilder.DropColumn(
                name: "Major",
                table: "JobHunters");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "JobHunters");

            migrationBuilder.DropColumn(
                name: "University",
                table: "JobHunters");

            migrationBuilder.DropColumn(
                name: "WorkingEmail",
                table: "JobHunters");

            migrationBuilder.CreateTable(
                name: "Profiles",
                columns: table => new
                {
                    ProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AboutMe = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    Awards = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Education = table.Column<int>(type: "int", nullable: false),
                    Major = table.Column<int>(type: "int", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: true),
                    University = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    WorkingEmail = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Profiles", x => x.ProfileId);
                    table.ForeignKey(
                        name: "FK_Profiles_JobHunters_UserId",
                        column: x => x.UserId,
                        principalTable: "JobHunters",
                        principalColumn: "JobHunterId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Profiles_UserId",
                table: "Profiles",
                column: "UserId",
                unique: true);
        }
    }
}
