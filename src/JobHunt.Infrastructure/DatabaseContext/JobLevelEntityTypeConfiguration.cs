using JobHunt.Core.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobHunt.Infrastructure.DatabaseContext;

public class JobLevelEntityTypeConfiguration : IEntityTypeConfiguration<JobLevel>
{
    public void Configure(EntityTypeBuilder<JobLevel> builder)
    {
        builder.HasData(
            new JobLevel()
            {
                JobLevelId = JobLevelKey.Intern,
                VietNameseName = "Thực Tập Sinh"
            },
            new JobLevel()
            {
                JobLevelId = JobLevelKey.Fresher,
                VietNameseName = "Nhân Viên Mới"
            },
            new JobLevel()
            {
                JobLevelId = JobLevelKey.Junior,
                VietNameseName = "Nhân Viên"
            },
            new JobLevel()
            {
                JobLevelId = JobLevelKey.Senior,
                VietNameseName = "Chuyên Viên"
            },
            new JobLevel()
            {
                JobLevelId = JobLevelKey.TeamLead,
                VietNameseName = "Trưởng Nhóm"
            },
            new JobLevel()
            {
                JobLevelId = JobLevelKey.Manager,
                VietNameseName = "Quản Lý",
            },
            new JobLevel()
            {
                JobLevelId = JobLevelKey.Director,
                VietNameseName = "Giám Đốc"
            }
        );
    }
}