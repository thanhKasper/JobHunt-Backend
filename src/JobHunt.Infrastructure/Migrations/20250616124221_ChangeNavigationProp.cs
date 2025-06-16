using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobHunt.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeNavigationProp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_JobHunters_UserJobHunterId",
                table: "Projects");

            migrationBuilder.RenameColumn(
                name: "UserJobHunterId",
                table: "Projects",
                newName: "ProjectOwnerJobHunterId");

            migrationBuilder.RenameIndex(
                name: "IX_Projects_UserJobHunterId",
                table: "Projects",
                newName: "IX_Projects_ProjectOwnerJobHunterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_JobHunters_ProjectOwnerJobHunterId",
                table: "Projects",
                column: "ProjectOwnerJobHunterId",
                principalTable: "JobHunters",
                principalColumn: "JobHunterId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_JobHunters_ProjectOwnerJobHunterId",
                table: "Projects");

            migrationBuilder.RenameColumn(
                name: "ProjectOwnerJobHunterId",
                table: "Projects",
                newName: "UserJobHunterId");

            migrationBuilder.RenameIndex(
                name: "IX_Projects_ProjectOwnerJobHunterId",
                table: "Projects",
                newName: "IX_Projects_UserJobHunterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_JobHunters_UserJobHunterId",
                table: "Projects",
                column: "UserJobHunterId",
                principalTable: "JobHunters",
                principalColumn: "JobHunterId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
