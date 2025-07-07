using JobHunt.Core.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobHunt.Infrastructure.DatabaseContext;

public class MajorEntityTypeConfiguration : IEntityTypeConfiguration<Major>
{
    public void Configure(EntityTypeBuilder<Major> builder)
    {
        builder.HasData(
            new Major()
            {
                MajorId = MajorKey.None,
                VietNameseName = "Không Có Chuyên Ngành",
            },
            new Major()
            {
                MajorId = MajorKey.Computer_Science,
                VietNameseName = "Khoa Học Máy Tính"
            },
            new Major()
            {
                MajorId = MajorKey.Software_Engineering,
                VietNameseName = "Phát Triển Phần Mềm"
            },
            new Major()
            {
                MajorId = MajorKey.Computer_Engineering,
                VietNameseName = "Kỹ Thuật Máy Tính",
            },
            new Major()
            {
                MajorId = MajorKey.Accounting,
                VietNameseName = "Kế Toán"
            }
        );
    }
}