using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobHunt.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Update_AddColumns_JobHunterTable_JobFilterTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "JobFilters",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsStarred",
                table: "JobFilters",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "JobFilterOwnerId",
                table: "JobFilters",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_JobFilters_JobFilterOwnerId",
                table: "JobFilters",
                column: "JobFilterOwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_JobFilters_AspNetUsers_JobFilterOwnerId",
                table: "JobFilters",
                column: "JobFilterOwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobFilters_AspNetUsers_JobFilterOwnerId",
                table: "JobFilters");

            migrationBuilder.DropIndex(
                name: "IX_JobFilters_JobFilterOwnerId",
                table: "JobFilters");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "JobFilters");

            migrationBuilder.DropColumn(
                name: "IsStarred",
                table: "JobFilters");

            migrationBuilder.DropColumn(
                name: "JobFilterOwnerId",
                table: "JobFilters");
        }
    }
}
