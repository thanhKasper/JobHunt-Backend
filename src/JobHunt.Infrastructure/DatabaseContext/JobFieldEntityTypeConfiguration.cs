using JobHunt.Core.Domain.Entities;
using JobHunt.Core.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobHunt.Infrastructure.DatabaseContext;

public class JobFieldEntityTypeConfiguration : IEntityTypeConfiguration<JobField>
{
    public void Configure(EntityTypeBuilder<JobField> builder)
    {
        builder.HasData(
            new JobField()
            {
                JobFieldId = JobFieldKey.Information_Technology,
                VietNameseName = "Công Nghệ Thông Tin"
            },
            new()
            {
                JobFieldId = JobFieldKey.Software_Development,
                VietNameseName = "Phát Triển Phần Mềm"
            },
            new()
            {
                JobFieldId = JobFieldKey.Language_Education,
                VietNameseName = "Giáo Dục Ngôn Ngữ"
            });
    }
}