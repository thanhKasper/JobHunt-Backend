using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace JobHunt.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTable_JobField_JobLevel_Major_Education : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Occupation",
                table: "JobFilters",
                newName: "OccupationJobFieldId");

            migrationBuilder.RenameColumn(
                name: "Level",
                table: "JobFilters",
                newName: "LevelJobLevelId");

            migrationBuilder.RenameColumn(
                name: "Major",
                table: "AspNetUsers",
                newName: "MajorId");

            migrationBuilder.RenameColumn(
                name: "Education",
                table: "AspNetUsers",
                newName: "EducationId");

            migrationBuilder.CreateTable(
                name: "Educations",
                columns: table => new
                {
                    EducationId = table.Column<int>(type: "int", nullable: false),
                    VietNameseName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Educations", x => x.EducationId);
                });

            migrationBuilder.CreateTable(
                name: "JobFields",
                columns: table => new
                {
                    JobFieldId = table.Column<int>(type: "int", nullable: false),
                    VietNameseName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobFields", x => x.JobFieldId);
                });

            migrationBuilder.CreateTable(
                name: "JobLevels",
                columns: table => new
                {
                    JobLevelId = table.Column<int>(type: "int", nullable: false),
                    VietNameseName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobLevels", x => x.JobLevelId);
                });

            migrationBuilder.CreateTable(
                name: "Majors",
                columns: table => new
                {
                    MajorId = table.Column<int>(type: "int", nullable: false),
                    VietNameseName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Majors", x => x.MajorId);
                });

            migrationBuilder.InsertData(
                table: "Educations",
                columns: new[] { "EducationId", "VietNameseName" },
                values: new object[,]
                {
                    { 0, "Không Có Bằng Cấp" },
                    { 1, "Tốt Nghiệp Trung Học" },
                    { 2, "Tốt Nghiệp Cao Đẳng" },
                    { 3, "Tốt Nghiệp Cử Nhân/Kỹ Sư/Bác Sĩ" },
                    { 4, "Tốt Nghiệp Thạc Sĩ" },
                    { 5, "Tốt Nghiệp Tiến Sĩ" }
                });

            migrationBuilder.InsertData(
                table: "JobFields",
                columns: new[] { "JobFieldId", "VietNameseName" },
                values: new object[,]
                {
                    { 0, "Công Nghệ Thông Tin" },
                    { 1, "Phát Triển Phần Mềm" },
                    { 48, "Giáo Dục Ngôn Ngữ" }
                });

            migrationBuilder.InsertData(
                table: "JobLevels",
                columns: new[] { "JobLevelId", "VietNameseName" },
                values: new object[,]
                {
                    { 0, "Thực Tập Sinh" },
                    { 1, "Nhân Viên Mới" },
                    { 2, "Nhân Viên" },
                    { 3, "Chuyên Viên" },
                    { 4, "Trưởng Nhóm" },
                    { 5, "Quản Lý" },
                    { 6, "Giám Đốc" }
                });

            migrationBuilder.InsertData(
                table: "Majors",
                columns: new[] { "MajorId", "VietNameseName" },
                values: new object[,]
                {
                    { 0, "Không Có Chuyên Ngành" },
                    { 1, "Khoa Học Máy Tính" },
                    { 3, "Phát Triển Phần Mềm" },
                    { 7, "Kỹ Thuật Máy Tính" },
                    { 22, "Kế Toán" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_JobFilters_LevelJobLevelId",
                table: "JobFilters",
                column: "LevelJobLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_JobFilters_OccupationJobFieldId",
                table: "JobFilters",
                column: "OccupationJobFieldId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_EducationId",
                table: "AspNetUsers",
                column: "EducationId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_MajorId",
                table: "AspNetUsers",
                column: "MajorId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Educations_EducationId",
                table: "AspNetUsers",
                column: "EducationId",
                principalTable: "Educations",
                principalColumn: "EducationId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Majors_MajorId",
                table: "AspNetUsers",
                column: "MajorId",
                principalTable: "Majors",
                principalColumn: "MajorId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_JobFilters_JobFields_OccupationJobFieldId",
                table: "JobFilters",
                column: "OccupationJobFieldId",
                principalTable: "JobFields",
                principalColumn: "JobFieldId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_JobFilters_JobLevels_LevelJobLevelId",
                table: "JobFilters",
                column: "LevelJobLevelId",
                principalTable: "JobLevels",
                principalColumn: "JobLevelId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Educations_EducationId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Majors_MajorId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_JobFilters_JobFields_OccupationJobFieldId",
                table: "JobFilters");

            migrationBuilder.DropForeignKey(
                name: "FK_JobFilters_JobLevels_LevelJobLevelId",
                table: "JobFilters");

            migrationBuilder.DropTable(
                name: "Educations");

            migrationBuilder.DropTable(
                name: "JobFields");

            migrationBuilder.DropTable(
                name: "JobLevels");

            migrationBuilder.DropTable(
                name: "Majors");

            migrationBuilder.DropIndex(
                name: "IX_JobFilters_LevelJobLevelId",
                table: "JobFilters");

            migrationBuilder.DropIndex(
                name: "IX_JobFilters_OccupationJobFieldId",
                table: "JobFilters");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_EducationId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_MajorId",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "OccupationJobFieldId",
                table: "JobFilters",
                newName: "Occupation");

            migrationBuilder.RenameColumn(
                name: "LevelJobLevelId",
                table: "JobFilters",
                newName: "Level");

            migrationBuilder.RenameColumn(
                name: "MajorId",
                table: "AspNetUsers",
                newName: "Major");

            migrationBuilder.RenameColumn(
                name: "EducationId",
                table: "AspNetUsers",
                newName: "Education");
        }
    }
}
