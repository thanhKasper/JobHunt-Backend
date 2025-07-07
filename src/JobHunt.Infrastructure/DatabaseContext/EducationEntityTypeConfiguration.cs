using JobHunt.Core.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobHunt.Infrastructure.DatabaseContext;

public class EducationEntityTypeConfiguration : IEntityTypeConfiguration<Education>
{
    public void Configure(EntityTypeBuilder<Education> builder)
    {
        builder.HasData(
            new Education()
            {
                EducationId = EducationKey.None,
                VietNameseName = "Không Có Bằng Cấp"
            },
            new Education()
            {
                EducationId = EducationKey.AssociateDegree,
                VietNameseName = "Tốt Nghiệp Cao Đẳng"
            },
            new Education()
            {
                EducationId = EducationKey.HighSchool,
                VietNameseName = "Tốt Nghiệp Trung Học"
            },
            new Education()
            {
                EducationId = EducationKey.BachelorDegree,
                VietNameseName = "Tốt Nghiệp Cử Nhân/Kỹ Sư/Bác Sĩ"
            },
            new Education()
            {
                EducationId = EducationKey.MasterDegree,
                VietNameseName = "Tốt Nghiệp Thạc Sĩ"
            },
            new Education()
            {
                EducationId = EducationKey.DoctorateDegree,
                VietNameseName = "Tốt Nghiệp Tiến Sĩ"
            }
        );
    }
}