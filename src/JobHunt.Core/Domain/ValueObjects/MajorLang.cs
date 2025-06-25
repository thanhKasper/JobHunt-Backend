using Microsoft.EntityFrameworkCore;

namespace JobHunt.Core.Domain.ValueObjects;

[PrimaryKey(nameof(MajorId), nameof(LanguageCode))]
public class MajorFieldLang
{
    public Major? MajorId { get; set; }
    public string? LanguageCode { get; set; }
    public string? Name { get; set; }
}